// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.VegetableRuleset.Objects;
using osu.Game.Rulesets.VegetableRuleset.UI;
using osuTK;

namespace osu.Game.Rulesets.VegetableRuleset.Beatmaps
{
    public class VegetableRulesetBeatmapConverter : BeatmapConverter<VegetableRulesetHitObject>
    {
        private readonly float minPosition;
        private readonly float maxPosition;

        public VegetableRulesetBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
            minPosition = beatmap.HitObjects.Min(getUsablePosition);
            maxPosition = beatmap.HitObjects.Max(getUsablePosition);
        }

        public override bool CanConvert() => Beatmap.HitObjects.All(h => h is IHasXPosition && h is IHasYPosition);

        protected override IEnumerable<VegetableRulesetHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken)
        {
            yield return new VegetableRulesetHitObject
            {
                Samples = original.Samples,
                StartTime = original.StartTime,
                Lane = getLane(original)
            };
        }

        private int getLane(HitObject hitObject) => (int)MathHelper.Clamp(
            (getUsablePosition(hitObject) - minPosition) / (maxPosition - minPosition) * VegetableRulesetPlayfield.LANE_COUNT, 0, VegetableRulesetPlayfield.LANE_COUNT - 1);

        private float getUsablePosition(HitObject h) => (h as IHasYPosition)?.Y ?? ((IHasXPosition)h).X;
    }
}
