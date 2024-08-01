using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyspinfloat : MonoBehaviour
{
    Vector3 startPos;
    Vector3 startRot;
    float rot;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        rot += Time.deltaTime;
        transform.eulerAngles = new Vector3(startRot.x, rot * 90, startRot.z);
        transform.position = new Vector3(startPos.x, startPos.y + (Mathf.Sin(rot * 2) * 0.25f), startPos.z);
    }
}
