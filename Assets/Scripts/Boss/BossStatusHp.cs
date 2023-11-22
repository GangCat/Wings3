using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatusHp : StatusHp, IBossDamageable
{
    public void Init(VoidVoidDelegate _phaseChangeCallback, VoidFloatDelegate _hpUpdateCallback)
    {
        curHp = maxHp;
        curPhaseNum = 1;
        phaseChangeCallback = _phaseChangeCallback;
        hpUpdateCallback = _hpUpdateCallback;
    }

    public float GetCurHp => curHp;
    public float GetMaxHp => maxHp;

    public void GetDamage(float _dmg)
    {
        if (curHp < 0)
            return;

        curHp -= _dmg;

        if (curPhaseNum == 1 && curHp < maxHp * 0.5f)
            ChangePhase();
        else if (curPhaseNum == 2 && curHp < 0)
        {
            ChangePhase();
            curHp = 0f;
        }

        hpUpdateCallback?.Invoke(curHp / maxHp);
    }

    private void ChangePhase()
    {
        ++curPhaseNum;
        phaseChangeCallback?.Invoke();
    }

    public void GetDamage(float _dmg, GameObject _attackGo)
    {
        curHp -= _dmg;

        if (curPhaseNum == 1 && curHp < maxHp * 0.5f)
            ChangePhase();
        else if (curPhaseNum == 2 && curHp < 0)
            ChangePhase();

        hpUpdateCallback?.Invoke(curHp / maxHp);
    }

    private VoidVoidDelegate phaseChangeCallback = null;
    private VoidFloatDelegate hpUpdateCallback = null;
    private int curPhaseNum = 0;
}
