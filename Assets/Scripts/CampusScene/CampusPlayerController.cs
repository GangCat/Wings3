using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampusPlayerController : MonoBehaviour
{
    private void Update()
    {
        CC.SimpleMove(new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0f, Input.GetAxis("Vertical") * moveSpeed));
    }

    [SerializeField]
    private CharacterController CC = null;
    [SerializeField]
    private float moveSpeed = 0f;
    [SerializeField]
    private Camera mainCam = null;
}