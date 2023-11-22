using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEditor;

public class CheckWindBlowRangeActionNode : ActionNode
{
    [SerializeField]
    private float range = 0f;
    private BossShieldGeneratorSpawnPoint[] shieldGenerators = null;

    protected override void OnStart() {
        shieldGenerators = context.bossCtrl.CurSpawnPoints;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (blackboard.curPhaseNum != 1)
            return State.Failure;

        for(int i = 0; i < shieldGenerators.Length; ++i)
        {
            if (Physics.OverlapSphere(shieldGenerators[i].GetPos(), range, 1 << LayerMask.NameToLayer("Player")).Length > 0)
            {
                blackboard.curClosedWeakPoint = i;
                return State.Success;
            }
        }

        return State.Failure;
    }

    public override void OnDrawGizmos()
    {
        for(int i = 0; i < shieldGenerators.Length; ++i)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(shieldGenerators[i].GetPos(), range);
        }
    }
}
