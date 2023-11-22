using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GatilingHeadController : MonoBehaviour
{
    public Transform player; // �÷��̾� ��ġ�� �����ϴ� Transform
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
            // �ε巴�� ȸ���ϱ� ���� Lerp ���
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
