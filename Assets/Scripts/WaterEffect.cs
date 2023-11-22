using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject waterEffect;
    [SerializeField]
    private float bossBodyEffectSize = 50f;
    [SerializeField]
    private float giantEffectSize = 10f;
    [SerializeField]
    private float groupEffectSize = 5f;
    [SerializeField]
    private float BallEffectSize = 5f;
    [SerializeField]
    private float RainBallEffectSize = 50f;
    [SerializeField]
    private float gatlinBulletSize = 3f;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 bottomPoint = other.bounds.center - new Vector3(0, other.bounds.extents.y, 0);
        GameObject effect = Instantiate(waterEffect, bottomPoint, Quaternion.identity);
        if (other.CompareTag("BossBody")) 
        {
            Vector3 newSize = effect.transform.localScale*bossBodyEffectSize;
            effect.transform.localScale = newSize;
        } else if (other.CompareTag("GiantHomingMissile"))
        {
            Vector3 newSize = effect.transform.localScale * giantEffectSize;
            Debug.Log("New Size: " + newSize); // 확인을 위한 로그

            effect.transform.localScale = newSize;
        } else if (other.CompareTag("GroupHomingMissile"))
        {
            Vector3 newSize = effect.transform.localScale * groupEffectSize;
            effect.transform.localScale = newSize;
        } else if (other.CompareTag("CannonBall"))
        {
            Vector3 newSize = effect.transform.localScale * BallEffectSize;
            effect.transform.localScale = newSize;
        }
        else if (other.CompareTag("CannonRainBall"))
        {
            Vector3 newSize = effect.transform.localScale * RainBallEffectSize;
            effect.transform.localScale = newSize;
        }
        else if (other.CompareTag("GatlingGunBullet"))
        {
            Vector3 newSize = effect.transform.localScale * gatlinBulletSize;
            effect.transform.localScale = newSize;
        }
        Destroy(effect, 5f);
    }
}
