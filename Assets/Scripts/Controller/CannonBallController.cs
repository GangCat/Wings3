using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 아래로 떨어지는 속도 값을 준다
/// 플레이어와 부딪히면 폭발하고 데미지를 준다.
/// 땅과 부딪히면 폭발한다.
/// </summary>
public class CannonBallController : AttackableObject, ISubscriber
{
    private float speed;
    private WaitForFixedUpdate waitFixedUpdate = null;
    private CannonMemoryPool memoryPool = null;
    private bool isPhaseChanged = false;
    private PlayEffectAudioDelegate audioCallback = null;

    private SoundManager soundManager = null;
    public void Init(float _speed, Vector3 _spawnPos, CannonMemoryPool _memoryPool = null, PlayEffectAudioDelegate _audioCallback = null)
    {
        soundManager = SoundManager.Instance;
        soundManager.AddAudioComponent(gameObject);
        speed = _speed;
        transform.position = _spawnPos;
        memoryPool = _memoryPool;
        waitFixedUpdate = new WaitForFixedUpdate();
        isPhaseChanged = false;
        audioCallback = _audioCallback;
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.CANNONPASSINGSOUND, true);

        Subscribe();
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            
            //플레이어와의 거리계산 > 가까울 수록 소리 증폭 > 포탄지나가는 바람 소리
            if (transform.position.y < 0)
            {
                memoryPool.DeactivateCannonBall(gameObject);
                yield break;
            }

            transform.position += Vector3.down * speed * Time.deltaTime;

            yield return waitFixedUpdate;

            if(isPhaseChanged)
            {
                memoryPool.DeactivateCannonBall(gameObject);
                yield break;
            }
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Floor"))
        {
            soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.CANNONWATERCRUSHSOUND);
        }
        else
        {
            soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.CANNONCRUSHSOUND);
        }

        
        if (_other.gameObject.layer == LayerMask.NameToLayer("BossBody"))
        {
            SetObjectToInvisible();
            Invoke("SetObjectToVisible", 5f);
            memoryPool.DeactivateCannonBall(gameObject);
        }

        if (AttackDmg(_other))
        {
            SetObjectToInvisible();
            Invoke("SetObjectToVisible", 5f);
            memoryPool.DeactivateCannonBall(gameObject);
        }
        //물인지 그 외의 물체와 충돌인지 검사 > 플레이어와의 거리계산 > 물에서 터지는 포탄 소리 or 그냥 폭발소리중 알맞는 소리 삽입
    }

    private void OnDisable()
    {
        audioCallback?.Invoke(EEffectAudio.CannonBallDestroy);
        Broker.UnSubscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void ReceiveMessage(EMessageType _message)
    {
        if (_message == EMessageType.PHASE_CHANGE)
            isPhaseChanged = true;
    }
    private void SetObjectToInvisible()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }
    private void SetObjectToVisible()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
