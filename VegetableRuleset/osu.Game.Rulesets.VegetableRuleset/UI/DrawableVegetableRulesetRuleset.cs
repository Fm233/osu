// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.VegetableRuleset.Objects;
using osu.Game.Rulesets.VegetableRuleset.Objects.Drawables;
using osu.Game.Rulesets.VegetableRuleset.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.VegetableRuleset.UI
{
    [Cached]
    public class DrawableVegetableRulesetRuleset : DrawableScrollingRuleset<VegetableRulesetHitObject>
    {
        public DrawableVegetableRulesetRuleset(VegetableRulesetRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
            Direction.Value = ScrollingDirection.Left;
            TimeRange.Value = 6000;
        }

        protected override Playfield CreatePlayfield() => new VegetableRulesetPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new VegetableRulesetFramedReplayInputHandler(replay);

        public override DrawableHitObject<VegetableRulesetHitObject> CreateDrawableRepresentation(VegetableRulesetHitObject h) => new DrawableVegetableRulesetHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new VegetableRulesetInputManager(Ruleset?.RulesetInfo);
    }
}
