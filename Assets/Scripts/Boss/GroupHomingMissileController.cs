using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class GroupHomingMissile : AttackableObject, IDamageable, ISubscriber
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
    private float deviationAmount = 0;
    [SerializeField] 
    private float deviationSpeed = 0;



    private Vector3 standardPrediction, deviatedPrediction;
    private bool isPhaseChanged = false;
    private bool isShieldBreak = false;
    private bool isBodyTrigger = true;
    private bool isExplosed = false;
    private GroupMissileMemoryPool groupMissileMemoryPool = null;
    private CustomAudioManager customAudioManager = null;
    private Transform playerTr;
    private VisualEffect vfx = null;
    private MeshRenderer mr = null;

    private SoundManager soundManager = null;


    private enum EGroupMissileAudio
    {
        NONE = -1,
        NORMALEXPLOSIONSOUND,
        HOMINGMISSILEPASSINGSOUND
    }

    public float GetCurHp => throw new NotImplementedException();

    public void Init(Transform _playerTr, Vector3 _spawnPos, Quaternion _spawnRot, GroupMissileMemoryPool _groupMissileMemoryPool, bool _isShieldBreak)
    {
        soundManager = SoundManager.Instance;
        soundManager.Init(gameObject);
            customAudioManager = GetComponent<CustomAudioManager>();
        if (!vfx)
            vfx = GetComponentInChildren<VisualEffect>();
        if (!mr)
            mr = GetComponentInChildren<MeshRenderer>();

        playerTr = _playerTr;
        groupMissileMemoryPool = _groupMissileMemoryPool;
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

        vfx.Reinit();
        mr.enabled = true;

        //deviationAmount = UnityEngine.Random.Range(5f, 20f);
        //deviationSpeed = UnityEngine.Random.Range(0.1f, 1f);
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.GROUPHOMINGMISSILEPASSINGSOUND, true);

        Subscribe();
        StartCoroutine(SetIsfirstTriggerFalseCoroutine());
        StartCoroutine(SetIsbodyTriggerFalseCoroutine());
        StartCoroutine(AutoDestroyCoroutine());
        StartCoroutine("FixedUpdateCoroutine");
    }

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
            // 미사일의 뒤에 불꽃 점화소리(타는 소리), 플레이어와의 거리 계산, 가까울 수록 볼륨은 커진다.
            if (isPhaseChanged)
            {
                groupMissileMemoryPool.DeactivateGroupMissile(gameObject);
                yield break;
            }

            rb.velocity = transform.forward * speed;
            float distanceBetweenPlayer = Vector3.Distance(gameObject.transform.position, playerTr.position);
            if (distanceBetweenPlayer < 15f)
            {
                customAudioManager.PlayAudio((int)EGroupMissileAudio.HOMINGMISSILEPASSINGSOUND, true);
            }
            else
            {
                customAudioManager.StopAllAudio();
            }
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

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("BossShield") && isShieldBreak)
    //        Explosion();
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BossShield"))
            isFirstTrigger = false;
        //else if (other.CompareTag("BossBody"))
        //    isBodyTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!isBodyTrigger)
            Explosion();
    }


    public void GetDamage(float _dmg)
    {
        Explosion();
    }

    public void Explosion()
    {
        if (isExplosed)
            return;

        //StopCoroutine("AutoExplosionCorutine");
        isExplosed = true;
        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        go.transform.localScale = Vector3.one * explosionRange;
        
        Destroy(go, 5f);
        customAudioManager.PlayAudio((int)EGroupMissileAudio.NORMALEXPLOSIONSOUND);
        Collider[] arrTempCollider = Physics.OverlapSphere(transform.position, explosionRange, explosionLayer);
        foreach (Collider col in arrTempCollider)
        {
            Debug.Log(col.name);
            KnockBack(col);
            AttackDmg(col);
        }
        if (soundManager.IsPlaying(GetComponent<AudioSource>()))
        {
            soundManager.StopAudio(GetComponent<AudioSource>());
        }
        StopAllCoroutines();
        rb.velocity = Vector3.zero;
        StartCoroutine(DeactivateCoroutine());
        //groupMissileMemoryPool.DeactivateGroupMissile(gameObject);
        // 플레이어와의 거리 계산 > 가까울 수록 볼륨은 커진다 > 미사일이 폭발하는 소리 재생
    }

    private IEnumerator DeactivateCoroutine()
    {
        vfx.Stop();
        mr.enabled = false;

        yield return new WaitForSeconds(effectDisableDelay);
        groupMissileMemoryPool.DeactivateGroupMissile(gameObject);
    }

    private void OnDisable()
    {
        if(vfx)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, standardPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(standardPrediction, deviatedPrediction);
    }
}
