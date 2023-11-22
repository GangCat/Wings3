using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldGeneratorSpawnPoint : MonoBehaviour
{
    public void Init()
    {
    //    windBlowHolder = GetComponentInChildren<WindBlowHolder>();
    //    windBlowHolder.Init();
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    public WindBlowHolder GetWindBlowHolder()
    {
        //return windBlowHolder;
        return null;
    }

    //private WindBlowHolder windBlowHolder = null;
}
