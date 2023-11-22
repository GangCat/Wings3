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
        soundManager.Init(gameObject);
        //Destroy(gameObject, _destroyTime);
        Invoke("DeactivateBullet", _destroyTime);
        gameObject.transform.position = _position;
        gameObject.transform.rotation = _rotation;
        gatlinMemoryPool = _gatlinMemoryPooll;
        isPhaseChange = false;
        isBodyTrigger = true;
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.BULLETPASSINGSOUND,true);
        //�÷��̿����� �Ÿ���� > �������� �Ҹ� ���� > �Ѿ��� �������� �Ҹ�
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
        // �÷��̾ �ǰ�, ���� �ǰ�, �׿��� ��ü �ǰ� ���� �˻� > �� Ȥ�� �׿��� ��ü�� ��� �Ÿ� ��� �� �Ҹ� ���� > �˸´� �Ҹ� ����
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
