using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CannonRainAttackAreaController : MonoBehaviour
{
    [SerializeField]
    private float duration = 4;

    private float min;
    private float max;
    private float startTime;

    private void Start()
    {
        max = 130f;
        min = 15f;
        startTime = Time.time;
    }

    private void Update()
    {
        float t = (Time.time - startTime) / duration; // �ùٸ� �ð� ���

        // Mathf.Lerp�� ����Ͽ� ũ�� ����
        float newScaleXZ = Mathf.Lerp(max, 0, t);
        transform.localScale = new Vector3(newScaleXZ, transform.localScale.y, newScaleXZ);
        if (transform.localScale.x == 0) Destroy(gameObject);
    }
}
