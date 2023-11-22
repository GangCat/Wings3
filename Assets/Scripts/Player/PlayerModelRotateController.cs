using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotateController : MonoBehaviour
{
    public void Init(PlayerData _data)
    {
        tr = GetComponent<Transform>();
        playerData = _data;

    }

    public void PlayerModelRotate()
    {
        if (playerData.currentMoveSpeed < 5 || playerData.input.InputZ <= 0)
        {
            rotZ = Mathf.MoveTowards(rotZ, 0, playerData.rollReturnAccel * Time.deltaTime);
        }
        else
        {
            RotateToMouse(ref rotZ);
            
        }
        //rotation.y = rotation.z;
        playerData.currentRotZ = rotZ;

      tr.localRotation = Quaternion.Euler(Vector3.forward * rotZ);

    }

    private void RotateToKeyboardZ(ref float _eulerAngleZ)
    {
        rollAccel = playerData.rollAccel;
        rollMaxVelocity = playerData.rollMaxVelocity;
        rollMaxAngle = playerData.rollMaxAngle;

        if (Mathf.Abs(playerData.input.InputX) > 0f)
            rollVelocity += rollAccel * Time.deltaTime * -playerData.input.InputX;
        else
            rollVelocity = Mathf.MoveTowards(rollVelocity, 0, rollAccel * Time.deltaTime);

        rollVelocity = Mathf.Clamp(rollVelocity, -rollMaxVelocity, rollMaxVelocity);

        currentRotZ += rollVelocity * Time.deltaTime;
        currentRotZ = Mathf.Clamp(currentRotZ, -rollMaxAngle, rollMaxAngle);

        if (Mathf.Abs(currentRotZ).Equals(rollMaxAngle))
        {
            rollVelocity = 0f;
        }
    }

    private void RotateToMouse(ref float _eulerAngleZ)
    {
        rollAccel = playerData.rollAccel;
        rollMaxVelocity = playerData.rollMaxVelocity;
        rollMaxAngle = playerData.rollMaxAngle;
        mousePos = playerData.currentMousePos;
        float mouseRatio = (-mousePos.x / 100);
        

        if (Mathf.Abs(mousePos.x) > 0f)
            rollVelocity += rollAccel * Time.deltaTime * mouseRatio;
        else
        {
          rollVelocity = Mathf.MoveTowards(rollVelocity, 0, rollAccel * Time.deltaTime);
          _eulerAngleZ = Mathf.MoveTowards(_eulerAngleZ, 0, playerData.rollReturnAccel * Time.deltaTime);
        }

        rollVelocity = Mathf.Clamp(rollVelocity, -rollMaxVelocity, rollMaxVelocity);
        lerpMouseRatio = Mathf.Lerp(lerpMouseRatio, mouseRatio, 2f * Time.deltaTime);
        _eulerAngleZ += rollVelocity * Time.deltaTime;
        _eulerAngleZ = Mathf.Clamp(_eulerAngleZ, -rollMaxAngle*Mathf.Abs(lerpMouseRatio), rollMaxAngle * Mathf.Abs(lerpMouseRatio));

        if (Mathf.Abs(_eulerAngleZ).Equals(rollMaxAngle))
            rollVelocity = 0f;
    }


    //private void RotateToSpeedX()
    //{
    //    float eulerAngleX;
    //    if (Mathf.Abs(playerData.input.InputX) > 0f)
    //        rollVelocity += rollAccel * Time.deltaTime * -playerData.input.InputX;
    //    else
    //        rollVelocity = Mathf.MoveTowards(rollVelocity, 0, rollAccel * Time.deltaTime);

    //    rollVelocity = Mathf.Clamp(rollVelocity, -rollMaxVelocity, rollMaxVelocity);

    //    currentRotX += rollVelocity * Time.deltaTime;
    //    eulerAngleX = rollVelocity * Time.deltaTime;
    //    currentRotX = Mathf.Clamp(currentRotZ, -rollMaxAngle, currentMaxAngle);

    //    if (Mathf.Abs(currentRotZ).Equals(currentMaxAngle))
    //    {
    //        rollVelocity = 0f;
    //        eulerAngleX = 0f;
    //    }
    //    transform.Rotate(tr.right * eulerAngleX, Space.World);
    //}

    //private IEnumerator Test2()
    //{
    //    while (true)
    //    {
    //        currentMoveVelocityRatio = playerData.currentMoveSpeed/playerData.moveForwardVelocityLimit;
    //        InputZRot = 45 * currentMoveVelocityRatio;
    //        if (Mathf.Abs(playerData.currentRotZ) <= 30)
    //        {
    //            mousePosRatio = 1;
    //        }
    //        else if(playerData.currentRotZ < 0)
    //        {
    //            if (playerData.currentMousePos.x < 0)
    //                mousePosRatio = 1 + -(playerData.currentMousePos.x / 100);
    //        }
    //        else{
    //            if (playerData.currentMousePos.x > 0)
    //                mousePosRatio = 1+(playerData.currentMousePos.x / 100);
    //        }
    //        resultRot = InputZRot * (mousePosRatio);
    //        currentMaxAngle = Mathf.Lerp(currentMaxAngle, resultRot, smoothness);
    //        currentMaxAngle = Mathf.Clamp(currentMaxAngle, -90, 90);

    //        rotation.x = currentMaxAngle;

    //        yield return new WaitForFixedUpdate();
    //    }
    //}

    private Vector3 rotation;
    private float rotZ;

    private float currentRotZ;
    private float currentRotX;
    private float currentMaxAngle;
    private float resultRot;
    private float smoothness = 0.1f;

    private float InputZRot;
    private float mousePosRatio;

    private float currentMoveVelocityRatio;


    private float rollVelocity = 0f;
    private float rollAccel = 0f;
    private float rollMaxVelocity = 0f;
    private float rollMaxAngle = 0f;
    private float lerpMouseRatio = 0;

    private Transform tr = null;
    private PlayerData playerData = null;

    private Vector2 mousePos = Vector2.zero;
}
