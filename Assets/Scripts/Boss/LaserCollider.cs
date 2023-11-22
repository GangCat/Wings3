using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollider : MonoBehaviour
{
    public void Init(VoidVoidDelegate _collisionCallback)
    {
        collisionCallback = _collisionCallback;
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionCallback?.Invoke();
    }

    private VoidVoidDelegate collisionCallback = null;
}
