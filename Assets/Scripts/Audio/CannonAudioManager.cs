using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAudioManager : MonoBehaviour
{
    private CustomAudioManager[] customAudioManagers;

    public void Init()
    {
        customAudioManagers = GetComponentsInChildren<CustomAudioManager>();
        for (int i = 0; i < customAudioManagers.Length; ++i)
        {
            customAudioManagers[i].Init();
        }
    }
    public void PlayAudio(int _idx)
    {
        for (int i = 0; i < customAudioManagers.Length; ++i)
        {
            customAudioManagers[i].PlayAudio(_idx);
        }
    }
}
