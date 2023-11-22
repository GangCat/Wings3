using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatilinSpinController : MonoBehaviour
{

    public void StartSpin()
    {
        isSpin = true;
    }

    public void StopSpin()
    {
        isSpin = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isSpin)
            transform.rotation *= Quaternion.Euler(Vector3.forward * rotateSpeedDegree * Time.fixedDeltaTime);
    }

    [SerializeField]
    private float rotateSpeedDegree = 30f;
    [SerializeField]
    private bool isSpin = false;
}
