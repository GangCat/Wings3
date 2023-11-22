using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 위치를 찾는다.
/// 총구를 플레이어 위치까지 돌릴 수 있는 속도가 잇다.
/// 현재 총구의 각도를 플레이어 위치까지 돌린다. 단, lerp를 사용해서 부드럽게 돌려야한다.
/// 플레이어 위치에 도달했을 경우 총알 프리팹을 발사한다.
/// 총알 발사 갯수가 있다.
/// 총알을 최대 발사 갯수까지 생성한다.
/// </summary>
public class GatlingGunController : MonoBehaviour
{
    public Transform playerTr; // 플레이어 위치를 저장하는 Transform
    public Transform gunMuzzleTr; // 총구 위치
    public GameObject bulletPrefab; // 총알 프리팹

    public float rotationSpeed = 5.0f; // 터렛 회전 속도
    public int maxBulletCount = 10; // 최대 총알 발사 갯수
    public float fireRate = 1.0f; // 총알 발사 속도 (초당 발사 횟수)

    private int currentBulletCount; // 현재 총알 발사 갯수
    private float lastFireTime; // 마지막 총알 발사 시간
    private float randomRange = 5f;


    private void Start()
    {
        currentBulletCount = maxBulletCount;
        lastFireTime = 0f;
    }

    private void Update()
    {
        RotateTurretToPlayer();

        if (CanFire())
        {
            FireBullet();
        }
    }

    private void RotateTurretToPlayer()
    {
        if (playerTr != null)
        {
            Vector3 playerDirection = new Vector3(playerTr.position.x-transform.position.x,0f,playerTr.position.z-transform.position.z);
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);

            // 부드럽게 회전하기 위해 Lerp 사용
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private bool CanFire()
    {
        return currentBulletCount > 0 && Time.time - lastFireTime >= 1 / fireRate;
    }

    private void FireBullet()
    {
        lastFireTime = Time.time;
        currentBulletCount--;

        Vector3 tmp = gunMuzzleTr.up;

        //float angle = Random.Range(0, 360);
        //float radians = angle * Mathf.Deg2Rad;
        //Vector3 spawnPosition = playerTr.position + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)) * randomRange;

        Quaternion rot = Quaternion.AngleAxis(Random.Range(0, 360), gunMuzzleTr.forward);
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
        Vector3 targetPos = rotationMatrix.MultiplyPoint3x4(tmp) + playerTr.position;

        GameObject bullet = Instantiate(bulletPrefab, gunMuzzleTr.position, gunMuzzleTr.rotation);
    }
}
