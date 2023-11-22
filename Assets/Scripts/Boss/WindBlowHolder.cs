using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlowHolder : MonoBehaviour
{
    public void Init()
    {
        windBlowPoints = GetComponentsInChildren<WindBlowPoint>();
        foreach (WindBlowPoint wbp in windBlowPoints)
            wbp.Init();
    }

    public WindBlowPoint[] WindBlowPoints => windBlowPoints;

    private WindBlowPoint[] windBlowPoints = null;
}
