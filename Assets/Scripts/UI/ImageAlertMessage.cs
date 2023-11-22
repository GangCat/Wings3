using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageAlertMessage : MonoBehaviour
{
    public void Init()
    {
        soundManager = SoundManager.Instance;
        soundManager.Init(gameObject);
        textAlert = GetComponentInChildren<TextAlertMessage>();
        textAlert.Init();
    }

    public void AlertDanger(int _idx)
    {
        //경고 사운드 재생
        soundManager.PlayAudio(GetComponent<AudioSource>(),(int)SoundManager.ESounds.SCREENWARNINGSOUND);
        textAlert.AlertDanger(alertMessage[_idx]);
    }


    [SerializeField]
    [TextArea]
    private string[] alertMessage = null;

    private TextAlertMessage textAlert = null;

    private SoundManager soundManager = null;
}
