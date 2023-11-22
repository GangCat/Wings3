using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlowPoint : MonoBehaviour
{
    public Vector3 GetPos => transform.position;
    public Quaternion GetRot => transform.rotation;

    public void Init()
    {
        windGenerator = GetComponent<WindBlowGenerator>();
    }

    public void StartGenerateSecond(GameObject _windCylinderPrefab)
    {
        windGenerator.InitSecond(_windCylinderPrefab);
    }

    public void StartGenerate(float _smallRadius, float _largeRadius, float _totalDuration, float _colliderInterval, float _cylinderLengthPerSecond, int _numVertices, GameObject _windCylinderPrefab)
    {
        windGenerator.Init(_smallRadius, _largeRadius, _totalDuration, _colliderInterval, _cylinderLengthPerSecond, _numVertices, _windCylinderPrefab);
    }

    public void updateWindBlow()
    {
        windGenerator.UpdateWindBlow();
    }

    public void FinishGenerate()
    {
        windGenerator.FinishGenerate();
    }

    private WindBlowGenerator windGenerator = null;
}
