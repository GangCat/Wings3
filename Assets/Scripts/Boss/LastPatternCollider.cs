using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPatternCollider : MonoBehaviour, IDamageable
{
    public void Init(VoidVoidDelegate _patternFinishCallback)
    {
        boxCol = GetComponent<BoxCollider>();
        patternFinishCallback = _patternFinishCallback;
    }

    public void Enable(bool _value)
    {
        boxCol.enabled = _value;
    }

    public void GetDamage(float _dmg)
    {
        patternFinishCallback?.Invoke();
    }

    private BoxCollider boxCol = null;

    public float GetCurHp => 1f;

    private VoidVoidDelegate patternFinishCallback = null;
}
