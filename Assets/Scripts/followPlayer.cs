using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    GameObject player;
    public Vector3 offset;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = transform.eulerAngles;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = player.transform.position + offset;
        //transform.eulerAngles = direction;
    }
}
