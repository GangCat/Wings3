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


    // EPlayerAudio Enum�� �����ϴ� ����� Ŭ���� ������ �迭
    [SerializeField]
    protected AudioClip[] myAudioClips;
    // ������� ����� ������ҽ�
    protected AudioSource audioSource;
    // ����� ����� Ŭ���� �ε���
    protected int audioIdx = 0;
}
