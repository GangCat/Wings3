using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldGenerator : MonoBehaviour, IDamageable
{
    public void Init(VoidGameObjectDelegate _destroyCallback, Vector3 _bossPos)
    {
        soundManager = SoundManager.Instance;
        soundManager.AddAudioComponent(gameObject);
        destroyCallback = _destroyCallback;
        curHp = maxHp;
        myCollider = GetComponent<SphereCollider>();

        StartCoroutine(GenIndicatorCoroutine(_bossPos));
        StartCoroutine(RotateCoroutine());
        // 아이들 사운드 루프 실행
    }

    private IEnumerator GenIndicatorCoroutine(Vector3 _bossPos)
    {
        Vector3 indicatorPos = transform.position;
        indicatorPos.y += 58f;
        RaycastHit hit;

        while (!Physics.Raycast(indicatorPos, (_bossPos - indicatorPos).normalized, out hit, 10000f, 1 << LayerMask.NameToLayer("Boss")))
            yield return null;

        GameObject indicator = Instantiate(shieldGenIndicatorPrefab, indicatorPos, Quaternion.LookRotation(_bossPos - indicatorPos));
        indicator.transform.localScale = new Vector3(15f, 15f, Vector3.Distance(indicatorPos, hit.point) * 0.5f);
        indicator.transform.parent = transform;
    }

    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            for(int i = 0; i < rotateModelTr.Length; ++i)
                rotateModelTr[i].rotation *= Quaternion.Euler(Vector3.one * (i + 1) * rotateSpeed * Time.deltaTime);

            yield return new WaitForFixedUpdate();
        }
    }


    public float GetCurHp => curHp;

    public void GetDamage(float _dmg)
    {
        if (curHp < 0)
            return;
        //피격 사운드 실행  일단 보류하기
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.BOSSSHIELDGENERATORHITSOUND);
        curHp -= _dmg;
        shieldHpbar.Damaged();
        BreakRing();

        if (curHp < 0)
        {
            destroyCallback?.Invoke(gameObject);
            // 아이들 사운드 루프 실행 사운드 끝날때까지 파괴 대기
            //쉴드 재생기 파괴되는소리 재생
            soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.BOSSSHIELDGENERATORDESTROYSOUND);
            Destroy(gameObject);
        }
    }

    private void BreakRing()
    {

        if (curHp < maxHp * 0.7f)
        {
            ringGo[0].SetActive(false);
            SetColliderSize(0);
        }
        if (curHp < maxHp * 0.4f)
        {
            ringGo[1].SetActive(false);
            SetColliderSize(1);
        }
        if (curHp < maxHp * 0.1f)
        {
            ringGo[2].SetActive(false);
            SetColliderSize(2);
        }
    }

    private void SetColliderSize(int _radiusIdx)
    {
        myCollider.radius = sphereColliderRadius[_radiusIdx];
    }
    private SoundManager soundManager = null;

    [SerializeField]
    private float curHp = 0;
    [SerializeField]
    private float maxHp = 0;
    [SerializeField]
    private GameObject shieldGenIndicatorPrefab = null;
    [SerializeField]
    private LayerMask bossLayer;
    [SerializeField]
    private float[] sphereColliderRadius = null;

    [Header("InformationForRotation")]
    [SerializeField]
    private Transform[] rotateModelTr = null;
    [SerializeField]
    private GameObject[] ringGo = null;
    [SerializeField]
    private float rotateSpeed = 20f;
    [SerializeField]
    private Ui_ShieldHpbar shieldHpbar = null;

    private VoidGameObjectDelegate destroyCallback = null;
    private SphereCollider myCollider = null;
}
