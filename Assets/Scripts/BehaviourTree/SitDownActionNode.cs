using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SitDownActionNode : ActionNode
{
    [SerializeField]
    private float durationTime = 5f;

    private float curDurationtime = 0f;

    private SoundManager soundManager = null;

    protected override void OnStart()
    {
        soundManager = SoundManager.Instance;
        soundManager.Init(context.sitDownSoundSpawnGO);
        soundManager.PlayAudio(context.sitDownSoundSpawnGO.GetComponent<AudioSource>(), (int)SoundManager.ESounds.BOSSSITDOWNSOUND, true);

        context.anim.bossSitDown();
        context.sitDownGo.SetActive(true);
        //플레이어와의 거리 계산 > 가까울수록 소리 증가 > 보스 기계음 시작
        curDurationtime = durationTime;
    }

    protected override void OnStop()
    {
        if (soundManager)
        {
            if (soundManager.IsPlaying(context.sitDownSoundSpawnGO.GetComponent<AudioSource>()))
            {
                soundManager.StopAudio(context.sitDownSoundSpawnGO.GetComponent<AudioSource>());
            }
        }
        context.sitDownGo.SetActive(false);
        // 보스 기계음 정지
    }

    protected override State OnUpdate()
    {
        curDurationtime -= Time.deltaTime;

        if (curDurationtime <= 0)
        {
            context.anim.BossStandUp();
            // 보스 기계음 정지
            soundManager.PlayAudio(context.sitDownSoundSpawnGO.GetComponent<AudioSource>(), (int)SoundManager.ESounds.BOSSSITDOWNSOUND, false);
            return State.Success;
        }

        return State.Running;
    }

}
