using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXMemoryPool : MonoBehaviour
{
    public void Init(Transform _playerTr)
    {
        playerTr = _playerTr;
        vfxMemoryPool = new MemoryPool(vfxGoPrefab, 50, transform);
    }

    public GameObject ActivatePoolItem()
    {
        return vfxMemoryPool.ActivatePoolItem(5, transform);
    }

    public void DeActivateItem(GameObject _deactivateItem)
    {
        vfxMemoryPool.DeactivatePoolItem(_deactivateItem);
    }


    [SerializeField]
    private GameObject vfxGoPrefab = null;

    private MemoryPool vfxMemoryPool = null;
    private Transform playerTr = null;
}
