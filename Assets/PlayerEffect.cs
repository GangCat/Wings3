using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(EffectActive());
    }

    private IEnumerator EffectActive()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }

}
