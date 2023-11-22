using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class FootWindPushActionNode : ActionNode
{
    [SerializeField]
    private float increasingScaleSpeed= 5f;
    [SerializeField]
    private float duration =5f;

    private float curDuration = 0f;
    private GameObject footWindGo;
    private Transform[] arrSpawnPointTr = null;
    protected override void OnStart() {
        Debug.Log("start");
        arrSpawnPointTr = context.footWindTr;
        for (int i = 0; i < arrSpawnPointTr.Length; ++i)
        {
            footWindGo = Instantiate(context.footWindGo, arrSpawnPointTr[i]);
        }
        curDuration = duration;
    }

    protected override void OnStop() {
        Destroy(footWindGo);
    }

    protected override State OnUpdate() {
        
        footWindGo.transform.localScale += new Vector3(increasingScaleSpeed * Time.deltaTime, 0, increasingScaleSpeed * Time.deltaTime);
        curDuration -= Time.deltaTime;

        if (curDuration > 0)
            return State.Running;

        return State.Success;
    }
}
