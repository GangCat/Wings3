using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasPauseMenu : MonoBehaviour
{
    private GameObject pauseMenu;
    private VoidVoidDelegate ResumeCallback;
    private VoidFloatDelegate ChangeVolumeCallback;

    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private Slider sensitiveSlider;
    [SerializeField]
    private Slider freeLookSensitiveSlider;
    [SerializeField]
    private VirtualMouse vm;
    [SerializeField]
    private CameraMovement cameraMove;
    [SerializeField]
    private TextMeshProUGUI sensitiveText;
    [SerializeField]
    private TextMeshProUGUI freeSensitiveText;
    [SerializeField]
    private TextMeshProUGUI soundText;


    public void Init(VoidVoidDelegate _resumeCallback)
    {
        ResumeCallback = _resumeCallback;
        pauseMenu = GetComponentInChildren<Image>().gameObject;
        sensitiveSlider.value = vm.sensitive;

        float slidertextValue = Mathf.Round(Map(sensitiveSlider.value, sensitiveSlider.minValue, sensitiveSlider.maxValue, 0f, 100f));
        sensitiveText.text = slidertextValue.ToString();

        freeLookSensitiveSlider.value = cameraMove.freeLockSensitive/100f;

        slidertextValue = Mathf.Round(Map(freeLookSensitiveSlider.value, freeLookSensitiveSlider.minValue, freeLookSensitiveSlider.maxValue, 0f, 100f));
        freeSensitiveText.text = slidertextValue.ToString();

        //volumeSlider.onValueChanged.AddListener(delegate { ChangeVolumeCallback(volumeSlider.value); });
        sensitiveSlider.onValueChanged.AddListener(delegate { ChangeSensitive(); });
        freeLookSensitiveSlider.onValueChanged.AddListener(delegate { ChangeFreeLookSensitive(); });
        SetPauseMenu(false);
    }

    public void SetPauseMenu(bool _bool)
    {
        pauseMenu.SetActive(_bool);
    }

    public void ResumeGame()
    {
        ResumeCallback();
    }

    public void RetryGame()
    {
        ResumeCallback();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ExitGame()
    {
        ResumeCallback();
        SceneManager.LoadScene("CampusScene");
    }

    public void ChangeSensitive()
    {
        vm.sensitive = sensitiveSlider.value;
        float slidertextValue = Mathf.Round(Map(sensitiveSlider.value, sensitiveSlider.minValue, sensitiveSlider.maxValue, 0f, 100f));
        sensitiveText.text = slidertextValue.ToString();
    }

    private void ChangeFreeLookSensitive()
    {
        cameraMove.freeLockSensitive = freeLookSensitiveSlider.value * 100f;
        float slidertextValue = Mathf.Round(Map(freeLookSensitiveSlider.value, freeLookSensitiveSlider.minValue, freeLookSensitiveSlider.maxValue, 0f, 100f));
        freeSensitiveText.text = slidertextValue.ToString();
    }


    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
