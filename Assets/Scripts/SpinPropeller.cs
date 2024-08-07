using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPropeller : MonoBehaviour
{
    private bool spin = false;
    private int currID;
    public float degreePerSec; // degree it rotates per sec
    private Transform pivot;

    void Start()
    {
        currID = gameObject.transform.parent.gameObject.GetInstanceID();
        pivot = transform.GetChild(0);
        GameEvents.current.spinPropeller += OnSpin;
        GameEvents.current.stopPropeller += OnStop;
    }

    void Update()
    {
        if (spin)
        {
            RotateFanBlade();
        }
    }

    void RotateFanBlade()
    {
        transform.RotateAround(pivot.position,pivot.forward, degreePerSec * Time.deltaTime);
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
