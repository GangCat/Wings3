using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMissileMemoryPool : MonoBehaviour
{
    public void Init()
    {
        memoryPool = new MemoryPool(groupMissilePrefab, 32, transform);
    }

    public GameObject ActivateGroupMissile()
    {
        GameObject groupMissileGo = memoryPool.ActivatePoolItem(8, transform);
        return groupMissileGo;
    }

    public void DeactivateGroupMissile(GameObject _deactivateGo)
    {
        memoryPool.DeactivatePoolItem(_deactivateGo);
    }
    private MemoryPool memoryPool;
    [SerializeField]
    private GameObject groupMissilePrefab = null;
}
