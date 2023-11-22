using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GiantHomingMissileController : AttackableObject, IDamageable, ISubscriber
{
    

    [Header("-Information")]
    [SerializeField] 
    private Rigidbody rb;
    [SerializeField] 
    private GameObject explosionPrefab;
    [SerializeField] 
    private float explosionRange = 0f;
    [SerializeField] 
    private LayerMask explosionLayer;
    [SerializeField]
    private float effectDisableDelay = 2f;
    [SerializeField]
    private float autoDestroyDelay = 15f;
    [SerializeField]
    private float bodyTriggerChangeDelay = 0.5f;
    [SerializeField]
    private float firstTriggerChangeDelay = 2f;

    [Header("-Move")]
    [SerializeField] 
    private float speed = 15;
    [SerializeField] 
    private float rotateSpeed = 95;

    [Header("-Predict")]
    [SerializeField] 
    private float maxDistancePredict = 100;
    [SerializeField] 
    private float minDistancePredict = 5;
    [SerializeField] 
    private float maxTimePrediction = 5;

    [Header("-Deviate")]
    [SerializeField] 
    private float deviationAmount = 50;
    [SerializeField] 
    private float deviationSpeed = 2;

    private Vector3 standardPrediction, deviatedPrediction;
    private bool isPhaseChanged = false;
    private bool isShieldBreak = false;
    private bool isBodyTrigger = true;
    private bool isExplosed = false;
    private Transform playerTr = null;
    private CustomAudioManager customAudioManager;
    private VisualEffect vfx = null;
    private MeshRenderer mr = null;

    private SoundManager soundManager = null;


    public void Init(Vector3 _spawnPos, Quaternion _spawnRot, bool _isShieldBreak, Transform _playerTr)
    {
        soundManager = SoundManager.Instance;
        soundManager.AddAudioComponent(gameObject);
            customAudioManager = GetComponent<CustomAudioManager>();
        if (!vfx)
            vfx = GetComponentInChildren<VisualEffect>();
        if (!mr)
            mr = GetComponentInChildren<MeshRenderer>();

        playerTr = _playerTr;
        transform.position = _spawnPos;
        transform.rotation = _spawnRot;

        isPhaseChanged = false;
        isBodyTrigger = true;
        isExplosed = false;
        isShieldBreak = _isShieldBreak;
        if (isShieldBreak)
            isFirstTrigger = false;
        else
            isFirstTrigger = true;

        vfx.Play();

        //deviationAmount = UnityEngine.Random.Range(30f, 70f);
        //deviationSpeed = UnityEngine.Random.Range(1f, 3f);
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.GIANTHOMINGMISSILEPASSINGSOUND, true);

        Subscribe();
        StartCoroutine(SetIsfirstTriggerFalseCoroutine());
        StartCoroutine(SetIsbodyTriggerFalseCoroutine());
        StartCoroutine(AutoDestroyCoroutine());
        StartCoroutine("FixedUpdateCoroutine");
    }

    public float GetCurHp => 100f;

    private IEnumerator SetIsfirstTriggerFalseCoroutine()
    {
        yield return new WaitForSeconds(firstTriggerChangeDelay);

        isFirstTrigger = false;
    }

    private IEnumerator SetIsbodyTriggerFalseCoroutine()
    {
        yield return new WaitForSeconds(bodyTriggerChangeDelay);
        isBodyTrigger = false;
    }

    private IEnumerator AutoDestroyCoroutine()
    {
        yield return new WaitForSeconds(autoDestroyDelay);

        Explosion();
    }


    private IEnumerator FixedUpdateCoroutine()
    {
        while (true)
        {
            // 플레이어와의 거리계산 > 가까울 수록 볼륨 크게 > 미사일 점화소리
            if (isPhaseChanged)
            {
                Destroy(gameObject);
                yield break;
            }

            rb.velocity = transform.forward * speed;

            var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, playerTr.position));

            PredictMovement(leadTimePercentage);
            AddDeviation(leadTimePercentage);

            RotateRocket();

            yield return new WaitForFixedUpdate();
        }
    }

    private void PredictMovement(float _leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, maxTimePrediction, _leadTimePercentage);

        standardPrediction = playerTr.position + playerTr.GetComponent<Rigidbody>().velocity * predictionTime;
    }

    private void AddDeviation(float _leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * _leadTimePercentage;

        deviatedPrediction = standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (!isShieldBreak && isFirstTrigger)
            return;

        else if (isBodyTrigger && _other.CompareTag("BossBody"))
            return;

        Explosion();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BossShield") && isShieldBreak)
            Explosion();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BossShield"))
            isFirstTrigger = false;
        //else if (other.CompareTag("BossBody"))
        //    isBodyTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isBodyTrigger)
            Explosion();
    }

    public void GetDamage(float _dmg)
    {
        Explosion();
    }


    private void Explosion()
    {
        if (isExplosed)
            return;

        // 플레이어와의 거리 계산 > 가까울 수록 볼륨 크게 > 대형 미사일 폭발 소리
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.GIANTHOMINGMISSILEXPLOSIONSOUND);
        SetObjectToInvisible();
        //StopCoroutine("AutoExplosionCorutine");
        isExplosed = true;
        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        go.transform.localScale = Vector3.one * explosionRange;
        
        Destroy(go, 5f);

        Collider[] arrTempCollider = Physics.OverlapSphere(transform.position, explosionRange, explosionLayer);
        foreach (Collider col in arrTempCollider)
        {
            Debug.Log(col.name);
            KnockBack(col);
            AttackDmg(col);
        }

        StopAllCoroutines();
        rb.velocity = Vector3.zero;
        StartCoroutine(DeactivateCoroutine());
    }

    private IEnumerator DeactivateCoroutine()
    {
        vfx.Stop();
        mr.enabled = false;

        yield return new WaitForSeconds(effectDisableDelay);

        StopAllCoroutines();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (vfx)
            vfx.Stop();
        Broker.UnSubscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void ReceiveMessage(EMessageType _message)
    {
        switch (_message)
        {
            case EMessageType.PHASE_CHANGE:
                isPhaseChanged = true;
                break;
            case EMessageType.SHIELD_BROKEN:
                isShieldBreak = true;
                break;
            default:
                break;
        }
    }
    private void SetObjectToInvisible()
    {
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }
    private void SetObjectToVisible()
    {
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, standardPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(standardPrediction, deviatedPrediction);
    }
}
