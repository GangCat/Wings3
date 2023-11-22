using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform; // 카메라의 Transform

    Vector3 originalPos; // 카메라 원래 위치

    private CameraShake() { }

    public static CameraShake Instance
    {
        get
        {
            if (instance == null)
                instance = FindAnyObjectByType<CameraShake>();
            return instance;
        }
    }

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
            ShakeCamera(1, 1f);
    }

    public void ShakeCamera(float intensity, float duration)
    {
        originalPos = cameraTransform.localPosition;
        StartCoroutine(Shake(intensity, duration));
    }

    IEnumerator Shake(float intensity, float duration)
    {
        float elapsedTime = 0f;
        Vector3 randomDir;

        while (elapsedTime < duration)
        {
            randomDir = Random.insideUnitSphere * intensity;
            cameraTransform.localPosition = originalPos + randomDir;

            intensity = Mathf.Lerp(intensity, 0f, elapsedTime / duration); // 점차 흔들림 감소

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }

    private static CameraShake instance = null;
}