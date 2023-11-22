using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMemoryPool : MonoBehaviour
{
    //public void SpawnProjectile(Vector3 _pos, Quaternion _quaternion, float _dmg, RetVoidRaramIntDelegate _callback = null)
    //{
    //    GameObject projectileGo = memoryPool.ActivatePoolItem();
    //    projectileGo.SetActive(false);
    //    projectileGo.transform.position = _pos;
    //    projectileGo.transform.rotation = _quaternion;
    //    projectileGo.GetComponent<ProjectileController>().Setup(memoryPool, _dmg, impactMemoryPool, _callback);
    //    projectileGo.SetActive(true);
    //}

    private void Start()
    {
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        memoryPool = new MemoryPool(ProjectilePrefab, increaseCnt);
    }

    [SerializeField]
    private GameObject ProjectilePrefab;
    [SerializeField]
    private int increaseCnt = 5;

    private MemoryPool memoryPool;
    private ImpactMemoryPool impactMemoryPool;
}