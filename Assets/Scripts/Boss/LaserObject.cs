using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObject : MonoBehaviour
{
    public void Init(VoidVoidDelegate _collisionCallback)
    {
        colliders = GetComponentsInChildren<LaserCollider>();
        foreach (LaserCollider lc in colliders)
            lc.Init(_collisionCallback);
    }

    private LaserCollider[] colliders = null;
}
