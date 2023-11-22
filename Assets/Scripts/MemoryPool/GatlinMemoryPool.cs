using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlinMemoryPool : MonoBehaviour
{
    public void Init()
    {
        memoryPool = new MemoryPool(bulletPrefab, 100, transform);
    }

    public GameObject ActivateBullet()
    {
        GameObject bulletGo = memoryPool.ActivatePoolItem(5, transform);
        return bulletGo;
    }

    public void DeactivateBullet(GameObject _deactivateGo)
    {
        memoryPool.DeactivatePoolItem(_deactivateGo);
    }
    private MemoryPool memoryPool;
    [SerializeField]
    private GameObject bulletPrefab = null;
}
