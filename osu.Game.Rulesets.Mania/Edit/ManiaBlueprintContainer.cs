// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Mania.Edit.Blueprints;
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Mania.Edit.Blueprints.Components;
using osuTK.Input;
using Random = System.Random;

namespace osu.Game.Rulesets.Mania.Edit
{
    public class ManiaBlueprintContainer : ComposeBlueprintContainer
    {
        public ManiaBlueprintContainer(HitObjectComposer composer)
            : base(composer)
        {
        }

        public override HitObjectSelectionBlueprint CreateHitObjectBlueprintFor(HitObject hitObject)
        {
            switch (hitObject)
            {
                case Note note:
                    return new NoteSelectionBlueprint(note);

                case HoldNote holdNote:
                    return new HoldNoteSelectionBlueprint(holdNote);
            }

            return base.CreateHitObjectBlueprintFor(hitObject);
        }



        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (check5kManiaQuickPlacementKey(e.Key, out int column))
            {
                Logger.Log("maniaIndex = " + column.ToString());
                place(column);
                return true;
            }
            if (e.Key == Key.B)
            {
                convertToXYXY();
            }
            if (e.Key == Key.N)
            {
                shuffle();
            }
            if (e.Key == Key.M)
            {
                convertToMid();
            }
            if (e.Key == Key.Comma)
            {
                clearWeirdStructure();
            }
            return base.OnKeyDown(e);
        }

        private bool check5kManiaQuickPlacementKey(Key key, out int index)
        {
            switch (key)
            {
                case Key.A:
                    index = 0;
                    break;

                case Key.S:
                    index = 1;
                    break;

                case Key.D:
                    index = 3;
                    break;

                case Key.F:
                    index = 4;
                    break;

                case Key.X:
                    index = 2;
                    break;

                default:
                    index = -1;
                    break;
            }

            return index >= 0;
        }

        private void place(int column)
        {
            Vector2 pos = inputManager.CurrentState.Mouse.Position;
            ManiaEditorPlayfield playfield = Composer.Playfield as ManiaEditorPlayfield;
            float x = playfield.GetColumn(column).ScreenSpacePositionAtTime(EditorClock.CurrentTime).X;
            Logger.Log("x is " + x.ToString());
            pos.X = x;

            var snapResult = Composer.SnapScreenSpacePositionToValidTime(pos);

            // if no time was found from positional snapping, we should still quantize to the beat.
            snapResult.Time ??= Beatmap.SnapTime(EditorClock.CurrentTime, null);

            currentPlacement.UpdateTimeAndPosition(snapResult);
            currentPlacement.EndPlacement(true);
        }



        void convertToXYXY()
        {
            ManiaEditorPlayfield playfield = Composer.Playfield as ManiaEditorPlayfield;
            bool even = false;
            Beatmap.PerformOnSelection(h =>
            {
                if (even)
                {
                    if (h is ManiaHitObject)
                    {
                        ManiaHitObject mh = (ManiaHitObject)h;
                        playfield.Remove(h);
                        mh.Column = 4 - mh.Column;
                        playfield.Add(h);
                    }
                }
                even = !even;
            });
            Logger.Log("Converted to XYXY.");
        }

        void convertToMid()
        {
            ManiaEditorPlayfield playfield = Composer.Playfield as ManiaEditorPlayfield;
            Beatmap.PerformOnSelection(h =>
            {
                if (h is ManiaHitObject)
                {
                    ManiaHitObject mh = (ManiaHitObject)h;
                    if (mh.Column != 2)
                    {
                        playfield.Remove(h);
                        mh.Column = 2;
                        playfield.Add(h);
                    }
                }
            });
            Logger.Log("Converted to Mid.");
        }

        void shuffle()
        {
            // Initialize temp vars.
            int lastOriginalColumn = 0;
            int originalColumn = 0;
            int lastColumn = 0;
            int column = 0;
            int pointer = 0;
            int index = 0;
            int[] map = new int[3];

            // Get playfield.
            ManiaEditorPlayfield playfield = Composer.Playfield as ManiaEditorPlayfield;

            // Iterate through list of hit objects.
            Beatmap.PerformOnSelection(h =>
            {
                ManiaHitObject mh = (ManiaHitObject)h;
                originalColumn = mh.Column;

                // Exclude middle.
                if (originalColumn != 2)
                {
                    // Generate a random valid column, but preserves stack.
                    if (originalColumn == lastOriginalColumn)
                    {
                        column = lastColumn;
                    }
                    else
                    {
                        pointer = 0;
                        index = new Random().Next(3);
                        for (int i = 0; i < 5; i++)
                        {
                            if (i != lastColumn && i != 2)
                            {
                                map[pointer] = i;
                                pointer++;
                            }
                        }
                        column = map[index];
                    }

                    // Set the column.
                    playfield.Remove(h);
                    mh.Column = column;
                    playfield.Add(h);

                    lastColumn = column;
                    lastOriginalColumn = originalColumn;
                }

            });
            Logger.Log("Shuffled.");
        }

        void clearWeirdStructure()
        {
            // Initialize temp vars.
            double lastTime = -1;
            double thisTime = -1;
            int leastColumn = -1;
            int lastColumn = -1;
            int thisColumn = -1;
            bool lastClose = false;
            bool thisClose = false;

            // Get playfield.
            ManiaEditorPlayfield playfield = Composer.Playfield as ManiaEditorPlayfield;

            // Iterate through list of hit objects.
            Beatmap.PerformOnSelection(h =>
            {
                ManiaHitObject mh = (ManiaHitObject)h;
                leastColumn = lastColumn;
                lastColumn = thisColumn;
                lastTime = thisTime;
                thisColumn = mh.Column;
                thisTime = mh.StartTime;

                // If interval is less than a 16th beat of 140 bpm, it will be detected as "close enough"
                lastClose = thisClose;
                thisClose = thisTime - lastTime < 60000d / (140 * 4) && thisTime - lastTime > 1;

                if (leastColumn != 2 && lastColumn != 2 && thisColumn != 2 && lastClose && thisClose)
                {
                    if (isVertical(leastColumn) == isVertical(lastColumn))
                    {
                        playfield.Remove(h);
                        mh.Column = leastColumn;
                        playfield.Add(h);
                    }
                    else
                    {
                        playfield.Remove(h);
                        mh.Column = 4 - leastColumn;
                        playfield.Add(h);
                    }
                }
            });
            Logger.Log("Weird structures cleared.");
        }

        bool isVertical(int column)
        {
            return column == 1 || column == 3;
        }

        protected override SelectionHandler<HitObject> CreateSelectionHandler() => new ManiaSelectionHandler();
    }
}
