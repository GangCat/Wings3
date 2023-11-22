using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupHomingMissileSpawnPos : MonoBehaviour
{
    public Vector3 GetPos()
    {
        return transform.position;
    }

    public Quaternion GetRot()
    {
        return transform.rotation;
    }
}
