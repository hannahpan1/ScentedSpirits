using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPropeller : MonoBehaviour
{
    private bool spin = false;
    private int currID;
    public float degreePerSec; // degree it rotates per sec
    private Transform pivot;
    
    // Start is called before the first frame update
    void Start()
    {
        currID = gameObject.transform.parent.gameObject.GetInstanceID();
        pivot = transform.GetChild(0);
        GameEvents.current.spinPropeller = OnSpin;
        GameEvents.current.stopPropeller = OnStop;
    }

    // Update is called once per frame
    void Update()
    {
        if (spin)
        {
            transform.RotateAround(pivot.position, Vector3.forward, degreePerSec * Time.deltaTime);  
        }
    }

    void OnSpin(int ID)
    {
        if (currID == ID)
        {
            spin = true;
        }
    }

    void OnStop(int ID)
    {
        if (currID == ID)
        {
            spin = false;
        }
    }
}
