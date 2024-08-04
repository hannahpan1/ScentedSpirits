using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        if (current != null && current != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            current = this; 
        }
    }

    public delegate void VectorDelegate(Vector3 vector);
    public delegate void StringDelegate(string s);
    public delegate void EmptyDelegate();
    public delegate void FloatDelegate(float n);

    public VectorDelegate followScent;
    public StringDelegate changeAbility;
    public EmptyDelegate gameOver;
    public EmptyDelegate resetPetPos;
    public VectorDelegate setCheckpoint;
    public VectorDelegate setPetSpawn;
    public EmptyDelegate itemEaten;
    public FloatDelegate fanTriggered;
    public EmptyDelegate lowerBridge;
    public EmptyDelegate stopBridge;
}
