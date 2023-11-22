using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAudioPlayer : AudioPlayer
{
    public override void Init()
    {
        arrAudioSources = new AudioSource[audioCount];
        arrAudioStartTime = new float[audioCount];

        // ������ ������ŭ ����� �ҽ��� �߰����ְ� ���� �������ִ� �Լ�
        for (int i = 0; i < audioCount; ++i)
        {
            arrAudioSources[i] = gameObject.AddComponent<AudioSource>();
            arrAudioSources[i].playOnAwake = false;
            arrAudioStartTime[i] = 0f;
        }
    }

    public override void PlayAudio(Enum _audioEnum)
    {
        audioIdx = Convert.ToInt32(_audioEnum);

        GetFirstStartedAudioSourceIdx();

        if (audioIdx >= 0 && audioIdx < myAudioClips.Length)
        {
            Debug.Log(_audioEnum.GetType());
            Debug.Log("curAudioSource: " + curAudioSourceIdx);
            Debug.Log("audioIdx: " + audioIdx);
            arrAudioStartTime[curAudioSourceIdx] = Time.time;
            //arrAudioSources[curAudioSourceIdx].clip = myAudioClips[audioIdx];
            //arrAudioSources[curAudioSourceIdx].Play();
        }
        else
        {
            Debug.LogError("Invalid player audio index");
        }
    }

    public override void SetVolume(float _volume)
    {
        for (int i = 0; i < audioCount; ++i)
            arrAudioSources[i].volume = _volume;
    }

    public override void StopAudio()
    {
        for (int i = 0; i < audioCount; ++i)
            arrAudioSources[i].Stop();
    }

    /// <summary>
    /// ����� ������ ���� ó���� ����� �ҽ��� �ε����� ã�� �Լ�
    /// </summary>
    private void GetFirstStartedAudioSourceIdx()
    {
        curAudioSourceIdx = 0;

        for (int i = 0; i < audioCount; ++i)
        {
            if (!arrAudioSources[i].isPlaying)
            {
                curAudioSourceIdx = i;
                return;
            }
        }

        for (int i = 0; i < audioCount - 1; ++i)
        {
            if (arrAudioStartTime[i] > arrAudioStartTime[i + 1])
                curAudioSourceIdx = i + 1;
        }
    }

    // ���� ����� ����� �ҽ��� �ε���
    private int curAudioSourceIdx = -1;
    // ��밡���� ����� �ҽ����� �迭
    private AudioSource[] arrAudioSources = null;
    // �� ����� �ҽ����� ���� �ֱٿ� ���Ǿ��� ����
    private float[] arrAudioStartTime = null;

    // ����� �ҽ��� ����
    [SerializeField]
    private int audioCount = 0;
}
