using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public void Init()
    {
        arrObstacleHolders = GetComponentsInChildren<ObstacleHolder>();
        foreach (var ob in arrObstacleHolders)
            ob.Init();
    }

    public BossShieldGeneratorSpawnPoint[] GetRandomSpawnPoint()
    {
        BossShieldGeneratorSpawnPoint[] tempArr = new BossShieldGeneratorSpawnPoint[4];

        for(int i = 0; i < arrObstacleHolders.Length; ++i)
            tempArr[i] = arrObstacleHolders[i].GetRandomSpawnPoint();

        return tempArr;
    }

    private ObstacleHolder[] arrObstacleHolders = null;
}
