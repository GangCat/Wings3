using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHp : MonoBehaviour
{
    public virtual void HealHp(float _heal)
    {
        curHp = curHp + _heal > maxHp ? maxHp : curHp + _heal;
    }

    [SerializeField]
    protected float maxHp = 0f;
    [SerializeField]
    protected float curHp = 0f;
}
