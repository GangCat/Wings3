using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아래로 떨어지는 속도 값을 준다
/// 플레이어와 부딪히면 폭발하고 데미지를 준다.
/// 땅과 부딪히면 폭발한다.
/// </summary>
public class CannonRainBallController : AttackableObject, ISubscriber
{
    private enum ECannonAudio
    {
        NONE = -1,
        UNDERWATEREXPLOSIONSOUND,
        NORMALEXPLOSIONSOUND,
        CANNONBALLPASSINGSOUND
    }


    private float speed;
    private WaitForFixedUpdate waitFixedUpdate = null;
    private CannonRainMemoryPool memoryPool = null;
    private bool isPhaseChanged = false;
    private GameObject attackAreaPrefab = null;
    private CustomAudioManager customAudioManger = null;
    private MeshRenderer mr = null;
    private Collider col = null;
    private Transform playerTr = null;
    private SoundManager soundManager = null;

    public void Init(float _speed, Vector3 _spawnPos, CannonRainMemoryPool _memoryPool = null, GameObject _attackAreaPrefab = null, Transform _playerTr= null)
    {
        speed = _speed;
        transform.position = _spawnPos;
        memoryPool = _memoryPool;
        waitFixedUpdate = new WaitForFixedUpdate();
        isPhaseChanged = false;
        Vector3 attackAreaSpawnPos = _spawnPos;
        attackAreaSpawnPos.y = 60f;
        attackAreaPrefab = Instantiate(_attackAreaPrefab, attackAreaSpawnPos, Quaternion.identity);
        Subscribe();
        customAudioManger = GetComponent<CustomAudioManager>();
        customAudioManger.Init();
        playerTr = _playerTr;
        mr = GetComponentInChildren<MeshRenderer>();
        col = GetComponent<Collider>();
        soundManager = SoundManager.Instance;
        soundManager.Init(gameObject);

        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.CANNONPASSINGSOUND, true);

        StartCoroutine("UpdateCoroutine");
    }

    
    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if(transform.position.y < 0)
            {
                StartCoroutine("DeactivedAndPlayUnderWaterExplosionSoundCoroutine");
                yield break;
            }
            transform.position += Vector3.down * speed * Time.deltaTime;
            //float distanceBetweenPlayer = Vector3.Distance(playerTr.position, gameObject.transform.position);
            //if (distanceBetweenPlayer < 15f)
            //{
            //    customAudioManger.PlayAudio((int)ECannonAudio.CANNONBALLPASSINGSOUND);
            //}


            
            yield return waitFixedUpdate;

            if (isPhaseChanged)
            {
                StopAllCoroutines();
                memoryPool.DeactivateCannonBall(gameObject);
            }
            //캐논이 떨어지면서 생기는 바람 소리, 플레이어와의 거리 계산, 가까울 수록 볼륨은 커진다.
        }
    }

    private IEnumerator DeactivedAndPlayUnderWaterExplosionSoundCoroutine()
    {
        StopCoroutine("UpdateCoroutine");

        //customAudioManger.PlayAudio((int)ECannonAudio.UNDERWATEREXPLOSIONSOUND);
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.CANNONWATERCRUSHSOUND);
        mr.enabled = false;
        col.enabled = false;
        //while (customAudioManger.IsPlaying())
        while (soundManager.IsPlaying(GetComponent<AudioSource>()))
        {
            yield return null;
        }
        mr.enabled = true;
        col.enabled = true;
        memoryPool.DeactivateCannonBall(gameObject);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (AttackDmg(_other))
        {            
            StartCoroutine("DeactivedAndPlayNormalExplosionSoundCoroutine");
            // 물과의 충돌일 경우 > 캐논이 물에서 폭발하는 소리재생
            // 물을 제외한 모든 콜라이더일 경우 > 플레이어와의 거리 계산 > 가까울 수록 볼륨은 커진다 > 캐논이 폭발하는 소리 재생
        }
    }

    private IEnumerator DeactivedAndPlayNormalExplosionSoundCoroutine()
    {
        StopCoroutine("UpdateCoroutine");

        //customAudioManger.PlayAudio((int)ECannonAudio.NORMALEXPLOSIONSOUND);
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.CANNONCRUSHSOUND);
        mr.enabled = false;
        col.enabled = false;
        //while (customAudioManger.IsPlaying())
        while (soundManager.IsPlaying(GetComponent<AudioSource>()))
        {
            yield return null;
        }
        mr.enabled = true;
        col.enabled = true;
        memoryPool.DeactivateCannonBall(gameObject);
    }

    private void OnDisable()
    {
        Broker.UnSubscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void ReceiveMessage(EMessageType _message)
    {
        if(_message == EMessageType.PHASE_CHANGE)
            isPhaseChanged = true;
    }
}
