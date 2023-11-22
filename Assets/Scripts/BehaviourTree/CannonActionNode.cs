using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CannonActionNode : ActionNode
{
    [SerializeField]
    private float radious = 100;
    [SerializeField]
    private float bossOffSet = 500;
    [SerializeField]
    private Vector2 mapRadious = new Vector2(5000, 5000);
    [SerializeField]
    private float duration = 9;
    [SerializeField]
    private float attackMinHeight = 1000;
    [SerializeField]
    private float attackMaxHeight = 1500;
    [SerializeField]
    private float term = 3;
    [SerializeField]
    private int cannonBallCnt = 80;
    [SerializeField]
    private GameObject cannonBallPrefab = null;
    [SerializeField]
    private float cannonBallSpeed = 1000;
    [SerializeField]
    private GameObject attackAreaPrefab = null;

    private float startTime;
    private float lastAttackTime;
    private Vector3 rndAttackPos;
    private Vector2 rnd1;

    protected override void OnStart() {
        context.anim.OpenRedzoneCannon();
        startTime = Time.time;
        lastAttackTime = 0;


    }

    protected override void OnStop() {
        context.anim.CloseRedzoneCannon();
    }

    protected override State OnUpdate() {
        rnd1.x = Random.Range(-1.0f, 1.0f) * bossOffSet;
        rnd1.y = Random.Range(-1.0f, 1.0f) * bossOffSet;
        if (rnd1.x < 0) mapRadious.x *= -1;
        if (rnd1.y < 0) mapRadious.y *= -1;

        rndAttackPos = new Vector3(Random.Range(rnd1.x, mapRadious.x),0 , Random.Range(rnd1.y, mapRadious.y)); //00나중에 예외처리 추후 duration이랑 term차이 확인, running을 반환하지 않으면?


        if (Time.time - startTime <= duration)
        {
            if (Time.time - lastAttackTime > term)
            {
                Destroy(Instantiate(attackAreaPrefab, new Vector3(rndAttackPos.x,60f,rndAttackPos.z), Quaternion.identity), 6f);
                
                for (int i=0; i < cannonBallCnt; ++i)
                {
                    //플레이어와의 거리계산 > 가까울 수록 소리 증폭 > 폭탄 발사 소리 재생
                    Vector2 rnd2 = Random.insideUnitCircle * radious;
                    Vector3 spawnPositionWithHeight = rndAttackPos + new Vector3(rnd2.x, Random.Range(attackMinHeight, attackMaxHeight), rnd2.y);
                    GameObject cannonBall = context.cannonMemoryPool.ActivateCannonBall();
                    cannonBall.GetComponent<CannonBallController>().Init(cannonBallSpeed, spawnPositionWithHeight,context.cannonMemoryPool);
                    lastAttackTime = Time.time;
                }
            }
            return State.Running;
        }

        return State.Success;

        
    }
}
