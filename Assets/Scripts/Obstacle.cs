using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public void Init()
    {
        spawnPoint = GetComponentInChildren<BossShieldGeneratorSpawnPoint>();
    }

    public BossShieldGeneratorSpawnPoint SpawnPoint => spawnPoint;

    private BossShieldGeneratorSpawnPoint spawnPoint = null;
}
