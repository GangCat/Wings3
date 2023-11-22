using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasGameClear : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void GameClear()
    {
        gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("CampusScene");
    }

    [SerializeField]
    private Image image = null;
}
