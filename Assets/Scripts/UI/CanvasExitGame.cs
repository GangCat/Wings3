using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasExitGame : MonoBehaviour
{
    [SerializeField]
    private GameObject exitUi;

    private void Awake()
    {
        exitUi.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUi();
        }
    }


    public void ToggleUi()
    {
        if (!exitUi.gameObject.activeSelf)
        {
            exitUi.SetActive(true);
        }
        else
        {
            exitUi.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
