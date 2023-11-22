using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerData playerData) : base(playerData)
    {
        tr = playerData.tr;
    }

    public override void Enter()
    {

        angulerVelocity = 0f;
        maxAngle = 45;
        diffAngle = 0f;
        maxVelocityDeg = 10f;

        eulerAngleX = tr.rotation.x;
        eulerAngleY = tr.rotation.y;
    }

    public override void LogicUpdate()
    {
        //accleVelocity = tr.forward * playerData.inputHandler.GetInputZ() * playerData.maxSpeed;
        //accleTime = playerData.accle * Time.deltaTime;
        //rb.velocity = Vector3.MoveTowards(rb.velocity, accleVelocity, accleTime);



        //angulerVelocity += playerData.rotAccle * -playerData.inputHandler.GetInputX() * Time.deltaTime;
        //angulerVelocity = Mathf.Clamp(angulerVelocity, -playerData.maxRotSpeed, playerData.maxRotSpeed);
        //angulerVelocity = Mathf.Clamp(angulerVelocity, -maxAngle, maxAngle);
        //angulerVelocity = playerData.maxRotSpeed * -playerData.inputHandler.GetInputX();
        //rb.angularVelocity = Vector3.MoveTowards(rb.angularVelocity, Vector3.forward * angulerVelocity, playerData.rotAccle * Time.deltaTime);


        //Debug.Log(rb.angularVelocity);
        //Debug.Log(-playerData.inputHandler.GetInputX());

        //// playerData.rotAccle * -playerData.inputHandler.GetInputX() * Time.deltaTime; �� ���ӵ�

        //targetAngle = -playerData.inputHandler.GetInputX() * maxAngle;

        // ������ �־����.
        // ���� ���� ��ġ�ؾ��� ��ġ�� ������ �־����.
        // �ƿ� ��׸��� ó������ ����
        // �׷��� �ִ�, �ּ� ��׸��� �ʿ���.
        // �׸��� ��׸� ��� ���ӵ�, �����ӵ��� �ʿ���.

        //velocityDeg = Mathf.MoveTowards(velocityDeg, playerData.rotMaxVelocityDeg, playerData.rotAccleDeg * Time.deltaTime * -playerData.inputHandler.GetInputX());

        //velocityDeg += playerData.rotAccleDeg * Time.deltaTime * -playerData.inputHandler.GetInputX();



        //if (Mathf.Abs(inputX) > 0f)
        //    velocityDeg += playerData.rotAccleDeg * Time.deltaTime * -inputX;
        //else
        //    velocityDeg = Mathf.MoveTowards(velocityDeg, 0, playerData.rotAccleDeg * Time.deltaTime);

        //velocityDeg = Mathf.Clamp(velocityDeg, -playerData.rotMaxVelocityDeg, playerData.rotMaxVelocityDeg);

        //destAngleDeg += velocityDeg * Time.deltaTime;
        ////tr.rotation = Quaternion.Euler(Vector3.forward * destAngleDeg);
        //destAngleDeg = Mathf.Clamp(destAngleDeg, -maxAngle, maxAngle);

        //if (Mathf.Abs(destAngleDeg).Equals(maxAngle))
        //    velocityDeg = 0f;








        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        Debug.Log("MoveX: " + mouseX);
        Debug.Log("MoveY: " + mouseY);

        UpdateRotate(mouseX, mouseY, destAngleDeg);

        Debug.Log("Move: " + eulerAngleX);
        Debug.Log("Move: " + eulerAngleY);

        //tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.Euler(Vector3.forward * angulerVelocityDeg), 0.5f);
        // a�� ������ -playerData.inputHandler.GetInputX() * maxAngle(90)�� ��������.
        // �׷��� ���� ��ġ�� ���������� �Ÿ��� ���´�.diffAngle
        // �� �� �ִ� �Ÿ� 180�� 1�� ������ ��
        // ���������� �Ÿ��� 180���� ������ ������ ������
        // �� �ð��� ���� ���������� ���� ���������� �ɸ��� �ð� ttlTime�̸�
        // �� �ð����� �����ϴ� �Ÿ��� diffAngle�̹Ƿ�
        // ���ӵ��� ���ϸ� diffAngle / ttlTime�̴�.
        // �� �� �� ���ӵ��� ���غ���
        // �� ���ӵ��� ��������.

    }




    private Vector3 accleVelocity;
    private Rigidbody rb;
    private Transform tr;
    private float inputX;
    private float inputZ;

    private float accleTime;

    private float angulerVelocity;
    private float destAngleDeg;
    private float targetAngle;
    private float maxAngle;
    private float diffAngle;
    private float velocityDeg;
    private float maxVelocityDeg;


    private float moveVelocityM;
    private float moveAccleM;
    private float destMovePos;


    private int tempFactor = 1;
}
