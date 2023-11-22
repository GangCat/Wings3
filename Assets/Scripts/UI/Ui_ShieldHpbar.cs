using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_ShieldHpbar : MonoBehaviour
{
    [SerializeField]
    private Image hpBar = null;
    [SerializeField]
    private RectTransform uiTransform = null;
    [SerializeField]
    private float initialSpeed = 500f;
    [SerializeField]
    private float acceleration = 500f;
    [SerializeField]
    private Vector2 targetPosition = Vector2.zero;
    [SerializeField]
    private BossShieldGenerator shieldGenerator = null;
    private Vector2 beforePos = Vector2.zero;
    private RectTransform rectTr = null;
    private bool isMoving = false;
    private bool isTimerOn = false;

    void Start()
    {
        rectTr = GetComponent<RectTransform>();
        uiHp = shieldGenerator.GetCurHp;
        curHp = shieldGenerator.GetCurHp;
        beforePos = uiTransform.anchoredPosition;
    }

    private void Update()
    {


        //// 카메라를 바라보도록 UI를 조정합니다.
        //if (Camera.main != null)
        //{
        //    rectTr.LookAt(Camera.main.transform);
        //}
    }

    public void Damaged()
    {
        if (shieldGenerator != null)
        {
            float shieldHp = shieldGenerator.GetCurHp;

            if (curHp != shieldHp)
            {
                curHp = shieldHp;
                uiHp = Mathf.Clamp01(shieldHp / 100);
                if (isTimerOn)
                {
                    StopAllCoroutines();
                    StartCoroutine(TimerThreeSecondsCorotuine());
                }
                else if(!isMoving)
                    MoveRight();

                if (hpBar != null)
                {
                    hpBar.fillAmount = uiHp;
                }
            }
            if (curHp <= 0)
            {
                ForceMoveLeft();
            }
        }

    }

    private void MoveRight()
    {
        if (isMoving)
            return;

        StopAllCoroutines();
        StartCoroutine(MoveRightCoroutine());
    }
    private void MoveLeft()
    {
        if (isMoving)
            return;

        StopAllCoroutines();
        StartCoroutine(MoveLeftCoroutine());
    }

    private void ForceMoveLeft()
    {
        StopAllCoroutines();
        StartCoroutine(MoveLeftCoroutine());
    }

    private IEnumerator MoveRightCoroutine()
    {
        isMoving = true;
        float x = targetPosition.x;
        while (uiTransform.anchoredPosition.x - x < 0.1f)
        {
            uiTransform.anchoredPosition += Vector2.right * initialSpeed * Time.deltaTime;
            yield return null; 
        }
        uiTransform.anchoredPosition = targetPosition;
        isMoving = false;

        StartCoroutine(TimerThreeSecondsCorotuine());
    }

    private IEnumerator MoveLeftCoroutine()
    {
        isMoving = true;
        float x = beforePos.x;
        while (uiTransform.anchoredPosition.x - x < 0.1f)
        {
            uiTransform.anchoredPosition -= Vector2.right * initialSpeed * Time.deltaTime;
            yield return null;
        }
        uiTransform.anchoredPosition = beforePos;
        isMoving = false;
    }

    private IEnumerator TimerThreeSecondsCorotuine()
    {
        isTimerOn = true;
        yield return new WaitForSeconds(3f);

        isTimerOn = false;
        MoveLeft();
    }

    private float uiHp = 0f;
    private float curHp = 0f;
}

