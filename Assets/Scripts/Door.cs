using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioClip doorOpen;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement player = other.GetComponent<playerMovement>();
            if (player != null && player.HasKey)
            {
                OpenDoor();
                player.HasKey = false;
            }
        }
    }

    private void OpenDoor()
    {
        source.clip = doorOpen;
        source.Play();
        MeshRenderer renderer = GetComponent<MeshRenderer>();  // deactivate the door object
        BoxCollider bc = GetComponent<BoxCollider>();
        MeshCollider mc = GetComponent<MeshCollider>();
        mc.enabled = false;
        bc.enabled = false;
        renderer.enabled = false;
        

    }
}
