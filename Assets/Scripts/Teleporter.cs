using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform player, destination, dog;
    public GameObject playerg;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerg.SetActive(false);
            player.position = destination.position;
            dog.position = destination.position;
            playerg.SetActive(true);
        }
    }
}
