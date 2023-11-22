using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckIsLastPhase : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (blackboard.curPhaseNum > 2)
            return State.Success;

        return State.Failure;
    }
}
