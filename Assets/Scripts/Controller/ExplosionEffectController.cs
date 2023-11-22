using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectController : MonoBehaviour
{
    public void Init()
    {
        holders = GetComponentsInChildren<ExplosionEffectHolder>();

        foreach (var ex in holders)
            ex.Init();
    }

    public void StartExplosion()
    {
        StartCoroutine(StartExplosionCoroutine());
    }

    private IEnumerator StartExplosionCoroutine()
    {
        holders[0].StartEffect();
        yield return new WaitForSeconds(explosionDelays[0]);

        holders[1].StartEffect();
        yield return new WaitForSeconds(explosionDelays[1]);

        holders[2].StartEffect();
        yield return new WaitForSeconds(explosionDelays[2]);

        holders[3].StartEffect();
    }

    private ExplosionEffectHolder[] holders = null;

    [SerializeField]
    private float[] explosionDelays = null;
}
