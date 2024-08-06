using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombinedDoorWinScript : MonoBehaviour
{
    [SerializeField] private GameObject doorEndDog;
    [SerializeField] private GameObject doorEndChef;
    [SerializeField] private Material defaultDogDoor;
    [SerializeField] private Material defaultChefDoor;
    [SerializeField] private Material activeDogDoor;
    [SerializeField] private Material activeChefDoor;
    [SerializeField] private float rotationAngle = 90f; // Angle to rotate the doors
    [SerializeField] private float rotationDuration = 1.0f; // Duration of the rotation
    public bool isFinalLevel = false;
    public bool useSceneIndex = true;
    public int specificSceneIndex;
    public string specificSceneName;

    private Renderer dogDoorRenderer;
    private Renderer chefDoorRenderer;
    private bool dogDoorActivated = false;
    private bool chefDoorActivated = false;
    private bool doorsOpening = false;
    private bool doorsOpened = false; 
    private bool playerInTrigger = false;
    private bool ratInTrigger = false;

    private void Start()
    {
        if (doorEndDog != null)
        {
            dogDoorRenderer = doorEndDog.GetComponent<Renderer>();
            dogDoorRenderer.material = defaultDogDoor;
        }

        if (doorEndChef != null)
        {
            chefDoorRenderer = doorEndChef.GetComponent<Renderer>();
            chefDoorRenderer.material = defaultChefDoor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            chefDoorActivated = true;
            if (chefDoorRenderer != null && activeChefDoor != null)
            {
                chefDoorRenderer.material = activeChefDoor;
            }
            CheckAndOpenDoors();
        }

        if (other.CompareTag("rat"))
        {
            ratInTrigger = true;
            dogDoorActivated = true;
            if (dogDoorRenderer != null && activeDogDoor != null)
            {
                dogDoorRenderer.material = activeDogDoor;
            }
            CheckAndOpenDoors();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            chefDoorActivated = false;
            if (chefDoorRenderer != null && defaultChefDoor != null)
            {
                chefDoorRenderer.material = defaultChefDoor;
            }
        }
        else if (other.CompareTag("rat"))
        {
            ratInTrigger = false;
            dogDoorActivated = false;
            if (dogDoorRenderer != null && defaultDogDoor != null)
            {
                dogDoorRenderer.material = defaultDogDoor;
            }
        }
    }

    private void CheckAndOpenDoors()
    {
        if (dogDoorActivated && chefDoorActivated && !doorsOpening && !doorsOpened)
        {
            StartCoroutine(OpenDoors());
        }
    }

    private IEnumerator OpenDoors()
    {
        doorsOpening = true;

        Quaternion dogDoorInitialRotation = doorEndDog.transform.rotation;
        Quaternion chefDoorInitialRotation = doorEndChef.transform.rotation;
        Quaternion dogDoorTargetRotation = dogDoorInitialRotation * Quaternion.Euler(0, rotationAngle, 0);
        Quaternion chefDoorTargetRotation = chefDoorInitialRotation * Quaternion.Euler(0, -rotationAngle, 0);

        float elapsedTime = 0;

        while (elapsedTime < rotationDuration)
        {
            doorEndDog.transform.rotation = Quaternion.Slerp(dogDoorInitialRotation, dogDoorTargetRotation, elapsedTime / rotationDuration);
            doorEndChef.transform.rotation = Quaternion.Slerp(chefDoorInitialRotation, chefDoorTargetRotation, elapsedTime / rotationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorEndDog.transform.rotation = dogDoorTargetRotation;
        doorEndChef.transform.rotation = chefDoorTargetRotation;

        doorsOpening = false;
        doorsOpened = true; // Set flag to ensure doors only open once
        CheckWinCondition();
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
