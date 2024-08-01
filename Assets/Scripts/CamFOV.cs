using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFOV : MonoBehaviour
{
    private Dictionary<GameObject, ObjectFader> fadedItems;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fadedItems = new Dictionary<GameObject, ObjectFader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player is not null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider is null)
                {
                    return;
                }

                if (hit.collider.gameObject.CompareTag("PlayerVisibility"))
                {
                    // Nothing is in front of the player so check
                    // if there are faded items and turn them back to opaque
                    if (fadedItems.Count != 0)
                    {
                        foreach (ObjectFader fader in fadedItems.Values)
                        {
                            fader.fadeOn = false;
                        }
                        // Remove from faded items list
                        fadedItems.Clear();
                    }
                }
                else
                {
                    // Something is blocking the camera's view of the player
                    // Note: we want to minimize the number of times getComponent is called,
                    // so we keep track of which gameObjects have already been faded and skip if
                    // it's currently faded
                    if (!fadedItems.ContainsKey(hit.collider.gameObject))
                    {
                        ObjectFader fader = hit.collider.gameObject.GetComponent<ObjectFader>();
                        if (fader is not null)
                        {
                            fader.fadeOn = true;
                            fadedItems[hit.collider.gameObject] = fader;
                        }
                    }
                }
            }
        }
    }
}