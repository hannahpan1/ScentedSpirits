using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceCamera : MonoBehaviour
{
    Camera camera;
    Vector3 parentsPos;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        parentsPos = GetComponentInParent<Transform>().position;
        float height = Mathf.Max(0, Mathf.Sin(transform.position.x + Time.time));
        transform.eulerAngles = Camera.main.transform.eulerAngles;
        //transform.position = parentsPos + new Vector3(0, height, 0);
    }
}
