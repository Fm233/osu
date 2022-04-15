// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Input.StateChanges;
using osu.Game.Replays;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.VegetableRuleset.Replays
{
    public class VegetableRulesetFramedReplayInputHandler : FramedReplayInputHandler<VegetableRulesetReplayFrame>
    {
        public VegetableRulesetFramedReplayInputHandler(Replay replay)
            : base(replay)
        {
        }

        protected override bool IsImportant(VegetableRulesetReplayFrame frame) => frame.Actions.Any();

        public override void CollectPendingInputs(List<IInput> inputs)
        {
            inputs.Add(new ReplayState<VegetableRulesetAction>
            {
                PressedActions = CurrentFrame?.Actions ?? new List<VegetableRulesetAction>(),
            });
        }
    }
}
