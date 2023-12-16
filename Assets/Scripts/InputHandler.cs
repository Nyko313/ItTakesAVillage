using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    enum SwipeDirection
    {
        None,
        Up,
        Down,
        Rigth,
        Left,
    }

    Vector2 swipeDirectionDelta = Vector2.zero;
    SwipeDirection swipeDirection = SwipeDirection.None;

    private void Start()
    {
    }

    void Update()
    {

        HandleSwipe();

    }

    private void HandleSwipe()
    {
        if (Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Moved:
                    swipeDirectionDelta += Input.GetTouch(0).deltaPosition;
                    break;
            }


        }
        else
        {
            swipeDirectionDelta = Vector2.zero;
            swipeDirection = SwipeDirection.None;
        }

        if (swipeDirectionDelta.x > 150)
        {
            swipeDirection = SwipeDirection.Left;
        }
        else if (swipeDirectionDelta.x < -150)
        {
            
            swipeDirection = SwipeDirection.Rigth;
        }
        if (swipeDirectionDelta.y > 100)
        {
            swipeDirection = SwipeDirection.Down;
        }
        else if (swipeDirectionDelta.y < -100)
        {
            swipeDirection = SwipeDirection.Up;
        }


    }

    private void  OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 70;
        GUI.Label(new Rect(50, 80, 100, 100), "touchCount: " + Input.touchCount.ToString(), myStyle);
        GUI.Label(new Rect(50, 130, 100, 100), "swipeDirection: " + swipeDirection.ToString(), myStyle);
    }

}
