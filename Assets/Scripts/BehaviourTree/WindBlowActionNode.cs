using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class WindBlowActionNode : ActionNode
{
    [SerializeField]
    private float smallRadius = 1f;
    [SerializeField]
    private float largeRadius = 2f;
    [SerializeField]
    private float totalDuration = 5f;
    [SerializeField]
    private float colliderInterval = 1f;
    [SerializeField]
    private float cylinderLengthPerSecond = 0.2f;
    [SerializeField]
    private int numVertices = 30;
    [SerializeField]
    private GameObject windCylinderPrefab;

    private float finishTime = 0f;
    private WindBlowPoint[] windBlowPoints = null;


    protected override void OnStart()
    {
        windBlowPoints = context.bossCtrl.CurSpawnPoints[blackboard.curClosedWeakPoint].GetWindBlowHolder().WindBlowPoints;
        foreach (WindBlowPoint wbp in windBlowPoints)
            wbp.StartGenerate(smallRadius, largeRadius, totalDuration, colliderInterval, cylinderLengthPerSecond, numVertices, windCylinderPrefab);

        finishTime = Time.time + totalDuration;
    }

    protected override void OnStop(){
        if (windBlowPoints != null)
        {
            foreach (WindBlowPoint wbp in windBlowPoints)
                wbp.FinishGenerate();
        }
    }

    protected override State OnUpdate()
    {
        if(Time.time >= finishTime)
            return State.Success;

        //for (int i = 0; i < windBlowPoints.Length; ++i)
        //    windBlowPoints[i].updateWindBlow();

        return State.Running;
    }

}
