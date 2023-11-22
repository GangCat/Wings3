using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPull : MonoBehaviour
{
    public void Init()
    {
        waitDelay = new WaitForSeconds(0.2f);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out var _component))
        {
            _component.isKinematic = false;
            StartCoroutine(SetVelocity(other));
        }
        
    }

    private void FixedUpdate()
    {
        transform.localScale += Vector3.one * Time.fixedDeltaTime;
    }


    private IEnumerator SetVelocity(Collider _other)
    {
        // 커브에서 해당 시간의 값 가져옴
        pullForce = curve.Evaluate((Time.time * 0.1f) % curve.length);

        Vector3 forceDir = transform.position - _other.transform.position;
        _other.GetComponent<Rigidbody>().AddForce(forceDir.normalized * pullForce * pullForceAmount);
        // 힘 주는 주기
        yield return waitDelay;

        if (_other == null)
            yield break;

        // 
        //if(Vector3.SqrMagnitude(transform.position - _other.transform.position) < Mathf.Pow(3f, 2f))
        //    StartCoroutine(SetVelocityUp(_other));
        //else
        StartCoroutine(SetVelocity(_other));
    }

    private IEnumerator SetVelocityUp(Collider _other)
    {
        _other.GetComponent<Rigidbody>().AddForce(Vector3.up * upForce);
        yield return waitDelay;
        StartCoroutine(SetVelocityUp(_other));
    }


    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private float pullForceAmount = 200f;
    [SerializeField]
    private  float upForce = 2000f;
    [SerializeField]
    private float rightForce = 100f;
    [SerializeField]
    private float scaleIncreaseRatio = 50f;

    private WaitForSeconds waitDelay = null;
    private float pullForce;
}
