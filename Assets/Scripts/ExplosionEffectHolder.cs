using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectHolder : MonoBehaviour
{
    public void Init()
    {
        effects = GetComponentsInChildren<ExplosionEffect>();
        foreach (var ef in effects)
            ef.Init();
    }

    public void StartEffect()
    {
        foreach (var ef in effects)
            ef.StartEffect();
    }

    private ExplosionEffect[] effects = null;
}
