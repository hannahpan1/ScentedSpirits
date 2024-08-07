using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform player, destination, dog;
    public GameObject playerg;
    private AudioSource _audioSource;
    private InventoryManager inventory;
    
    private void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _audioSource.Play();
            inventory.ClearInventory();
            playerg.SetActive(false);
            player.position = destination.position;
            dog.position = destination.position;
            playerg.SetActive(true);
        }
    }
}
