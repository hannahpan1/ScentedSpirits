using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class biscuitThrow : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    float distance;
    float rise;
    float run;
    float lerpLength;
    float lerpPos;
    public bool stopped;
    GameObject odore;
    BoxCollider col;
    LayerMask jumpMask;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        odore = FindAnyObjectByType<PetNavigation2>().gameObject;
        transform.position = start;
        distance = (start - end).magnitude;
        lerpLength = distance * 0.02f;
        // Get the layer number of "Ignore Raycast"
        int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
        // Create a LayerMask that includes all layers
        jumpMask = ~0;

        // Exclude the "Ignore Raycast" layer
        jumpMask &= ~(1 << ignoreRaycastLayer);
        ignoreRaycastLayer = LayerMask.NameToLayer("cheese");
        jumpMask &= ~(1 << ignoreRaycastLayer);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(start, end, Mathf.Min(lerpPos / lerpLength, 1));
        lerpPos += Time.deltaTime;
        if (lerpPos > lerpLength)
        {
            stopped  = true;
        }
        if (stopped)
        {
            if (!groundCol())
            {
                transform.position += Vector3.down * Time.deltaTime;
            }

            if ((transform.position-odore.transform.position).magnitude < 1)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private bool groundCol()
    {
        Vector3 start = transform.position - (Vector3.down * 0.95f);
        Vector3 dir = Vector3.down;
        Debug.DrawRay(start, dir, Color.red, 1);
        RaycastHit hit;
        Physics.Raycast(start, dir, out hit, 1, jumpMask);
        if (hit.collider != null)
        {
            return true;
        }
        return false;

    }
}
