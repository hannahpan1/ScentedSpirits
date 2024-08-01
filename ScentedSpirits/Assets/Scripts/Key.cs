using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] AudioClip keyClip;
    AudioSource source;

    private void OnEnable()
    {
        if (source != null && keyClip != null) 
        {
            source = GetComponent<AudioSource>();
            source.clip = keyClip;
            source.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement player = other.GetComponent<playerMovement>();
            if (player != null)
            {
                player.PickUpKey();
                Destroy(gameObject);
            }
        }
    }
}
