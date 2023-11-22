using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageProgressbar : MonoBehaviour
{
    public void Init()
    {
        currentRatio = 1f;
    }

    /// <summary>
    /// 0.3 전달시 30%만큼 채워지는 방식
    /// </summary>
    /// <param name="_ratio"></param>
    public virtual void UpdateLength(float _ratio)
    {
        StartCoroutine(UpdateLerpLength(_ratio));
    }

    protected IEnumerator UpdateLerpLength(float _target)
    {
        float initialLength = currentRatio;
        float timer = 0.0f;

        while (timer <= transitionDuration)
        {
            timer += Time.deltaTime;
            myImage.fillAmount = Mathf.Lerp(initialLength, _target, timer / transitionDuration);
            yield return null;
        }

        currentRatio = _target;
    }

    [SerializeField]
    protected Image myImage = null;
    [SerializeField]
    protected float transitionDuration = 0.5f;

    protected float currentRatio = 0f;

}
