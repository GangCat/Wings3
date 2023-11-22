using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;

public class LaunchMissileGroupActionNode : ActionNode
{
    [SerializeField]
    private GameObject missilePrefab = null;
    [SerializeField]
    private float spawnRate = 1f;

    private GroupHomingMissileSpawnPos[] arrGroupHomingMissileSpawnPos = null;
    private GameObject[] arrMissileGroup = new GameObject[32];
    private Transform playerTr = null;
    private GroupMissileMemoryPool memoryPool = null;

    private int missileSpawnIdx = 0;
    private float startTime = 0;

    private bool isSpawnFinish = false;


    protected override void OnStart() 
    {
        arrGroupHomingMissileSpawnPos = context.arrGroupHomingMissileSpawnPos;
        startTime = Time.time;
        isSpawnFinish = false;
        missileSpawnIdx = 0;
        playerTr = context.playerTr;
        memoryPool = context.groupMissileMemoryPool;
        context.anim.OpenMissileDoor();

        SpawnMissile();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (!isSpawnFinish && Time.time - startTime > spawnRate)
        {
            SpawnMissile();
            startTime = Time.time;
        }


        for (int i = 0; i < arrMissileGroup.Length; ++i)
        {
            if (arrMissileGroup[i].activeSelf)
            {
                return State.Running;
            }
        }


        Debug.Log("LaunchMissileGroup");
        return State.Success;
    }

    private void SpawnMissile()
    {
        for (int i = missileSpawnIdx; i < arrGroupHomingMissileSpawnPos.Length; i += 8)
        {
            arrMissileGroup[i] = context.groupMissileMemoryPool.ActivateGroupMissile();
            Vector3 spawnPos = arrGroupHomingMissileSpawnPos[i].GetPos();
            Quaternion spawnRot = arrGroupHomingMissileSpawnPos[i].GetRot();
            arrMissileGroup[i].GetComponent<GroupHomingMissile>().Init(playerTr, spawnPos, spawnRot, memoryPool, blackboard.isShieldDestroy);
        }

        ++missileSpawnIdx;

        if (missileSpawnIdx >= 8)
        {
            context.anim.CloseMissileDoor();
            isSpawnFinish = true;
        }
    }
}
