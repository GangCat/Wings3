using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDestroyTrigger : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
