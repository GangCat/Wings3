using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GatilingHeadController : MonoBehaviour
{
    public Transform player; // 플레이어 위치를 저장하는 Transform
    public float rotationSpeed = 10f;
    private float diffY;
    private float cetha;

    private void Start()
    {
    }

    private void Update()
    {
        RotateTurretHeadToPlayer();
    }

    private void RotateTurretHeadToPlayer()
    {
        if (player != null)
        {
            diffY = player.position.y - transform.position.y;
            cetha = Mathf.Asin(diffY/Vector3.Distance(player.position, transform.position))*Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(Vector3.left*cetha);

            //Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            // 부드럽게 회전하기 위해 Lerp 사용
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
