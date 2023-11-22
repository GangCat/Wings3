using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRoundMove : MonoBehaviour
{
    void Start()
    {
        DrawProjectilePath();
    }

    void DrawProjectilePath()
    {
        int numPoints = 100; // ��θ� ��Ÿ���� ���� ���� ��
        lineRenderer.positionCount = numPoints;

        float radianAngle = launchAngle * Mathf.Deg2Rad; // ������ �������� ��ȯ
        float totalTime = (2f * initialSpeed * Mathf.Sin(radianAngle)) / Physics.gravity.magnitude; // �� �ð� ���

        for (int i = 0; i < numPoints; i++)
        {
            float time = i * (totalTime / numPoints); // �� �������� �ð� ���

            // ������ � ����� x, y ��ǥ ���
            float x = initialPosition.x + initialSpeed * Mathf.Cos(radianAngle) * time;
            float y = initialPosition.y + initialSpeed * Mathf.Sin(radianAngle) * time - 0.5f * Physics.gravity.magnitude * Mathf.Pow(time, 2);

            // ���� ��ǥ�� LineRenderer�� ����
            Vector3 pointPosition = new Vector3(x, y, initialPosition.z);
            lineRenderer.SetPosition(i, pointPosition);
        }
    }

    void OnDrawGizmos()
    {
        float radianAngle = launchAngle * Mathf.Deg2Rad; // ������ �������� ��ȯ

        // ������ � ���� ������ �ӵ� ���
        float horizontalSpeed = initialSpeed * Mathf.Cos(radianAngle);

        // ������ � ���� ������ �ӵ� ���
        float verticalSpeed = initialSpeed * Mathf.Sin(radianAngle) - Physics.gravity.magnitude * Time.fixedDeltaTime;

        Gizmos.color = Color.red;

        // ����� �� ���� �׸��� ���� ���
        Vector3 previousDrawPoint = initialPosition;
        int numPoints = 50;
        for (int i = 1; i <= numPoints; i++)
        {
            float time = i * (horizontalSpeed / Physics.gravity.magnitude) * 2f;
            float x = initialPosition.x + horizontalSpeed * time;
            float y = initialPosition.y + verticalSpeed * time - 0.5f * Physics.gravity.magnitude * Mathf.Pow(time, 2);
            Vector3 currentDrawPoint = new Vector3(x, y, initialPosition.z);
            Gizmos.DrawLine(previousDrawPoint, currentDrawPoint);
            previousDrawPoint = currentDrawPoint;
        }
    }

    public LineRenderer lineRenderer;
    public float initialSpeed = 10f; // �ʱ� �ӵ�
    public float launchAngle = 45f;   // ���� ���� (�� ����)
    public Vector3 initialPosition;   // �ʱ� ��ġ
}
