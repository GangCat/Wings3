using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
/// <summary>
/// 1. �÷��̾ ������ ���ö����� ������ �о�� ����
/// 2. ���� �ȿ� ���� ������ �÷��̾� ������ �ߵ�
/// 3. ������ �÷��̾��� ���� ����ؼ� �о
/// ����
/// �÷��̾��� ������ ������ ��� �ؾ��ϴ°� �÷��̾� ��ũ��Ʈ�� �ǵ鿩�� �ϳ�(bool ���?)
/// �� ���� �ſ��� ����ü �÷��̾�� ���� �о�� ���ΰ�
/// �̰Ͷ��� �ٶ� ��Ʈ�ѷ��� ����޾�� �ϴ°�
/// �Ʒ��� ���� ���� ������ context���� ���� ��Ʈ�ѷ����� �����ص� �Ǵ°�
/// </summary>
public class AirPushAcionNode : ActionNode
{
    [SerializeField]
    private float durationTime = 5f;

    private float curDurationTime = 0f;

    private SoundManager soundManager = null;
    private enum EAirPushAudio
    {
        NONE = -1,
        AIRPUSHSOUND
    }
    protected override void OnStart() {
        soundManager = SoundManager.Instance;
        soundManager.Init(context.airPushSoundSpawnGO);
        if (blackboard.curPhaseNum > 1)
        {
            context.airPushGo.SetActive(true);
            soundManager.PlayAudio(context.airPushSoundSpawnGO.GetComponent<AudioSource>(), (int)SoundManager.ESounds.AIRPUSHWINDSOUND,true);
            curDurationTime = durationTime;
        }
    }

    protected override void OnStop() {
        if(blackboard.curPhaseNum > 1)
        {
            context.airPushGo.SetActive(false);
            if (soundManager)
            {
                if (soundManager.IsPlaying(context.airPushSoundSpawnGO.GetComponent<AudioSource>()))
                {
                    soundManager.StopAudio(context.airPushSoundSpawnGO.GetComponent<AudioSource>());
                }
            }
        }
    }

    protected override State OnUpdate() {
        if (blackboard.curPhaseNum < 2)
            return State.Success;

        curDurationTime -= Time.deltaTime;

        if(curDurationTime <= 0)
            return State.Success;
        return State.Running;
    }
}
