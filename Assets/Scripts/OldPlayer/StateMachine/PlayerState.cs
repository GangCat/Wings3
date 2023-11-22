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
        if (_angle < -360) _angle += 360; // angle이 -360보다 작으면 360을 더해줌. 결과적으로 -380이 -20과 같게 계산됨
        if (_angle > 360) _angle -= 360;

        return Mathf.Clamp(_angle, _min, _max); // 대소를 비교하여 범위 안의 값일 경우 _angle을 반환, _min 이하일경우 _min을 반환, _max이상일 경우 _max를 반환함.
    }

    protected float mouseX;
    protected float mouseY;

    protected float MinAngleX = -80f; // 카메라 x축 회전 범위, 즉 위로 올릴 수 있는 최대 범위
    protected float MaxAngleX = 80f; // 아래로 내릴 수 있는 최대 범위
    protected float eulerAngleX = 0f;
    protected float eulerAngleY = 0f;
    protected float rotCamXAxisSpeed = 5f; // 카메라 x축 회전 속도
    protected float rotCamYAxisSpeed = 3f; // 카메라 y축 회전 속도

}
