using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CrossLazerActionNode : ActionNode
{
    [SerializeField]
    private GameObject crossLaserPrefab = null;
    [SerializeField]
    private float slowExpandTime = 3f;
    [SerializeField]
    private float autoDestroyTime = 10f;
    [SerializeField]
    private float slowExpandSpeed = 10f;
    [SerializeField]
    private float fastExpandSpeed = 70f;

    private GameObject crossLaserGo = null;

    protected override void OnStart() {
        crossLaserGo = Instantiate(crossLaserPrefab, context.transform.position, Quaternion.identity);
        crossLaserGo.GetComponent<CrossLaserController>().Init(slowExpandTime, autoDestroyTime, slowExpandSpeed, fastExpandSpeed);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return crossLaserGo != null ? State.Running : State.Success;
    }
}
