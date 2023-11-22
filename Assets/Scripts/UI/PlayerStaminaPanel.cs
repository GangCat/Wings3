using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaPanel : MonoBehaviour
{
    public void Init()
    {
        arrImageSp = GetComponentsInChildren<Image>();
        curSp = 3;
    }

    public void DecreaseSp()
    {
        arrImageSp[curSp-1].sprite = null;
        arrImageSp[curSp-1].color = new Color(arrImageSp[curSp - 1].color.r, arrImageSp[curSp - 1].color.g, arrImageSp[curSp - 1].color.b, 0f);
        curSp--;
    }

    public void IncreaseSp()
    {
        arrImageSp[curSp].sprite = null;
        arrImageSp[curSp].color = new Color(arrImageSp[curSp].color.r, arrImageSp[curSp].color.g, arrImageSp[curSp].color.b, 1f);
       curSp++;
    }

    public void UpdateSpChecker(int _stamina)
    {
        if (curSp > _stamina)
        {
            DecreaseSp();
        } else if ( curSp < _stamina)
        {
            IncreaseSp();
        }
        curSp = _stamina;
    }

    [SerializeField]
    private Image[] arrImageSp = null;
    private int curSp = 0;

    public int CurSp => curSp;
}
