using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �Ʒ��� �������� �ӵ� ���� �ش�
/// �÷��̾�� �ε����� �����ϰ� �������� �ش�.
/// ���� �ε����� �����Ѵ�.
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
        soundManager.Init(gameObject);
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
            
            //�÷��̾���� �Ÿ���� > ����� ���� �Ҹ� ���� > ��ź�������� �ٶ� �Ҹ�
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
        //������ �� ���� ��ü�� �浹���� �˻� > �÷��̾���� �Ÿ���� > ������ ������ ��ź �Ҹ� or �׳� ���߼Ҹ��� �˸´� �Ҹ� ����
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
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }
    private void SetObjectToVisible()
    {
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
