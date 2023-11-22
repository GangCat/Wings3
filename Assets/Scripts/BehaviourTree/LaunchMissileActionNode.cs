using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

/// <summary> 
/// ȣ�� �� ������ �̻��� ���� 
/// �̻��� ������, �÷��̾� ��ġ, ���� ��ġ���� ���� �޾ƾ� �� 
/// �̻����� ���� ������ runninng ��ȯ 
/// �̻����� ���� �ӵ��� �̵� 
/// �ð��� �������� ���ӵ��� ���� 
/// �÷��̾� �������� ȸ�� 
/// ȸ�� ���� �� ���� �ӵ� ���� ���ӵ��� ���� �پ��
/// ȸ�� ���� �ƴϰ� �ִ� �ӵ����� ���ӵ��� ���� �þ
/// ������ �Ӹ� �������� ���� ���⺤�� ���� �� ���ư� 
/// �ʿ��� ���� : �̻��� ������, �÷��̾� ��ġ, ������ ���� ��ġ, ���� �ӵ�, �ְ� �ӵ�, ���ӵ�, ���� ���� ���� 
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
