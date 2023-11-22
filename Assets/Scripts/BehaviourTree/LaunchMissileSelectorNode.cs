using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class LaunchMissileSelectorNode : CompositeNode
{
    [SerializeField, Range(0, 1)]
    private float groupMissileRatio = 0f;
    [SerializeField, Range(0, 1)]
    private float curGroupMissileRatio = 0f;

    private int rndNum = 0;
    protected override void OnStart() {
        rndNum = Random.Range(0, 100);
    }

    protected override void OnStop() {
        if (rndNum < curGroupMissileRatio * 100)
            curGroupMissileRatio -= 0.1f;
        else
            curGroupMissileRatio = groupMissileRatio;
    }

    protected override State OnUpdate() {

        if (rndNum < curGroupMissileRatio * 100)
            return children[0].Update();
        else
            return children[1].Update();
    }
}
