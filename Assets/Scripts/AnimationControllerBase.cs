using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerBase : MonoBehaviour
{
    public virtual void Init()
    {
        anim = GetComponent<Animator>();
    }

    public void SetBool(string _paramName, bool _value)
    {
        anim.SetBool(_paramName, _value);
    }

    public void SetTriggger(string _paramName)
    {
        anim.SetTrigger(_paramName);
    }

    public void SetFloat(string _paramName, float _value)
    {
        anim.SetFloat(_paramName, _value);
    }

    public void SetInt(string _paramName, int _value)
    {
        anim.SetInteger(_paramName, _value);
    }

    protected Animator anim = null;
}
