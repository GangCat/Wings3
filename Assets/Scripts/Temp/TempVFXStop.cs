using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TempVFXStop : MonoBehaviour
{
    public bool stop;
    public VisualEffect vfx;
    bool isstopped;

    private void Update()
    {
        if (stop && !isstopped)
        {
            isstopped = true;
            vfx.Stop();
        }
    }
}
