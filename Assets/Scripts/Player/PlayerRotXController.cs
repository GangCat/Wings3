using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotXController : MonoBehaviour
{
    [SerializeField]
    private Transform tr;
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private float smooth;

    private float currentXRotation = 0f;
    private float targetRotateX;
    private float calcRotateX;
    private void Update()
    {
        targetRotateX = tr.rotation.eulerAngles.x;
        if (targetRotateX >= 250 && targetRotateX<310) // 250
        {
            Debug.Log(targetRotateX);
            calcRotateX = 360-targetRotateX;
        } else if(targetRotateX >= 45 && targetRotateX <= 100) // 5
        {
            calcRotateX = targetRotateX * (playerData.currentMoveSpeed/playerData.moveForwardVelocityLimit);
        }
        else
        {
            calcRotateX = 45 * (playerData.currentMoveSpeed / playerData.moveForwardVelocityLimit);
        }

        currentXRotation = Mathf.Lerp(currentXRotation, calcRotateX, smooth*Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);

    }
}
