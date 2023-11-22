using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� ��ġ�� ã�´�
/// �÷��̾���ġ�� �߽����� ���׶� ���� ������ �����
/// ��ź ������ ���Ѵ�.
/// ��ź ���� ��ŭ ���׶� ���� ���� �ȿ��� �������� ��ź ���� pos�� �����Ѵ�
/// ��ź pos ������ ������ ��ź�� �����Ѵ�.
/// </summary>
public class CannonManager : MonoBehaviour
{
    public Transform player = null;
    public float attackRange = 5.0f; // ���� ���� ������
    public int bulletCount = 5; // ������ ��ź�� ����
    public float attackHeight = 2.0f; // ��ź�� ����
    public GameObject bulletPrefab; // ��ź ������
    private float randomRange;

    private void Start()
    {
        StartCoroutine(coolDownCoroutine());
    }
    private void Update()
    {
        
    }

    IEnumerator coolDownCoroutine()
    {
        while (true)
        {
            // ���� ��ġ�� �������� ���׶� ���� ���� ����
            Vector3 attackPosition = (Vector3)player.position/*+ player.forward*10*/;
            for (int i = 0; i < bulletCount; i++)
            {
                randomRange = Random.Range(0, attackRange);
                // ������ ������ ����Ͽ� ��ź�� �� ������ ����
                float angle = Random.Range(0, 360);
                float radians = angle * Mathf.Deg2Rad;
                //Vector3 spawnPosition = attackPosition + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)) * randomRange;
                Vector3 spawnPosition = attackPosition + new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * randomRange;

                // ��ź ���� (���� �߰�)
                //Vector3 spawnPositionWithHeight = new Vector3(spawnPosition.x, attackHeight, spawnPosition.z + spawnPosition.y);



                Vector2 rnd = Random.insideUnitCircle * 20f;

                Vector3 spawnPositionWithHeight = attackPosition+new Vector3(rnd.x, attackHeight, rnd.y);


                GameObject bullet = Instantiate(bulletPrefab, spawnPositionWithHeight, Quaternion.identity);

                // ���ϴ� �߰� ������ ����
                // ���� ���, ��ź�� ���� ���ϰų� �ٸ� ������ �߰��� �� �ֽ��ϴ�.
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
}
