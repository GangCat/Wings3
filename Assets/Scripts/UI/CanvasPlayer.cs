using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPlayer : MonoBehaviour
{
    public void Init()
    {
        hpPanel = GetComponentInChildren<PlayerHpCountPanel>();
        spPanel = GetComponentInChildren<PlayerStaminaVer2>();
        hpPanel.Init();
        spPanel.Init();
    }


    public void UpdateSp(int _stamina)
    {
        spPanel.UpdateSpChecker(_stamina);
    }

    public void UpdateHp(float _curHpRatio)
    {
        hpPanel.UpdateHp(_curHpRatio);
    }

    private PlayerHpCountPanel hpPanel;
    private PlayerStaminaVer2 spPanel;
}
