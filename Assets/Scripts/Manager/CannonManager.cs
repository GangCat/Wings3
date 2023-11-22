using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 위치를 찾는다
/// 플레이어위치를 중심으로 동그란 공격 범위를 만든다
/// 포탄 갯수를 정한다.
/// 포탄 갯수 만큼 동그란 공격 범위 안에서 랜덤으로 포탄 공격 pos를 지정한다
/// 포탄 pos 위에서 프리팹 포탄을 생성한다.
/// </summary>
public class CannonManager : MonoBehaviour
{
    public Transform player = null;
    public float attackRange = 5.0f; // 공격 범위 반지름
    public int bulletCount = 5; // 생성할 포탄의 갯수
    public float attackHeight = 2.0f; // 포탄의 높이
    public GameObject bulletPrefab; // 포탄 프리팹
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
            // 땅의 위치를 기준으로 동그란 공격 범위 생성
            Vector3 attackPosition = (Vector3)player.position/*+ player.forward*10*/;
            for (int i = 0; i < bulletCount; i++)
            {
                randomRange = Random.Range(0, attackRange);
                // 랜덤한 각도를 사용하여 포탄을 원 주위에 생성
                float angle = Random.Range(0, 360);
                float radians = angle * Mathf.Deg2Rad;
                //Vector3 spawnPosition = attackPosition + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)) * randomRange;
                Vector3 spawnPosition = attackPosition + new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * randomRange;

                // 포탄 생성 (높이 추가)
                //Vector3 spawnPositionWithHeight = new Vector3(spawnPosition.x, attackHeight, spawnPosition.z + spawnPosition.y);



                Vector2 rnd = Random.insideUnitCircle * 20f;

                Vector3 spawnPositionWithHeight = attackPosition+new Vector3(rnd.x, attackHeight, rnd.y);


                GameObject bullet = Instantiate(bulletPrefab, spawnPositionWithHeight, Quaternion.identity);

                // 원하는 추가 설정을 수행
                // 예를 들어, 포탄에 힘을 가하거나 다른 동작을 추가할 수 있습니다.
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
}
