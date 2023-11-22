using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonMemoryPool : MonoBehaviour
{
    public void Init()
    {
        memoryPool = new MemoryPool(cannonBallPrefab, 50, transform);
    }

    public GameObject ActivateCannonBall()
    {
        GameObject cannonBallGo = memoryPool.ActivatePoolItem(5, transform);
        return cannonBallGo;
    }

    public void DeactivateCannonBall(GameObject _deactivateGo)
    {
        memoryPool.DeactivatePoolItem(_deactivateGo);
    }
    private MemoryPool memoryPool;
    [SerializeField]
    private GameObject cannonBallPrefab = null;
}
