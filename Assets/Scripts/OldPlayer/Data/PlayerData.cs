using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public Transform tr;
    public PlayerInputHandler input;

    [Header("Status Data")]
    public int maxHealth = 100;
    public int stamina = 3;

    [Header("Move Data")]
    public float moveBackVelocityLimit = 0f;
    public float moveForwardVelocityLimit = 0f;
    public float moveAccel = 0f;
    public float moveDashSpeed = 0f;
    public float moveDashAccel = 0f;
    public float moveStopAccel = 50f;
    public float gravityAccel = 50f;
    public float gravitySpeed = 100f;

    public bool isDash = false;
    public bool isCrash = false;
    public bool isAction = false;


    [Header("Dodge Data")]
    public float dodgeSpeed = 60f;
    public float dodgeDuration = 0.3f;

    [Header("Cam Data")]
    public float rotCamSpeed = 100f;
    public float rotCamXAxisSensitive = 2f;
    public float rotCamYAxisSensitive = 1.5f;
    public float minAngleX = 0f;
    public float maxAngleX = 0f;
    public bool isFreeLock = false;

    [Header("Virtual Mouse")]
    public float returnMouseSpeed = 100f;
    public float mouseSensitive = 3f;
    public float maxMouseSpeed = 3f;
    public float maxMouseRadius = 100f;
    public float returnMouseRadius = 90f;
    public Vector2 currentMousePos = Vector2.zero;


    [Header("Roll Data")]
    public float rollAccel = 0f;
    public float rollMaxVelocity = 0f;
    public float rollMaxAngle = 0f;
    public float rollReturnAccel = 0f;

    public float currentRotZ = 0f;
    public float currentMoveSpeed = 0f;
}
