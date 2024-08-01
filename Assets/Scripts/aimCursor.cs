using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimCursor : MonoBehaviour
{
    public bool controller;
    float cooldownMax = 3f;
    float cooldown;
    bool visible;
    RectTransform rectTransform;
    


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            controller = false;
        } else if (Input.GetAxis("Horizontal Cam") != 0 || Input.GetAxis("Vertical Cam") != 0)
        {
            controller = true;
        }
        if (controller)
        {
            controllerVoid();
        } else
        {
            keyboardVoid();
        }
    }

    private void controllerVoid()
    {
        if (!visible) {
            if (Input.GetAxis("Horizontal Cam") != 0 || Input.GetAxis("Vertical Cam") != 0)
            {
                
                visible = true;

                /*
                 * //this is your object that you want to have the UI element hovering over
GameObject WorldObject;

//this is the ui element
RectTransform UI_Element;

//first you need the RectTransform component of your canvas
RectTransform CanvasRect=Canvas.GetComponent<RectTransform>();

//then you calculate the position of the UI element
//0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

Vector2 ViewportPosition=Cam.WorldToViewportPoint(WorldObject.transform.position);
Vector2 WorldObject_ScreenPosition=new Vector2(
((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)),
((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)));

//now you can set the position of the ui element
UI_Element.anchoredPosition=WorldObject_ScreenPosition;
                 */
            }
        }
        
    }

    private void keyboardVoid()
    {

    }
}
