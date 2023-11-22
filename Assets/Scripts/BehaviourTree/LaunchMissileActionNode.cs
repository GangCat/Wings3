using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

/// <summary> 
/// 호출 될 때마다 미사일 생성 
/// 미사일 프리팹, 플레이어 위치, 생성 위치등을 전달 받아야 함 
/// 미사일이 아직 있으면 runninng 반환 
/// 미사일은 최저 속도로 이동 
/// 시간이 지날수록 가속도가 붙음 
/// 플레이어 방향으로 회전 
/// 회전 중일 시 최저 속도 까지 가속도가 점점 줄어듬
/// 회전 중이 아니고 최대 속도까지 가속도가 점점 늘어남
/// 본인의 머리 방향으로 직선 방향벡터 생성 후 날아감 
/// 필요한 변수 : 미사일 프리팹, 플레이어 위치, 본인의 생성 위치, 최저 속도, 최고 속도, 가속도, 직진 방향 벡터 
/// </summary>
public class LaunchMissileActionNode : ActionNode
{
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float autoDestroyTime;
    [SerializeField]
    private GameObject giantHomingMissilePrefab;

    private float currentSpeed;
    private float rotateAngle = 0f;
    private GameObject missile;

    protected override void OnStart()
    {
        missile = Instantiate(giantHomingMissilePrefab, context.giantHomingMissileSpawnTr.position, Quaternion.identity);
        Destroy(missile, autoDestroyTime);
        currentSpeed = minSpeed;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {

        if (missile)
        {
            Vector3 curDirVec = missile.transform.forward;
            Vector3 direction = context.playerTr.position - missile.transform.position;
            //rotateAngle = Vector3.Dot(curDirVec, Quaternion.LookRotation(direction).eulerAngles);
            rotateAngle = Quaternion.Angle(Quaternion.LookRotation(curDirVec), Quaternion.LookRotation(direction));
            Debug.Log(currentSpeed + "//" + Mathf.Abs(rotateAngle));


            // Rotate towards the player



            // Increase acceleration and speed over time
            if (Mathf.Abs(rotateAngle) <=20)
            {
                missile.transform.rotation = Quaternion.Slerp(missile.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += acceleration *Time.deltaTime;
                }
            }
            else
            {
                missile.transform.rotation = Quaternion.Slerp(missile.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2);
                if (currentSpeed > minSpeed)
                {
                    currentSpeed -= acceleration * Time.deltaTime;
                }
            }
            // Move missile in the direction of rotation
            missile.transform.position += curDirVec * currentSpeed * Time.deltaTime;
            return State.Running;
        }
        else
        {
            return State.Success;
        }        
    }    
}
