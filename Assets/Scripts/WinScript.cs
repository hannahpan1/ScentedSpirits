using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    public bool isFinalLevel = false;
    public bool useSceneIndex = true;
    public int specificSceneIndex;
    public string specificSceneName;

    private bool playerInTrigger = false;
    private bool ratInTrigger = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = true;
            CheckWinCondition();
        }
        else if (other.gameObject.CompareTag("rat"))
        {
            ratInTrigger = true;
            CheckWinCondition();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
        else if (other.gameObject.CompareTag("rat"))
        {
            ratInTrigger = false;
        }
    }

    private void CheckWinCondition()
    {
        if (playerInTrigger && ratInTrigger)
        {
            if (isFinalLevel)
            {
                SceneManager.LoadScene("WinScreen");
            }
            else
            {
                if (useSceneIndex)
                {
                    // Load the next scene in the build index
                    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                    {
                        SceneManager.LoadScene(nextSceneIndex);
                    }
                    else
                    {
                        Debug.LogWarning("Next scene index is out of bounds. Check your build settings.");
                    }
                }
                else
                {
                    // Load the specific scene
                    SceneManager.LoadScene(specificSceneName);
                }
            }
        }
    }
}
