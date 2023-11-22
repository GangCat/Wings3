using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public void Init(Transform _playerTr,PlayerData _playerData)
    {
        playerTr = _playerTr;
        playerData = _playerData;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !backMirror) 
        {
            cameraYaw = currentRotation.y;
            cameraPitch = currentRotation.x;
            playerData.isFreeLock = true;
        }
        else if (Input.GetMouseButtonUp(1) ) 
        {
            playerData.isFreeLock = false;
        }

        //if (Input.GetKeyDown(KeyCode.Tab) && !playerData.isFreeLock) 
        //{
        //    backMirror = true;
        //}
        //else if (Input.GetKeyUp(KeyCode.Tab)) 
        //{
        //    backMirror = false;
        //}


    }

    private void FixedUpdate()
    {
        if (!playerData.isFreeLock)
        {
            FollowPlayerRot();
            FollowPlayerPos();
        }
        else
        {
            FreeLock();
        }

        transform.rotation = quaternion;
        transform.position = cameraPos;
    }

    private void LateUpdate()
    {
        //transform.position = Pos;
        //desiredRotation = Quaternion.LookRotation(playerTr.forward);
        //quaternion = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeed);
        //Pos = playerTr.position + -playerTr.forward * offset + transform.up * 3f;
        //transform.rotation = quaternion;
        //transform.position = Pos;
    }

    private void FollowPlayerPos()
    {
        if(backMirror)
            calcPos = playerTr.position + playerTr.forward * offset + transform.up * upOffset;
        else
        calcPos = playerTr.position - playerTr.forward * offset + transform.up * upOffset;
        float smoothedPosX = Mathf.Lerp(transform.position.x, calcPos.x, posSmoothX * Time.deltaTime);
        float smoothedPosY = Mathf.Lerp(transform.position.y, calcPos.y, posSmoothY * Time.deltaTime);
        float smoothedPosZ = Mathf.Lerp(transform.position.z, calcPos.z, posSmoothZ * Time.deltaTime);
        cameraPos = calcPos;
        //cameraPos = new Vector3(smoothedPosX, smoothedPosY, smoothedPosZ);
    }

    private void FollowPlayerPos(Vector3 _playerPos)
    {
        float smoothedPosX = Mathf.Lerp(transform.position.x, _playerPos.x, posSmoothX * Time.deltaTime);
        float smoothedPosY = Mathf.Lerp(transform.position.y, _playerPos.y, posSmoothY * Time.deltaTime);
        float smoothedPosZ = Mathf.Lerp(transform.position.z, _playerPos.z, posSmoothZ * Time.deltaTime);
        cameraPos = new Vector3(smoothedPosX, smoothedPosY, smoothedPosZ);
    }

    private void FollowPlayerRot()
    {
        Vector2 mousePos = playerData.currentMousePos;
        currentRotation = transform.rotation.eulerAngles;
        if (playerData.currentMoveSpeed >= 1)
            cameraZ = new Vector3(0f, 0f, -rotCamreaZ * (mousePos.x / 100));
        else
            cameraZ = Vector3.zero;
        if (backMirror)
            desiredRotation = Quaternion.LookRotation(-playerTr.forward).eulerAngles+cameraZ;
        else
        desiredRotation = Quaternion.LookRotation(playerTr.forward).eulerAngles+cameraZ;

        float xRot = Mathf.SmoothDampAngle(currentRotation.x, desiredRotation.x, ref rotVectorVelocity.x, rotSmoothX * Time.deltaTime);
        float yRot = Mathf.SmoothDampAngle(currentRotation.y, desiredRotation.y, ref rotVectorVelocity.y, rotSmoothY * Time.deltaTime);
        float zRot = Mathf.SmoothDampAngle(currentRotation.z, desiredRotation.z, ref rotVectorVelocity.z, rotSmoothZ * Time.deltaTime);

        quaternion = Quaternion.Euler(new Vector3(xRot, yRot, zRot));

    }

    private bool CameraRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerTr.position - transform.position, out hit))
        {
            Debug.DrawRay(transform.position, playerTr.position - transform.position, Color.red);
            if (hit.collider.gameObject != playerTr.gameObject)
            {
                transform.position = hit.point;
                return true;
            }
        }
        return false;
    }

    private void FreeLock()
    {
        cameraYaw += Input.GetAxis("Mouse X") * freeLockSensitive * Time.deltaTime;
        cameraPitch -= Input.GetAxis("Mouse Y") * freeLockSensitive * Time.deltaTime;

        // 카메라 Pitch 각도를 제한
        //cameraPitch = Mathf.Clamp(cameraPitch, pitchMinMax.x, pitchMinMax.y);

        // 카메라의 회전을 적용
        quaternion = Quaternion.Euler(cameraPitch, cameraYaw, 0);
        Vector3 calcPlayerPos = playerTr.position - (quaternion * Vector3.forward) * (offset) + transform.up * upOffset;
        cameraPos = calcPlayerPos;
       
    }


    private float currentAngle = 0.0f;
    private Vector3 cameraOffset;
    private float cameraYaw; 
    private float cameraPitch;

    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float posSmoothX = 10f;
    public float posSmoothY = 10f;
    public float posSmoothZ = 10f;

    public float rotSmoothX = 0.1f;
    public float rotSmoothY = 0.05f;
    public float rotSmoothZ = 0.1f;

    public float offset = 0f;
    public float upOffset = 3f;

    public float freeLockSensitive = 300f;

    private Vector3 currentRotation = Vector3.zero;
    private Vector3 desiredRotation = Vector3.zero;
    private Vector3 calcPos = Vector3.zero;
    private Vector3 cameraPos = Vector3.zero;
    private Vector3 cameraZ = Vector3.zero;
    private Vector3 rotVectorVelocity;
    private Quaternion quaternion = Quaternion.identity;
    private Transform playerTr = null;
    private PlayerData playerData = null;
    private bool backMirror = false;

    [SerializeField]
    private float rotCamreaZ = 15f;
}
