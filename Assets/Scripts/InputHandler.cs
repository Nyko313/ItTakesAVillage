using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Rigth,
        Left,
    }

    public enum AreaState
    {
        None,
        Clicked,
        DoubleClicked,
    }

    public struct Area
    {
        public float lastTimeClicked;
        public AreaState areaState;
    }
    
    // Swipe
    private Vector2 swipeDirectionDistance = Vector2.zero;
    private SwipeDirection swipeDirection = SwipeDirection.None;

    // Button Area
    private Area centerArea;
    private Area bottomLeftArea;
    private Area bottomRightArea;
    private float doubleClickTime = 0.3f;

    private void Start()
    {
        centerArea = new Area();
        bottomLeftArea = new Area();
        bottomRightArea = new Area();
    }

    void Update()
    {
        
        HandleSwipe();
 
        UpdateAreaClickedTimer(ref bottomRightArea);
        UpdateAreaClickedTimer(ref bottomLeftArea);
        UpdateAreaClickedTimer(ref centerArea);
    }

    public void BottomLeftBtnPressed()
    {
        UpdateAreaState(ref bottomLeftArea);
    }
    
    public void BottomRightBtnPressed(){
        UpdateAreaState(ref bottomRightArea);
    }
    
    public void CenterBtnPressed()
    {
        UpdateAreaState(ref centerArea);
    }

    private void UpdateAreaState(ref Area area)
    {
        if (swipeDirection != SwipeDirection.None)
        {
            area.areaState = AreaState.None;
            return;
        }
        
        switch (area.areaState)
        {
            case AreaState.None:
                area.areaState = AreaState.Clicked;
                area.lastTimeClicked = Time.time;
                break;
            case AreaState.Clicked:
                area.areaState = AreaState.DoubleClicked;
                area.lastTimeClicked = Time.time;
                break;
            
        }

    }

    private void UpdateAreaClickedTimer(ref Area area)
    {
        if (area.areaState != AreaState.None && Time.time - area.lastTimeClicked > doubleClickTime)
        {
            Debug.Log("Diomerdaccia");
            area.areaState = AreaState.None;
        }
    }
    
    private void HandleSwipe()
    {
        if (Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Moved:
                    swipeDirectionDistance += Input.GetTouch(0).deltaPosition;
                    break;
            }
        }
        else
        {
            swipeDirectionDistance = Vector2.zero;
            swipeDirection = SwipeDirection.None;
        }

        if (swipeDirectionDistance.x > 150)
        {
            swipeDirection = SwipeDirection.Left;
        }
        else if (swipeDirectionDistance.x < -150)
        {
            
            swipeDirection = SwipeDirection.Rigth;
        }
        if (swipeDirectionDistance.y > 100)
        {
            swipeDirection = SwipeDirection.Down;
        }
        else if (swipeDirectionDistance.y < -100)
        {
            swipeDirection = SwipeDirection.Up;
        }


    }
    
    
    
    private void  OnGUI()
    {
        int space = 55;
        int debugTextSize = 50;
        GUIStyle debugTextStyle = new GUIStyle();
        debugTextStyle.fontSize = 70;
        
        
        GUI.Label(new Rect(50, 80 + space * 0, debugTextSize, debugTextSize), "touchCount: " + Input.touchCount.ToString(), debugTextStyle);
        GUI.Label(new Rect(50, 80 + space * 1, debugTextSize, debugTextSize), "swipeDirection: " + swipeDirection.ToString(), debugTextStyle);
        GUI.Label(new Rect(50, 80 + space * 2, debugTextSize, debugTextSize), "centerArea: " + centerArea.areaState.ToString(), debugTextStyle);
        GUI.Label(new Rect(50, 80 + space * 3, debugTextSize, debugTextSize), "bottomLeftArea: " + bottomLeftArea.areaState.ToString(), debugTextStyle);
        GUI.Label(new Rect(50, 80 + space * 4, debugTextSize, debugTextSize), "bottomRightArea: " + bottomRightArea.areaState.ToString(), debugTextStyle);

        GUI.color = new Color(0,0,0,0);
        if (GUI.Button(new Rect(0, Screen.height * 0.25f, Screen.width, Screen.height * 0.5f), ""))
        {
            CenterBtnPressed();
        }
        if (GUI.Button(new Rect(0, Screen.height * 0.75f, Screen.width *0.5f, 800), ""))
        {
            BottomLeftBtnPressed();
        }
        if (GUI.Button(new Rect(Screen.width *0.5f, Screen.height * 0.75f, Screen.width *0.5f, 800), ""))
        {
            BottomRightBtnPressed();
        }
        
        GUI.color = Color.white;
    }

}
