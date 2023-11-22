using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public void Init(Transform _playerTr,PlayerData _playerData, VoidBoolDelegate _actionFinishCallback)
    {
        cam = GetComponent<Camera>();
        //mainCam = GetComponentInChildren<CameraMovement>();
        actionCam = GetComponentInChildren<ActionCamera>();

        mainCam.Init(_playerTr, _playerData);
        actionCam.Init(_actionFinishCallback);
    }

    public void CameraAction(int _curPhaseNum)
    {
        actionCam.StartAction(_curPhaseNum);
    }

    private IEnumerator ChangeFOV(Camera camera, float targetFOV, float duration)
    {
        float startFOV = camera.fieldOfView;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            camera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        camera.fieldOfView = targetFOV;
    }
    [SerializeField]
    private CameraMovement mainCam = null;
    private Camera cam = null;
    private ActionCamera actionCam = null;
}
