using JetBrains.Annotations;
//using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw2 : MonoBehaviour
{
    
    Camera camera;
    LayerMask layerMask;
    private InventoryManager inventoryManager;
    public AudioClip throwSound;
    AudioSource[] sources;
    GameObject crosshair;
    [SerializeField] GameObject crosshairPrefab;
    Vector2 cursorPoint;
    public Animator myAnim;
    bool controller;
    bool fireButton;
    public bool canThrow;

    // Start is called before the first frame update
    void Start()
    {
        sources = GetComponents<AudioSource>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        Debug.Log("found: " + inventoryManager.name);
        camera = Camera.main;
        layerMask &= ~(1 << 3);
        layerMask |= (1 << 0);
        canThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            controller = false;
        }
        else if (Input.GetAxis("HorCam") != 0 || Input.GetAxis("VerCam") != 0)
        {
            controller = true;
        }
        if (controller)
        {
            cursorPoint += new Vector2(Input.GetAxis("HorCam"), -Input.GetAxis("VerCam")) * 5f;
            
        } else
        {
            cursorPoint = Input.mousePosition;
        }
        if (Input.GetButtonUp("Fire1") && canThrow)
        {
            myAnim.SetBool("Throwing", false);
            Destroy(crosshair);
            if (!Input.GetButton("CancelThrow"))
            {
                //if (inventoryManager.GetSelectedItem(true).itemType == )
                if (inventoryManager.GetSelectedItem(false) != null)
                {
                    throwCheese(inventoryManager.GetSelectedItem(false).throwablePrefab);
                }
            }
        } else if (Input.GetButtonDown("Fire1") && canThrow) 
        {
            myAnim.SetBool("Throwing", true);
            if (controller)
            {
                cursorPoint = Camera.main.WorldToScreenPoint(transform.position);
            }
            crosshair = Instantiate(crosshairPrefab);
            moveCrosshair(crosshair);

        } else if (Input.GetButton("Fire1") && canThrow)
        {
            moveCrosshair(crosshair);
        }
    }

    private Vector3 launchVector(Vector3 startPos, Vector3 targetPos, Rigidbody rb)
    {
        // Calculate the displacement between the target and the start position
        Vector3 displacement = targetPos - startPos;
        float displacementY = Mathf.Abs(displacement.y);
        displacement.y = 0;

        // Calculate the time it will take for the projectile to reach the target
        float time = Mathf.Sqrt((2 * displacementY) / -Physics.gravity.y);

        // Calculate the initial velocity (launch vector) needed to reach the target
        Vector3 velocity = displacement / time;
        velocity.y = displacementY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        return velocity*0.5f;
    }

    private void moveCrosshair(GameObject crosshair)
    {
        Vector3 mousePosInWorld;
        mousePosInWorld = camera.ScreenToWorldPoint(new Vector3(cursorPoint.x, cursorPoint.y, 8f));
        Vector3 mousePosInWorldFar;
        mousePosInWorldFar = camera.ScreenToWorldPoint(new Vector3(cursorPoint.x, cursorPoint.y, 10f));
        RaycastHit hit;
        Physics.Raycast(mousePosInWorld, mousePosInWorldFar - mousePosInWorld, out hit, 50, layerMask);
        Debug.DrawLine(mousePosInWorld, hit.point, Color.red, 2f);
        Vector3 raypoint = hit.point;
        Quaternion normal = Quaternion.FromToRotation(Vector3.up, hit.normal);

        crosshair.transform.position = raypoint;
        crosshair.transform.rotation = normal;
    }

    private void throwCheese(GameObject throwablePrefab)
    {
        Vector3 mousePosInWorld;
        mousePosInWorld = camera.ScreenToWorldPoint(new Vector3(cursorPoint.x, cursorPoint.y, 8f));
        Vector3 mousePosInWorldFar;
        mousePosInWorldFar = camera.ScreenToWorldPoint(new Vector3(cursorPoint.x, cursorPoint.y, 10f));
        RaycastHit hit;
        Physics.Raycast(mousePosInWorld, mousePosInWorldFar - mousePosInWorld, out hit, 50, layerMask);
        Debug.DrawLine(mousePosInWorld, hit.point, Color.red, 2f);
        Vector3 raypoint = hit.point;
        float throwDistance;
        if (hit.collider != null)
        {
            AudioSource aS = findEmptySource(sources);
            aS.clip = throwSound;
            aS.Play();
            inventoryManager.GetSelectedItem(true);
            Debug.DrawRay(hit.point, Vector3.up, Color.magenta, 2f);
            GameObject gameObject;
            biscuitThrow bt;
            gameObject = Instantiate(throwablePrefab, transform.position, Quaternion.identity);
            bt = gameObject.GetComponent<biscuitThrow>();
            bt.start = transform.position - ((transform.position - raypoint).normalized * 1);
            bt.end = hit.point;
            
            // Item is thrown, used for pet navigation
            //GameEvents.current.itemThrown?.Invoke();
        }
    }

    public AudioSource findEmptySource(AudioSource[] array)
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
