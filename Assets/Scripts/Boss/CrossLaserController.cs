using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossLaserController : AttackableObject, ISubscriber
{
    public void Init(float _slowExpandTime, float _autoDestroyTime, float _slowExpandSpeed, float _fastExpandSpeed)
    {
        slowExpandTime = _slowExpandTime;
        autoDestroyTime = _autoDestroyTime;
        slowExpandSpeed = _slowExpandSpeed;
        fastExpandSpeed = _fastExpandSpeed;
        Subscribe();
        StartCoroutine(FixedUpdateCoroutine());
    }

    private IEnumerator FixedUpdateCoroutine()
    {
        float startTime = Time.time;
        while(Time.time - startTime < slowExpandTime)
        {
            transform.localScale += Vector3.one * slowExpandSpeed * Time.fixedDeltaTime;

            yield return waitFixedUpdate;
        }

        while (Time.time - startTime < autoDestroyTime)
        {
            transform.localScale += Vector3.one * fastExpandSpeed * Time.fixedDeltaTime;

            yield return waitFixedUpdate;
        }

        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider _other)
    {
        AttackDmg(_other);
    }

    private void OnDestroy()
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
            Destroy(gameObject);
    }

    private WaitForFixedUpdate waitFixedUpdate = null;
    private float slowExpandTime = 3f;
    private float autoDestroyTime = 10f;
    private float fastExpandSpeed = 10f;
    private float slowExpandSpeed = 10f;
}
