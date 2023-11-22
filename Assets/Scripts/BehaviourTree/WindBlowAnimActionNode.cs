using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class WindBlowAnimActionNode : ActionNode
{
    [SerializeField]
    private float animTime = 0f;
    [SerializeField]
    private bool isWindBlow = false;

    private float finishTime = 0f;

    private SoundManager soundManager = null;
    protected override void OnStart() {
        soundManager = SoundManager.Instance;
        soundManager.AddAudioComponent(context.windBlowSoundSpawnGO);
        soundManager.PlayAudio(context.windBlowSoundSpawnGO.GetComponent<AudioSource>(), (int)SoundManager.ESounds.BOSSTORNADOSOUND, true);
        //context.anim.SetBool("isWindBlowStart", isWindBlow);
        Debug.Log("start");
        finishTime = Time.time + animTime;
    }

    protected override void OnStop() {
        if (soundManager)
        {
            if (soundManager.IsPlaying(context.windBlowSoundSpawnGO.GetComponent<AudioSource>()))
            {
                soundManager.StopAudio(context.windBlowSoundSpawnGO.GetComponent<AudioSource>());
            }
        }
    }

    protected override State OnUpdate() {
        return Time.time < finishTime ? State.Running : State.Success;
    }
}
