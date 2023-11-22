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
        Cursor.visible = true;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CampusScene");
    }
}
