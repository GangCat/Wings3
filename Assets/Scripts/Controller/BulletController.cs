using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : AttackableObject, ISubscriber
{
    [SerializeField]
    private float speed = 200f;

    private GatlinMemoryPool gatlinMemoryPool = null;
    private bool isPhaseChange = false;
    private bool isBodyTrigger = true;

    private SoundManager soundManager = null;

    public void Init(float _destroyTime, Vector3 _position, Quaternion _rotation,GatlinMemoryPool _gatlinMemoryPooll = null)
    {
        soundManager = SoundManager.Instance;
        soundManager.AddAudioComponent(gameObject);
        //Destroy(gameObject, _destroyTime);
        Invoke("DeactivateBullet", _destroyTime);
        gameObject.transform.position = _position;
        gameObject.transform.rotation = _rotation;
        gatlinMemoryPool = _gatlinMemoryPooll;
        isPhaseChange = false;
        isBodyTrigger = true;
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.BULLETPASSINGSOUND,true);
        //플레이오와의 거리계산 > 가까울수록 소리 증폭 > 총알이 지나가는 소리
    }
    //private void Start()
    //{
    //    rb.velocity = transform.forward * speed;
    //}

    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;

        if (isPhaseChange)
            DeactivateBullet();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("GatlinGunMuzzle"))
            return;
        else if (_other.CompareTag("BossShield"))
            return;
        else if (isBodyTrigger && _other.gameObject.layer == LayerMask.NameToLayer("BossBody"))
            return;
        // 플레이어에 피격, 물에 피격, 그외의 물체 피격 조건 검사 > 물 혹은 그외의 물체일 경우 거리 계산 후 소리 증폭 > 알맞는 소리 삽입
        if (_other.CompareTag("Floor"))
        {
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.BULLETWATERHITSOUND);
        }
        else if (_other.CompareTag("Player"))
        { 
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.BULLETPLAYERHITSOUND);
        }
        else
        {
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.BULLETHARDOBJECTHITSOUND);
        }
        if (_other.gameObject.layer == LayerMask.NameToLayer("BossBody"))
            DeactivateBullet();
        else
        {
            AttackDmg(_other);
            DeactivateBullet();
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("BossBody"))
            isBodyTrigger = false;
    }

    private void DeactivateBullet()
    {
        if (soundManager.IsPlaying(GetComponent<AudioSource>()))
        {
            soundManager.StopAudio(GetComponent<AudioSource>());
        }
        gatlinMemoryPool.DeactivateBullet(gameObject);
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
        if (_message == EMessageType.PHASE_CHANGE)
            isPhaseChange = true;
    }
}
