using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampusPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
            //Debug.Log("Portal");
            SceneManager.LoadScene("GameScene");
    }
}
