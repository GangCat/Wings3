using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class WindBlowActionSecondNode : ActionNode
{
    [SerializeField]
    private float totalDuration = 5f;
    [SerializeField]
    private GameObject windCylinderPrefab;

    private WindBlowPoint[] windBlowPoints = null;
    private float finishTime = 0f;

    private SoundManager soundManager = null;

    protected override void OnStart() {
        soundManager = SoundManager.Instance;
        soundManager.Init(context.windBlowSoundSpawnGO);

        windBlowPoints = context.bossCtrl.CurSpawnPoints[blackboard.curClosedWeakPoint].GetWindBlowHolder().WindBlowPoints;
        foreach (WindBlowPoint wbp in windBlowPoints)
            wbp.StartGenerateSecond(windCylinderPrefab);
        //�ٶ� �Ҹ� ����(����)
        soundManager.PlayAudio(context.windBlowSoundSpawnGO.GetComponent<AudioSource>(), (int)SoundManager.ESounds.BOSSTORNADOSOUND, true);

        finishTime = Time.time + totalDuration;
    }

    protected override void OnStop() {
        if (windBlowPoints != null)
        {
            if (soundManager.IsPlaying(context.windBlowSoundSpawnGO.GetComponent<AudioSource>()))
            {
                soundManager.StopAudio(context.windBlowSoundSpawnGO.GetComponent<AudioSource>());
            }

            //���� �� �˻� �ѹ��ϰ� �ξƴϸ� ���� ����
            foreach (WindBlowPoint wbp in windBlowPoints)
                wbp.FinishGenerate();
        }
    }

    protected override State OnUpdate() {
        if (blackboard.isPhaseEnd || Time.time >= finishTime)
        {
            return State.Success;
        }
        return State.Running;
    }
}
