using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTeleporter : MonoBehaviour
{
    public Transform player, destination;
    public GameObject playerg;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _audioSource.Play();
            playerg.SetActive(false);
            player.position = destination.position;
            playerg.SetActive(true);
        }
    }
}
