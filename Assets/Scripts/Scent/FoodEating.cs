using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class FoodEating : MonoBehaviour
{
    public string eatsMe;
    LayerMask layerMask;
    AudioSource[] sources;
    public AudioClip[] hitSounds;
    // Start is called before the first frame update
    void Start()
    {
        sources = GetComponents<AudioSource>();
        layerMask &= ~(1 << 3);
        layerMask |= (1 << 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(eatsMe) && groundCol())
        {
            GameEvents.current.changeAbility?.Invoke("Humanoid");
            Destroy(gameObject.transform.parent.gameObject);
            GameEvents.current.itemEaten?.Invoke();
        }
    }

    public bool groundCol()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1f, layerMask);
        if (hit.collider != null)
        {
            return true;
            AudioSource aS = findEmptySource(sources);
            AudioClip aClip = hitSounds[Random.Range(0, hitSounds.Length)];
            aS.clip = aClip;
            aS.Play();
        }
        return false;
    }

    private AudioSource findEmptySource(AudioSource[] array)
    {
        if (array != null)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (!array[i].isPlaying)
                {
                    return array[i];
                }
            }
        }
        return null;
    }
}
