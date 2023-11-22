using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void StartEffect()
    {
        gameObject.SetActive(true);
    }

}
