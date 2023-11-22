using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerRotateController : MonoBehaviour
{
    public void Init(PlayerData _playerData)
    {
        playerData = _playerData;
        rotCamSpeed = playerData.rotCamSpeed;
        rotCamXAxisSensitive = playerData.rotCamXAxisSensitive;
        rotCamYAxisSensitive = playerData.rotCamYAxisSensitive;
        minAngleX = playerData.minAngleX;
        maxAngleX = playerData.maxAngleX;
        playerTr = playerData.tr;
        rollReturnAccel = playerData.rollReturnAccel;
    }

    public void PlayerRotate() // Update �����°�
    {
        RotateToMouse(ref rotVec.x, ref rotVec.y);
        playerTr.rotation = Quaternion.Euler(rotVec);


    }

    public void PlayerFixedRotate()
    {
        Quaternion rotation = Quaternion.Euler(rotVec);
        rb.MoveRotation(rotation);
    }

    public void PlayerRotate2() 
    {
        RotateToMouse(ref rotVec.x, ref rotVec.y);
        if (playerData.currentMoveSpeed < 5 || playerData.input.InputZ <= 0)
        {
            rotVec.z = Mathf.MoveTowards(rotVec.z, 0, Time.deltaTime);
        }
        else
        {
            //RotateToKeyboard(ref rotVec.z);
        }

        float targetAngleX = rotVec.x;
        float targetAngleY = rotVec.y;
        float targetAngleZ = rotVec.z;

        currentAngleX = Mathf.SmoothDampAngle(currentAngleX, targetAngleX, ref velocityX, smoothTime);
        currentAngleY = Mathf.SmoothDampAngle(currentAngleY, targetAngleY, ref velocityY, smoothTime);
        currentAngleZ = Mathf.SmoothDampAngle(currentAngleZ, targetAngleZ, ref velocityZ, smoothTime);


            playerData.currentRotZ = currentAngleZ;
            
            playerTr.rotation = Quaternion.Euler(currentAngleX, currentAngleY, currentAngleZ);

    }


    private void RotateToMouse(ref float _eulerAngleX, ref float _eulerAngleY)
    {
        mousePos = playerData.currentMousePos;

        _eulerAngleY += rotCamSpeed * rotCamYAxisSensitive * Time.deltaTime * (mousePos.x / 100);
        _eulerAngleX -= rotCamSpeed * rotCamXAxisSensitive * Time.deltaTime * (mousePos.y / 100);

        //_eulerAngleY += rotCamSpeed * rotCamYAxisSensitive * Time.deltaTime * Input.GetAxis("Mouse X");
        //_eulerAngleX -= rotCamSpeed * rotCamXAxisSensitive * Time.deltaTime * Input.GetAxis("Mouse Y");

        _eulerAngleX = ClampAngle(_eulerAngleX, minAngleX, maxAngleX);
    }

    private float ClampAngle(float _angle, float _min, float _max)
    {
        if (_angle < -360) _angle += 360; // angle�� -360���� ������ 360�� ������. ��������� -380�� -20�� ���� ����
        if (_angle > 360) _angle -= 360;

        return Mathf.Clamp(_angle, _min, _max); // ��Ҹ� ���Ͽ� ���� ���� ���� ��� _angle�� ��ȯ, _min �����ϰ�� _min�� ��ȯ, _max�̻��� ��� _max�� ��ȯ��.
    }

    /// ���콺�� z�� ȸ��
    /// 500  100  40
    //private void RotateToKeyboard(ref float _eulerAngleZ)
    //{
    //    rollAccel = playerData.rollAccel;
    //    rollMaxVelocity = playerData.rollMaxVelocity;
    //    rollMaxAngle = playerData.rollMaxAngle;

    //    mousePos = playerData.mousePos;

    //    if (Mathf.Abs(mousePos.x) > 0f)
    //        rollVelocity += rollAccel * Time.deltaTime * -mousePos.x / 100;
    //    else
    //        rollVelocity = Mathf.MoveTowards(rollVelocity, 0, rollAccel * Time.deltaTime);

    //    rollVelocity = Mathf.Clamp(rollVelocity, -rollMaxVelocity, rollMaxVelocity);

    //    _eulerAngleZ += rollVelocity * Time.deltaTime;
    //    _eulerAngleZ = Mathf.Clamp(_eulerAngleZ, -rollMaxAngle, rollMaxAngle);

    //    if (Mathf.Abs(_eulerAngleZ).Equals(rollMaxAngle))
    //        rollVelocity = 0f;
    //}

    /// <summary>
    /// ����
    /// </summary>



    [SerializeField]
    private float smoothTime = 0.5f;
    float currentAngleX, currentAngleY, currentAngleZ;
    float velocityX = 0f, velocityY = 0f, velocityZ = 0f;

    private float rollReturnAccel = 0f;

    private float eulerAngleX = 0f; // �����ϴµ�?
    private float eulerAngleY = 0f; // �̰͵�

    private float rotCamSpeed = 0f;
    private float rotCamXAxisSensitive = 0f;
    private float rotCamYAxisSensitive = 0f;
    private float minAngleX = 0f;
    private float maxAngleX = 0f;

    private Transform playerTr = null;

    private Vector3 rotVec = Vector3.zero;
    private PlayerData playerData = null;

    private Vector2 mousePos;


    [SerializeField]
    private Rigidbody rb;


}
