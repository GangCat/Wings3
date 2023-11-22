using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp1 : MonoBehaviour
{
    private void Start()
    {
        //rb.velocity = transform.forward * 10f;
        //rb.angularVelocity = Vector3.up * (10 * Mathf.Deg2Rad);
    }

    private void Update()
    {
        //transform.position += transform.forward * 10 * Time.deltaTime;
        transform.rotation *= Quaternion.Euler(Vector3.up * 10 * Time.deltaTime);
    }

    public Rigidbody rb;
    public float accel;
}
