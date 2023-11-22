using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhaseConditionActionNode : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        //Debug.Log($"BlackBoard.IsPhaseEnd: {blackboard.isPhaseEnd}");
        return blackboard.isPhaseEnd ? State.Failure : State.Success;
    }
}
