using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System.Buffers;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;

public class CannonRainActionNode : ActionNode
{
    [SerializeField]
    private float bossOffSet = 300;
    [SerializeField]
    private Vector2 mapRadious = new Vector2(1500,1500);
    [SerializeField]
    private float attackMinHeight = 10000;
    [SerializeField]
    private float attackMaxHeight = 15000;
    [SerializeField]
    private int cannonBallCnt = 20;
    [SerializeField]
    private GameObject cannonBallPrefab = null;
    [SerializeField]
    private float cannonBallSpeed = 2000;
    [SerializeField]
    private GameObject attackAreaPrefab = null;
    [SerializeField]
    private float patternFinishTime = 10f;

    private Vector3 rndAttackPos;
    private Vector2 rnd1;
    private float startTime = 0f;
    private float lastSoundPlayTime = 0f;
    private int launchSoundPlayTime = 0;

    private SoundManager soundManager = null;
    private enum ECannonAudio
    {
        NONE = -1,
        CANNONBALLMULTYFIRESOUND
    }
    protected override void OnStart()
    {
        launchSoundPlayTime = 0;
        startTime = Time.time;
        lastSoundPlayTime = Time.time;
        soundManager = SoundManager.Instance;
        //context.cannonAudioManager.Init();
        //context.cannonAudioManager.PlayAudio((int)ECannonAudio.CANNONBALLMULTYFIRESOUND);
        //SoundManager.PlayAudio((int)SoundManager.ESounds.CANNONLAUNCHSOUND);
        for (int i = 0; i < context.cannonSoundSpawnGOs.Length; ++i)
        {
            soundManager.AddAudioComponent(context.cannonSoundSpawnGOs[i]);
        }
        

        context.anim.OpenRadar();

        for (int i = 0; i < cannonBallCnt; ++i)
        {
            rnd1.x = Random.Range(-1.0f, 1.0f) * bossOffSet;
            rnd1.y = Random.Range(-1.0f, 1.0f) * bossOffSet;
            if (rnd1.x < 0) mapRadious.x *= -1;
            if (rnd1.y < 0) mapRadious.y *= -1;

            rndAttackPos = new Vector3(Random.Range(rnd1.x, mapRadious.x), 0, Random.Range(rnd1.y, mapRadious.y));
            Vector3 spawnPositionWithHeight = rndAttackPos + new Vector3(0, Random.Range(attackMinHeight, attackMaxHeight), 0);
            GameObject bullet = context.cannonRainMemoryPool.ActivateCannonBall();
            bullet.GetComponent<CannonRainBallController>().Init(cannonBallSpeed, spawnPositionWithHeight, context.cannonRainMemoryPool, attackAreaPrefab, context.playerTr);
            //플레이어와의 거리 계산 > 가까울 수록 볼륨은 커진다 > 캐논을 발사하는 소리
            //context.cannonAudioManager.Init();S
            
            
        }
    }

    protected override void OnStop()
    {
        context.anim.CloseRadar();
    }

    protected override State OnUpdate()
    {

        if (Time.time -lastSoundPlayTime  > 0.3 && launchSoundPlayTime < cannonBallCnt) CannonLaunchSoundPlay();
        if (Time.time - startTime > patternFinishTime)
        return State.Success;

        return context.cannonRainMemoryPool.IsActiveItemEmpty() ? State.Success : State.Running;
    }
    private void CannonLaunchSoundPlay()
    {
        int j = launchSoundPlayTime % context.cannonSoundSpawnGOs.Length;
        soundManager.PlayAudio(context.cannonSoundSpawnGOs[j].GetComponent<AudioSource>(), (int)SoundManager.ESounds.CANNONLAUNCHSOUND);
        lastSoundPlayTime = Time.time;
        ++launchSoundPlayTime;
    }
}
