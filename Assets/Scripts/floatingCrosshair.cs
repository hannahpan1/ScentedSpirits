using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingCrosshair : MonoBehaviour
{
    [SerializeField] Vector3 Direction;
    public Vector3 offset;
    public float speed;
    public float amplitude;
    // Start is called before the first frame update
    void Start()
    {
        Direction = Direction.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = (Direction * (Mathf.Sin(Time.time * speed) * amplitude));
    }
}
