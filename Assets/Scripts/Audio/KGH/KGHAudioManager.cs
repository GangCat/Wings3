using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class KGHAudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSound, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            //Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
           // Destory(gamgeObject);
        }
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, x=> x.name == name);
        if( s== null)
        {
            Debug.Log("Sound Nor Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }

    }
    public void PlaySFX(String name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if(s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
