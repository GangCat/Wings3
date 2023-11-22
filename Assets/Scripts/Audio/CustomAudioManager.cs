using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips;
    [SerializeField, Range(0,100)]
    private float[] volumes;
    private AudioSource audioSource;


    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudio(int _audioIdx, bool _isLoop = false)
    {
       // audioSource.Stop(); // 나중에 차이함 보기
       // audioSource.clip = audioClips[_audioIdx];
       // audioSource.volume = Mathf.InverseLerp(0, 100, volumes[_audioIdx]);
       // audioSource.loop = _isLoop;
       // audioSource.Play();
    }
    public void StopAllAudio()
    {
        //audioSource.Stop();
    }
    public bool IsPlaying()
    {
        //return audioSource.isPlaying;
        return false;
    }
}
