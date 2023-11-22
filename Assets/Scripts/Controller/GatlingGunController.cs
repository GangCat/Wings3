using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� ��ġ�� ã�´�.
/// �ѱ��� �÷��̾� ��ġ���� ���� �� �ִ� �ӵ��� �մ�.
/// ���� �ѱ��� ������ �÷��̾� ��ġ���� ������. ��, lerp�� ����ؼ� �ε巴�� �������Ѵ�.
/// �÷��̾� ��ġ�� �������� ��� �Ѿ� �������� �߻��Ѵ�.
/// �Ѿ� �߻� ������ �ִ�.
/// �Ѿ��� �ִ� �߻� �������� �����Ѵ�.
/// </summary>
public class GatlingGunController : MonoBehaviour
{
    public Transform playerTr; // �÷��̾� ��ġ�� �����ϴ� Transform
    public Transform gunMuzzleTr; // �ѱ� ��ġ
    public GameObject bulletPrefab; // �Ѿ� ������

    public float rotationSpeed = 5.0f; // �ͷ� ȸ�� �ӵ�
    public int maxBulletCount = 10; // �ִ� �Ѿ� �߻� ����
    public float fireRate = 1.0f; // �Ѿ� �߻� �ӵ� (�ʴ� �߻� Ƚ��)

    private int currentBulletCount; // ���� �Ѿ� �߻� ����
    private float lastFireTime; // ������ �Ѿ� �߻� �ð�
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

            // �ε巴�� ȸ���ϱ� ���� Lerp ���
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
