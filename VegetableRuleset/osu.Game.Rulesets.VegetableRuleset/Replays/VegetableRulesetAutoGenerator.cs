// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.VegetableRuleset.Objects;
using osu.Game.Rulesets.VegetableRuleset.UI;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.VegetableRuleset.Replays
{
    public class VegetableRulesetAutoGenerator : AutoGenerator<VegetableRulesetReplayFrame>
    {
        public new Beatmap<VegetableRulesetHitObject> Beatmap => (Beatmap<VegetableRulesetHitObject>)base.Beatmap;

        public VegetableRulesetAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        protected override void GenerateFrames()
        {
            int currentLane = 0;

            Frames.Add(new VegetableRulesetReplayFrame());

            foreach (VegetableRulesetHitObject hitObject in Beatmap.HitObjects)
            {
                if (currentLane == hitObject.Lane)
                    continue;

                int totalTravel = Math.Abs(hitObject.Lane - currentLane);
                var direction = hitObject.Lane > currentLane ? VegetableRulesetAction.MoveDown : VegetableRulesetAction.MoveUp;

                double time = hitObject.StartTime - 5;

                if (totalTravel == VegetableRulesetPlayfield.LANE_COUNT - 1)
                    addFrame(time, direction == VegetableRulesetAction.MoveDown ? VegetableRulesetAction.MoveUp : VegetableRulesetAction.MoveDown);
                else
                {
                    time -= totalTravel * KEY_UP_DELAY * 2;

                    for (int i = 0; i < totalTravel; i++)
                    {
                        addFrame(time, direction);
                        time += KEY_UP_DELAY * 2;
                    }
                }

                currentLane = hitObject.Lane;
            }
        }

        private void addFrame(double time, VegetableRulesetAction direction)
        {
            Frames.Add(new VegetableRulesetReplayFrame(direction) { Time = time });
            Frames.Add(new VegetableRulesetReplayFrame { Time = time + KEY_UP_DELAY * 0.5 }); //Release the keys as well
        }
    }
}
