using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ShakeBodySlowRotationActionNode : ActionNode
{
    [SerializeField]
    private float maxRotSpeed = 20f;
    [SerializeField]
    private float rotationLimitDegree = 30f;
    [SerializeField]
    private float rotationAccel = 20f;

    private float curRotation = 0f;
    private float curRotationSpeed = 0f;
    private float curRotationDegree = 0f;
    private Transform bossTr = null;
    private Rigidbody bossRb = null;

    protected override void OnStart() {
        bossTr = context.bossCtrl.RotateTr;
        bossRb = context.physics;
        //bossRb.maxAngularVelocity = maxRotSpeed * Mathf.Rad2Deg;
        //움직이는 사운드 시작(루프)
        curRotationDegree = bossTr.rotation.eulerAngles.y;
    }

    protected override void OnStop() {
        //움직이는 사운드 끄기(루프)
    }

    protected override State OnUpdate() {
        curRotationSpeed += curRotationSpeed < maxRotSpeed ? rotationAccel * Time.deltaTime : 0;
        bossTr.rotation = bossTr.rotation * Quaternion.Euler(Vector3.up * curRotationSpeed * Mathf.Deg2Rad);
        //bossRb.angularVelocity = Vector3.up * (curRotationSpeed * Mathf.Deg2Rad);
        //Debug.Log(curRotationSpeed);
        
        //curRotationDegree += curRotationSpeed * Time.deltaTime;
        //bossTr.rotation = Quaternion.Euler(Vector3.up * curRotationDegree);

        curRotation = bossTr.rotation.eulerAngles.y;
        curRotation -= 180;

        if (curRotation > 180)
            curRotation -= 360;


        return curRotation < rotationLimitDegree ? State.Running : State.Success;
    }
}
