using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPatternController : MonoBehaviour
{ 
    public void Init(VoidVoidDelegate _patternFinishCallback)
    {
        lastPatternCollider.Init(_patternFinishCallback);
    }

    public void StartPattern()
    {
        lastPatternCollider.Enable(true);
        pullTrigger.Init();
        destroytrigger.Init();
    }


    [SerializeField]
    private LastPatternCollider lastPatternCollider = null;
    [SerializeField]
    private TempPull pullTrigger = null;
    [SerializeField]
    private TempDestroyTrigger destroytrigger = null;
}
