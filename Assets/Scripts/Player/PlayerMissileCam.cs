using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMissileCam : MonoBehaviour
{

    private Transform playerTr;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float findDistance = 300f;
    [SerializeField]
    private float minOffset = 300f;
    [SerializeField]
    private float maxOffset = 300f;
    [SerializeField]
    private float maxUpOffset = 6f;
    [SerializeField]
    private float smooth = 0.5f;
    CameraMovement cam;

    private class MissileInfo
    {
        public float lastChangeTime;
        public float targetOffset;
    }
    private Dictionary<Collider, MissileInfo> missileInfoDict = new Dictionary<Collider, MissileInfo>();
    public void Init()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }
    public void Start()
    {
        playerTr = transform;
    }
    private void Update()
    {
        //Collider[] missiles = Physics.OverlapSphere(playerTr.position, 10000f, layerMask);

        //if (missiles != null)
        //{
        //    foreach (Collider missile in missiles)
        //    {
        //        Vector3 missilePosition = missile.transform.position;
        //        float distance = Vector3.Distance(playerTr.position, missilePosition);
        //        Vector3 toMissile = missilePosition - playerTr.position;
        //        float angle = Vector3.Angle(playerTr.transform.forward, toMissile);
        //        if (angle > 90 && angle < 270 && distance < findDistance)
        //        {
        //            float targetOffset = Mathf.Lerp(minOffset, 180, distance / findDistance);
        //            cam.offset = Mathf.Lerp(cam.offset, targetOffset, smooth * Time.deltaTime);
        //            Debug.Log(targetOffset);

        //        }
        //        else
        //        {
        //            cam.offset = Mathf.Lerp(cam.offset, 10f, smooth * Time.deltaTime);
        //        }
        //    }

        //}

        if (Physics.OverlapSphere(playerTr.position, findDistance, layerMask).Length > 0)
        {
            //cam.offset = maxOffset;
            //cam.upOffset = maxUpOffset;

            cam.offset = Mathf.Lerp(cam.offset, maxOffset, 0.5f * Time.fixedDeltaTime);
            cam.upOffset = Mathf.Lerp(cam.upOffset, maxUpOffset, 0.5f * Time.fixedDeltaTime);
        }
        else
        {
            //cam.offset = 10f;
            //cam.upOffset = 3f;
            cam.offset = Mathf.Lerp(cam.offset, 10f, 0.5f * Time.fixedDeltaTime);
            cam.upOffset = Mathf.Lerp(cam.upOffset, 3f, 0.5f * Time.fixedDeltaTime);
        }


        //Collider[] missiles = Physics.OverlapSphere(playerTr.position, 10000f, layerMask);
        //Collider closestMissile = null;
        //float closestDistance = Mathf.Infinity;

        //// 가장 가까운 미사일 찾기
        //foreach (Collider missile in missiles)
        //{
        //    Vector3 missilePosition = missile.transform.position;
        //    float distance = Vector3.Distance(playerTr.position, missilePosition);

        //    if (distance < closestDistance)
        //    {
        //        closestDistance = distance;
        //        closestMissile = missile;
        //    }
        //}

        //// 가장 가까운 미사일에 대해서만 오프셋 조절
        //if (closestMissile != null)
        //{
        //    Vector3 closestMissilePosition = closestMissile.transform.position;
        //    Vector3 toClosestMissile = closestMissilePosition - playerTr.position;
        //    float angle = Vector3.Angle(playerTr.transform.forward, toClosestMissile);

        //    if (angle > 90 && angle < 270 && closestDistance < findDistance)
        //    {
        //        float targetOffset = Mathf.Lerp(minOffset, maxOffset, closestDistance / findDistance);
        //        cam.offset = Mathf.Lerp(cam.offset, targetOffset, 0.4f * Time.deltaTime);
        //        cam.upOffset = Mathf.Lerp(cam.upOffset, maxUpOffset, 0.4f * Time.deltaTime);
        //        Debug.Log(targetOffset);
        //    }
        //    else
        //    {
        //        cam.offset = Mathf.Lerp(cam.offset, 10f, 0.5f * Time.deltaTime);
        //        cam.upOffset = Mathf.Lerp(cam.upOffset, 3f, 0.5f * Time.deltaTime);
        //    }
        //}
        //else
        //{
        //    cam.offset = Mathf.Lerp(cam.offset, 10f, 0.5f * Time.deltaTime);
        //    cam.upOffset = Mathf.Lerp(cam.upOffset, 3f, 0.5f * Time.deltaTime);
        //}
    }
}