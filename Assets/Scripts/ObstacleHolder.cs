using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHolder : MonoBehaviour
{

    public void Init()
    {
        arrObstacles = GetComponentsInChildren<Obstacle>();

        foreach (Obstacle ob in arrObstacles)
            ob.Init();
    }

    public BossShieldGeneratorSpawnPoint GetRandomSpawnPoint()
    {
        return arrObstacles[Random.Range(0, arrObstacles.Length)].SpawnPoint;
    }

    private Obstacle[] arrObstacles = null;
}
