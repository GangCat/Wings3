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

        // 설정된 개수만큼 오디오 소스를 추가해주고 최초 설정해주는 함수
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
    /// 실행된 순서가 가장 처음인 오디오 소스의 인덱스를 찾는 함수
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

    // 현재 사용할 오디오 소스의 인덱스
    private int curAudioSourceIdx = -1;
    // 사용가능한 오디오 소스들의 배열
    private AudioSource[] arrAudioSources = null;
    // 각 오디오 소스들이 가장 최근에 사용되었던 시점
    private float[] arrAudioStartTime = null;

    // 오디오 소스의 개수
    [SerializeField]
    private int audioCount = 0;
}
