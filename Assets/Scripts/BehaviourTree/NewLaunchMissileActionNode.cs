using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class NewLaunchMissileActionNode : ActionNode
{
    [SerializeField]
    private GameObject giantHomingMissilePrefab = null;

    private Transform spawnTr = null;
    private GameObject crossLaserGo = null;

    protected override void OnStart() {
        context.anim.OpenBigMissileDoor();
        spawnTr = context.giantHomingMissileSpawnTr;
        crossLaserGo = Instantiate(giantHomingMissilePrefab, spawnTr.position, spawnTr.rotation);
        crossLaserGo.GetComponent<GiantHomingMissileController>().Init(spawnTr.position, spawnTr.rotation, blackboard.isShieldDestroy, context.playerTr);
        context.bossCtrl.AlertGiantMissileLaunch();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return crossLaserGo != null ? State.Running : State.Success;
    }
}
