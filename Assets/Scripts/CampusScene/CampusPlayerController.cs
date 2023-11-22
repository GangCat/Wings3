using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampusPlayerController : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    private void Update()
    {
        CC.SimpleMove(transform.forward * Input.GetAxis("Vertical") * moveSpeed);
        cameraYaw += Input.GetAxis("Mouse X") * freeLookSensitive * Time.deltaTime;
        cameraPitch -= Input.GetAxis("Mouse Y") * freeLookSensitive * Time.deltaTime;


        // 카메라의 회전을 적용
        quaternion = Quaternion.Euler(cameraPitch, cameraYaw, 0);
        transform.rotation = quaternion;
    }

    [SerializeField]
    private CharacterController CC = null;
    [SerializeField]
    private float moveSpeed = 0f;
    [SerializeField]
    private Camera mainCam = null;
    [SerializeField]
    private float freeLookSensitive;

    private float cameraYaw;
    private float cameraPitch;
    private Quaternion quaternion;
}