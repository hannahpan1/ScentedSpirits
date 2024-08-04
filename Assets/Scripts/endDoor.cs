using UnityEngine;
using System.Collections;

public class EndDoorMaterialChanger : MonoBehaviour
{
    [SerializeField] private GameObject doorEndDog;
    [SerializeField] private GameObject doorEndChef;
    [SerializeField] private Material activeDogDoor;
    [SerializeField] private Material activeChefDoor;
    [SerializeField] private float rotationAngle = 90f; // Angle to rotate the doors
    [SerializeField] private float rotationDuration = 1.0f; // Duration of the rotation

    private Renderer dogDoorRenderer;
    private Renderer chefDoorRenderer;
    private bool dogDoorActivated = false;
    private bool chefDoorActivated = false;
    private bool doorsOpening = false;
    private bool doorsOpened = false; 

    private void Start()
    {
        if (doorEndDog != null)
        {
            dogDoorRenderer = doorEndDog.GetComponent<Renderer>();
        }

        if (doorEndChef != null)
        {
            chefDoorRenderer = doorEndChef.GetComponent<Renderer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && chefDoorRenderer != null && activeChefDoor != null)
        {
            chefDoorRenderer.material = activeChefDoor;
            chefDoorActivated = true;
            CheckAndOpenDoors();
        }

        if (other.CompareTag("rat") && dogDoorRenderer != null && activeDogDoor != null)
        {
            dogDoorRenderer.material = activeDogDoor;
            dogDoorActivated = true;
            CheckAndOpenDoors();
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
    }
}
