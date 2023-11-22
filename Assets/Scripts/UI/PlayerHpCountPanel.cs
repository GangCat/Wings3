using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHpCountPanel : MonoBehaviour
{
    public void Init()
    {
        imageProgress = GetComponentInChildren<ImageProgressbar>();
        imageProgress.Init();
    }

    public void UpdateHp(float _curHpRatio)
    {
        imageProgress.UpdateLength(_curHpRatio);
        StartShaking(1, 5f);
    }

    IEnumerator ShakeRectTransform( float _duration, float _magnitude)
    {
        RectTransform rectTr = GetComponent<RectTransform>();
        Vector2 originalPosition = rectTr.anchoredPosition;
        float elapsed = 0.0f;

        while (elapsed < _duration)
        {
            float x = Random.Range(-1f, 1f) * _magnitude;
            float y = Random.Range(-1f, 1f) * _magnitude;

            rectTr.anchoredPosition = new Vector2(originalPosition.x + x, originalPosition.y + y);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        rectTr.anchoredPosition = originalPosition;
    }

    public void StartShaking(float duration, float magnitude)
    {
        StartCoroutine(ShakeRectTransform(duration, magnitude));
    }

    private ImageProgressbar imageProgress = null;
}
