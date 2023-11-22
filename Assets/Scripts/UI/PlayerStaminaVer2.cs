using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaVer2 : MonoBehaviour
{
    [SerializeField]
    private Image[] arrImageSp = null;


    private int maxSp = 3;  // �ִ� ���׹̳� ��
    private int curSp = 3;  // ���� ���׹̳� ��

    private void Start()
    {
        if (arrImageSp == null || arrImageSp.Length != maxSp)
        {
            Debug.LogError("�̹��� �迭�� null�̰ų� ���ϴ� ũ��� ���� �ʽ��ϴ�.");
            return;
        }

        Init();
    }

    public void Init()
    {
        curSp = maxSp;
        UpdateSpUI();
    }

    public void DecreaseSp()
    {
        if (curSp > 0)
        {
            curSp--;
            UpdateSpUI();
        }
    }

    public void IncreaseSp()
    {
        if (curSp < maxSp)
        {
            curSp++;
            UpdateSpUI();
        }
    }

    public void UpdateSpChecker(int _stamina)
    {
        if (_stamina > maxSp)
        {
            _stamina = maxSp;
        }

        if (curSp > _stamina)
        {
            curSp = _stamina;
            UpdateSpUI();
        }
        else if (curSp < _stamina)
        {
            curSp = _stamina;
            UpdateSpUI();
        }
    }

    private void UpdateSpUI()
    {
        for (int i = 0; i < maxSp; i++)
        {
            if (i < curSp)
            {
                arrImageSp[i].sprite = null;
                arrImageSp[i].color = new Color(arrImageSp[i].color.r, arrImageSp[i].color.g, arrImageSp[i].color.b, 1f);
            }
            else
            {
                arrImageSp[i].sprite = null;
                arrImageSp[i].color = new Color(arrImageSp[i].color.r, arrImageSp[i].color.g, arrImageSp[i].color.b, 0f);
            }
        }
    }

    public int CurSp => curSp;
}
