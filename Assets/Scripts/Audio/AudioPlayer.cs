using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public virtual void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void PlayAudio(Enum _audioEnum)
    {
        audioIdx = Convert.ToInt32(_audioEnum);

        if (audioIdx >= 0 && audioIdx < myAudioClips.Length)
        {
            Debug.Log(_audioEnum.GetType());
            Debug.Log(audioIdx);
            //audioSource.clip = myAudioClips[audioIndex];
            //audioSource.Play();
        }
        else
        {
            Debug.LogError("Invalid player audio index");
        }
    }

    public virtual void SetVolume(float _volume)
    {
        audioSource.volume = _volume;
    }

    public virtual void StopAudio()
    {
        audioSource.Stop();
    }


    // EPlayerAudio Enum에 대응하는 오디오 클립을 저장할 배열
    [SerializeField]
    protected AudioClip[] myAudioClips;
    // 오디오를 재생할 오디오소스
    protected AudioSource audioSource;
    // 재생할 오디오 클립의 인덱스
    protected int audioIdx = 0;
}
