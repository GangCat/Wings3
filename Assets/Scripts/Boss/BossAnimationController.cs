using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBossAnimator
{
    NONE = -1,
    Body,
    Body_Under,
    Leg,
    Radar,
    Missile_Door,
    TimeBomb_Cannon,
    Redzone_Cannon,
    Big_Missile_Door
}

public class BossAnimationController : MonoBehaviour
{
    public void Init()
    {

    }

    public void BossStandUp()
    {
        bossAnims[(int)EBossAnimator.Body].SetTrigger("doStandUp");
        bossAnims[(int)EBossAnimator.Leg].SetTrigger("doStandUp");
    }

    public void bossSitDown()
    {
        bossAnims[(int)EBossAnimator.Body].SetTrigger("doSitDown");
        bossAnims[(int)EBossAnimator.Leg].SetTrigger("doSitDown");
    }

    public void ResetBoss()
    {
        bossAnims[(int)EBossAnimator.Body].Play("BodySit");
        bossAnims[(int)EBossAnimator.Leg].Play("LegSit");
    }

    public void OpenMissileDoor()
    {
        bossAnims[(int)EBossAnimator.Missile_Door].SetTrigger("doOpen");
    }

    public void CloseMissileDoor()
    {
        bossAnims[(int)EBossAnimator.Missile_Door].SetTrigger("doClose");
    }

    public void OpenBodyUnder()
    {
        bossAnims[(int)EBossAnimator.Body_Under].SetTrigger("doOpen");
    }

    public void OpenRadar()
    {
        bossAnims[(int)EBossAnimator.Radar].SetTrigger("doOpen");
    }

    public void CloseRadar()
    {
        bossAnims[(int)EBossAnimator.Radar].SetTrigger("doClose");
    }

    public void ReloadTimeBomb()
    {
        bossAnims[(int)EBossAnimator.TimeBomb_Cannon].SetTrigger("doReload");
    }

    public void OpenRedzoneCannon()
    {
        bossAnims[(int)EBossAnimator.Redzone_Cannon].SetTrigger("doOpen");
    }

    public void CloseRedzoneCannon()
    {
        bossAnims[(int)EBossAnimator.Redzone_Cannon].SetTrigger("doClose");
    }

    public void OpenBigMissileDoor()
    {
        bossAnims[(int)EBossAnimator.Big_Missile_Door].SetTrigger("doOpen");
    }


    [SerializeField]
    private Animator[] bossAnims = null;
}
