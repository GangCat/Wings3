using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public void Init(
        Transform _PlayerTr, 
        VoidIntDelegate _cameraActionCallback, 
        VoidFloatDelegate _hpUpdateCallback, 
        VoidFloatDelegate _shieldUpdateCallback, 
        VoidVoidDelegate _removeShieldCallback, 
        BossController.GetRandomSpawnPointDelegate _callback, 
        VoidVoidDelegate _bossClearCallback)
    {
        exCtrl = GetComponentInChildren<ExplosionEffectController>();
        exCtrl.Init();
        bossCtrl = GetComponentInChildren<BossController>();
        bossCtrl.Init(_PlayerTr, _cameraActionCallback, _hpUpdateCallback, _shieldUpdateCallback, _callback, _bossClearCallback, _removeShieldCallback, exCtrl.StartExplosion);
    }

    public void ClearCurPhase()
    {
        bossCtrl.ClearShieldGenerator();
    }

    public void ActionFinish()
    {
        bossCtrl.PatternStart();
    }

    public void JumpToNextPattern()
    {
        bossCtrl.JumpToNextPhase();
    }

    public void GameStart()
    {
        bossCtrl.GameStart();
    }
    public void FinishiDebug()
    {
        bossCtrl.BossClear();
    }

    private BossController bossCtrl = null;
    private ExplosionEffectController exCtrl = null;
}
