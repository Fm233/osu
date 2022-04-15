// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.VegetableRuleset.Beatmaps;
using osu.Game.Rulesets.VegetableRuleset.Mods;
using osu.Game.Rulesets.VegetableRuleset.UI;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.VegetableRuleset
{
    public class VegetableRulesetRuleset : Ruleset
    {
        public override string Description => "gather the osu!coins";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableVegetableRulesetRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new VegetableRulesetBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(WorkingBeatmap beatmap) => new VegetableRulesetDifficultyCalculator(this, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.Automation:
                    return new[] { new VegetableRulesetModAutoplay() };

                default:
                    return new Mod[] { null };
            }
        }

        public override string ShortName => "vegetableruleset";

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.W, VegetableRulesetAction.MoveUp),
            new KeyBinding(InputKey.S, VegetableRulesetAction.MoveDown),
        };

        public override Drawable CreateIcon() => new SpriteIcon { Icon = OsuIcon.CrossCircle };
    }
}
