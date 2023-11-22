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
        int numPoints = 100; // 경로를 나타내기 위한 점의 수
        lineRenderer.positionCount = numPoints;

        float radianAngle = launchAngle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        float totalTime = (2f * initialSpeed * Mathf.Sin(radianAngle)) / Physics.gravity.magnitude; // 총 시간 계산

        for (int i = 0; i < numPoints; i++)
        {
            float time = i * (totalTime / numPoints); // 각 점에서의 시간 계산

            // 포물선 운동 경로의 x, y 좌표 계산
            float x = initialPosition.x + initialSpeed * Mathf.Cos(radianAngle) * time;
            float y = initialPosition.y + initialSpeed * Mathf.Sin(radianAngle) * time - 0.5f * Physics.gravity.magnitude * Mathf.Pow(time, 2);

            // 계산된 좌표를 LineRenderer에 설정
            Vector3 pointPosition = new Vector3(x, y, initialPosition.z);
            lineRenderer.SetPosition(i, pointPosition);
        }
    }

    void OnDrawGizmos()
    {
        float radianAngle = launchAngle * Mathf.Deg2Rad; // 각도를 라디안으로 변환

        // 포물선 운동 수평 방향의 속도 계산
        float horizontalSpeed = initialSpeed * Mathf.Cos(radianAngle);

        // 포물선 운동 수직 방향의 속도 계산
        float verticalSpeed = initialSpeed * Mathf.Sin(radianAngle) - Physics.gravity.magnitude * Time.fixedDeltaTime;

        Gizmos.color = Color.red;

        // 경로의 각 점을 그리기 위해 계산
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
    public float initialSpeed = 10f; // 초기 속도
    public float launchAngle = 45f;   // 던진 각도 (도 단위)
    public Vector3 initialPosition;   // 초기 위치
}
