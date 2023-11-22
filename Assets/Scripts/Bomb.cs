using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public void Init(Vector3 _targetPos, float _launchAngle, float _gravity, Transform _targetTr, Color _color, int _idx)
    {
        launchAngle = _launchAngle;
        targetPos = _targetPos;
        gravity = _gravity;
        targetTr = _targetTr;
        myColor = new Color(_color.r * 500f, _color.g * 500f, _color.b * 500f, _color.a);
        myIdx = _idx;
        waitFixedTime = new WaitForFixedUpdate();
        soundManager = SoundManager.Instance;
        soundManager.AddAudioComponent(gameObject);
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_EmissionColor", _color * 2f);
        decoSphereGo.GetComponent<MeshRenderer>().SetPropertyBlock(block);

        //Test();
        StartCoroutine(SimulateProjectile());
        StartCoroutine("SpinModelCoroutine");
    }

    private void SetColor()
    {
        GetComponentInChildren<ParticleSystemRenderer>().material.SetColor("_BaseColor", myColor);
        GetComponentInChildren<ParticleSystemRenderer>().material.SetFloat("_Dissolve", 0.8f);
    }

    IEnumerator SimulateProjectile()
    {
        // 시작 전 잠시 딜레이
        yield return new WaitForSeconds(0.1f);

        // 거리 계산
        float targetDistance = Vector3.Distance(transform.position, targetPos);

        float sinAngle = Mathf.Sin(launchAngle * Mathf.Deg2Rad);
        float cosinAngle = Mathf.Cos(launchAngle * Mathf.Deg2Rad);

        // 각도와 거리를 이용한 초기속도 계산
        float initVelocity = Mathf.Sqrt(targetDistance / (2 * sinAngle * cosinAngle / gravity));

        // 초기 속도를 이용한 수평, 수직 속도 계산
        float HorizontalVelocity = initVelocity * cosinAngle;
        float VerticalVelocity = initVelocity * sinAngle;

        // 총 비행시간 계산
        float flightDuration = targetDistance / HorizontalVelocity;

        // 타겟 방향으로 회전
        // Translate이기 때문에 돌림
        transform.rotation = Quaternion.LookRotation(targetPos - projectile.position);

        float elapsedtime = 0;
        while (elapsedtime < flightDuration)
        {
            transform.Translate(0, (VerticalVelocity - (gravity * elapsedtime)) * Time.fixedDeltaTime, HorizontalVelocity * Time.fixedDeltaTime);
            elapsedtime += Time.fixedDeltaTime;

            yield return waitFixedTime;
        }

        particleGo.transform.rotation = Quaternion.Euler(Vector3.left * 90f);
        StopCoroutine("SpinModelCoroutine");
        SetColor();
        soundManager.PlayAudio(audioSource, (int)SoundManager.ESounds.TIMEBOMBTIMEFLOWSOUND,true);
    }

    private IEnumerator SpinModelCoroutine()
    {
        while (true)
        {
            modelGo.transform.localRotation *= Quaternion.Euler(Vector3.one * 50f * Time.fixedDeltaTime);
            yield return waitFixedTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trig");

        if (other.TryGetComponent<LaserController>(out var comp))
        {
            if (comp.GetIdx.Equals(myIdx))
            {
                soundManager.PlayAudio(audioSource, (int)SoundManager.ESounds.TIMEBOMBEXPLOSIONSOUND);
                SetObjectToInvisible();
                Destroy(gameObject, 5f);
            }
        }
    }

    public void Explosion()
    {
        // 폭발하며 플레이어에게 큰 데미지
        targetTr.GetComponent<IPlayerDamageable>().ForceGetDmg(150);
        // 화면 연출
        
        soundManager.PlayAudio(audioSource, (int)SoundManager.ESounds.TIMEBOMBEXPLOSIONSOUND);
        
        SetObjectToInvisible();
        Destroy(Instantiate(explosionPrefab, transform.position, Quaternion.identity), 5f);
        Debug.Log("Explosion!");
        Destroy(gameObject,5f);
    }
    private void SetObjectToInvisible()
    {
        ch1.SetActive(false);
        ch2.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = false;
    }

    [SerializeField]
    private GameObject ch1;
    [SerializeField]
    private GameObject ch2;
    [SerializeField]
    private Transform projectile;
    [SerializeField]
    private GameObject explosionPrefab = null;
    [SerializeField]
    private GameObject modelGo = null;
    [SerializeField]
    private GameObject particleGo = null;
    [SerializeField]
    private GameObject decoSphereGo = null;

    private Vector3 targetPos;
    private float launchAngle = 45.0f;
    private float gravity = 9.81f;


    private WaitForFixedUpdate waitFixedTime = null;
    private Rigidbody rb = null;
    private Transform targetTr = null;
    private Color myColor = Color.black;
    private SoundManager soundManager = null;
    private AudioSource audioSource = null;
    private int myIdx = -1;
}
