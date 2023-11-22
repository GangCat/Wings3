using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerData playerData;

    public PlayerState(PlayerData playerData)
    {
        this.playerData = playerData;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        Debug.Log("IdleX: " + mouseX);
        Debug.Log("IdleY: " + mouseY);

        UpdateRotate(mouseX, mouseY);
    }

    public virtual void PhysicsUpdate()
    {

    }


    protected void UpdateRotate(float _mouseX, float _mouseY, float _angleZ = 0f)
    {
        eulerAngleY += _mouseX * rotCamYAxisSpeed;
        eulerAngleX -= _mouseY * rotCamXAxisSpeed;
        eulerAngleX = ClampAngle(eulerAngleX, MinAngleX, MaxAngleX);
        //playerData.tr.rotation = Quaternion.Lerp(playerData.tr.rotation, Quaternion.Euler(eulerAngleX, eulerAngleY, 0), playerData.rotAccle * Time.deltaTime);

        playerData.tr.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0f);

        Debug.Log(eulerAngleX);
        Debug.Log(eulerAngleY);
    }

    protected float ClampAngle(float _angle, float _min, float _max)
    {
        if (_angle < -360) _angle += 360; // angle�� -360���� ������ 360�� ������. ��������� -380�� -20�� ���� ����
        if (_angle > 360) _angle -= 360;

        return Mathf.Clamp(_angle, _min, _max); // ��Ҹ� ���Ͽ� ���� ���� ���� ��� _angle�� ��ȯ, _min �����ϰ�� _min�� ��ȯ, _max�̻��� ��� _max�� ��ȯ��.
    }

    protected float mouseX;
    protected float mouseY;

    protected float MinAngleX = -80f; // ī�޶� x�� ȸ�� ����, �� ���� �ø� �� �ִ� �ִ� ����
    protected float MaxAngleX = 80f; // �Ʒ��� ���� �� �ִ� �ִ� ����
    protected float eulerAngleX = 0f;
    protected float eulerAngleY = 0f;
    protected float rotCamXAxisSpeed = 5f; // ī�޶� x�� ȸ�� �ӵ�
    protected float rotCamYAxisSpeed = 3f; // ī�޶� y�� ȸ�� �ӵ�

}
