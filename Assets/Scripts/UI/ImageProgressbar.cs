using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageProgressbar : MonoBehaviour
{
    public void Init()
    {
        maxLength = imageBack.GetComponent<RectTransform>().rect.width;
        myRt = GetComponent<RectTransform>();
        myHeight = myRt.rect.height;
        currentLength = maxLength;
    }

    /// <summary>
    /// 0.3 전달시 30%만큼 채워지는 방식
    /// </summary>
    /// <param name="_ratio"></param>
    public virtual void UpdateLength(float _ratio)
    {
        targetLength = maxLength * _ratio;
        StartCoroutine(UpdateLerpLength(targetLength));
        //myRt.sizeDelta = new Vector2(maxLength * _ratio, myHeight);
    }

    private IEnumerator UpdateLerpLength(float _target)
    {
        float initialLength = currentLength;
        float timer = 0.0f;

        while (timer <= transitionDuration)
        {
            timer += Time.deltaTime;
            currentLength = Mathf.Lerp(initialLength, _target, timer / transitionDuration);
            myRt.sizeDelta = new Vector2(currentLength, myHeight);
            yield return null;
        }
        currentLength = targetLength;


    }

    [SerializeField]
    protected Image imageBack = null;

    protected float maxLength = 0f;
    protected float myHeight = 0f;
    protected float currentLength = 0f;
    protected float targetLength = 0f;

    [SerializeField]
    private float transitionDuration = 0.5f;
    protected RectTransform myRt;
}
