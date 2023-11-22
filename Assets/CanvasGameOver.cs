using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasGameOver : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void GameOver()
    {
        gameObject.SetActive(true);
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("CampusScene");
    }

}
