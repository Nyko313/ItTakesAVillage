using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private MotionDetection motionDetection;

    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Right,
        Left,
    }

    public enum AreaState
    {
        None,
        Clicked,
        DoubleClicked,
    }

    public enum EventState
    {
        NotCompleted,
        Completed
    }

    public struct Area
    {
        public float lastTimeClicked;
        public AreaState areaState;
        public string position;
    }
    
    // Check Event
    public bool[] eventCompleted;
    public bool[] eventError;
    private bool wrongAction;
    private BabyEvent[] eventsToCheck;
    private bool checkEvents = false;
    private bool checkMovement;
    private bool checkSwipe;
    private bool checkTouch;
    
    // Swipe
    private Vector2 swipeDirectionDistance = Vector2.zero;
    private SwipeDirection swipeDirection = SwipeDirection.None;

    // Button Area
    private Area centerArea;
    private Area bottomLeftArea;
    private Area bottomRightArea;
    private float doubleClickTime = 0.3f;

    // For the button
    [SerializeField] Animator chickAnimator;
    [SerializeField] Animator eggAnimator;
    [SerializeField] Animator deathAnimator;

    private void Start()
    {
        centerArea = new Area();
        centerArea.position = "Center";
        bottomLeftArea = new Area();
        bottomLeftArea.position = "Left";
        bottomRightArea = new Area();
        bottomRightArea.position = "Right";
    }

    void Update()
    {
        
        HandleSwipe();
 
        UpdateAreaClickedTimer(ref bottomRightArea);
        UpdateAreaClickedTimer(ref bottomLeftArea);
        UpdateAreaClickedTimer(ref centerArea);

        CheckMovementDetection();
    }
    
    // ---------- Read Event Action ----------

    public void StartCheckingEvents(BabyEvent[] events)
    {
        eventsToCheck = events;
        eventCompleted = new bool[events.Length];
        eventError = new bool[events.Length];
        checkEvents = true;
    }

    private void UpdateEvent()
    {
        if (wrongAction == true && checkEvents == true)
        {
            gameHandler.WrongAction();
            checkEvents = false;
            return;
        }

        bool allEventCompleted = true;

        // succes animation
        deathAnimator.SetTrigger("Succes");

        foreach (var b in eventCompleted)
        {
            if (b == false)
            {
                allEventCompleted = false;
            }
        }
        

        checkEvents = !allEventCompleted;
        
        if(allEventCompleted) gameHandler.AllActionDone();
    }

    public void StopCheckEvents()
    {
        checkEvents = false;
    }
    
    // ---------- Button Area ----------

    
    
    public void BottomLeftBtnPressed()
    {
        UpdateAreaState(ref bottomLeftArea);
        chickAnimator.SetTrigger("Left");
    }
    
    public void BottomRightBtnPressed(){
        UpdateAreaState(ref bottomRightArea);
        chickAnimator.SetTrigger("Right");
    }
    
    public void CenterBtnPressed()
    {
        UpdateAreaState(ref centerArea);
        eggAnimator.SetTrigger("Center");

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
            Debug.Log("Caspiterina");
            
            if(checkEvents) AreaPressedCheck(area);
            
            area.areaState = AreaState.None;
        }
    }

    private void AreaPressedCheck(Area area)
    {
        for (int i = 0; i < eventsToCheck.Length; i++)
        {
            if(eventCompleted[i] == true) continue;
            if (eventsToCheck[i].action.actionName.Contains("Press"))
            {
                if (eventsToCheck[i].action.actionName.Contains(area.position))
                {
                    if ((eventsToCheck[i].action.actionName.Contains("Double") && area.areaState == AreaState.DoubleClicked) || (eventsToCheck[i].action.actionName.Contains("Single") && area.areaState == AreaState.Clicked))
                    {
                        eventCompleted[i] = true;
                    }
                    else
                    {
                        eventError[i] = true;
                    }
                }else
                {
                    eventError[i] = true;
                }
            }
            else
            {
                eventError[i] = true;
            }
        }

        CheckEventError();

        UpdateEvent();
    }
    
    // ---------- Swipe ----------
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
            
            swipeDirection = SwipeDirection.Right;
        }
        if (swipeDirectionDistance.y > 100)
        {
            swipeDirection = SwipeDirection.Down;
        }
        else if (swipeDirectionDistance.y < -100)
        {
            swipeDirection = SwipeDirection.Up;
        }
        
        
        if(checkEvents && swipeDirection != SwipeDirection.None) SwipeEventCheck();
    }

    private void SwipeEventCheck()
    {
        for (int i = 0; i < eventsToCheck.Length; i++)
        {
            if(eventCompleted[i] == true) continue;
            if (eventsToCheck[i].action.actionName.Contains("Swipe"))
            {
                if (eventsToCheck[i].action.actionName.Contains("Right") && swipeDirection == SwipeDirection.Right)
                {
                    eventCompleted[i] = true;
                }else if (eventsToCheck[i].action.actionName.Contains("Left")&& swipeDirection == SwipeDirection.Right)
                {
                    eventCompleted[i] = true;
                }else if (eventsToCheck[i].action.actionName.Contains("Up")&& swipeDirection == SwipeDirection.Up)
                {
                    eventCompleted[i] = true;
                }else if (eventsToCheck[i].action.actionName.Contains("Down")&& swipeDirection == SwipeDirection.Down)
                {
                    eventCompleted[i] = true;
                }
                else
                {
                    eventError[i] = true;
                }

                if (eventCompleted[i] == true && eventsToCheck[i].action.actionName.Contains("Double") &&
                    Input.touchCount < 2)
                {
                    eventError[i] = true;
                }
            }
            else
            {
                eventError[i] = true;
            }
        }

        CheckEventError();
        
        UpdateEvent();  
    }

    private void CheckEventError()
    {
        wrongAction = true;
        foreach (var b in eventError)
        {
            if (b == false)
            {
                wrongAction = false;
            }
        }
    }
    
    // ---------- Motion Detection -------------

    private void CheckMovementDetection()
    {
        if(!checkEvents) return;

        if (motionDetection.currentMotion != MotionDetected.NO_MOTION)
        {
            
            for (int i = 0; i < eventsToCheck.Length; i++)
            {
                if (eventsToCheck[i].action.actionName.Contains("Swing"))
                {
                    
                    if (eventsToCheck[i].action.actionName.Contains("Vertically") && motionDetection.currentMotion == MotionDetected.UP_DOWN)
                    {
                        eventCompleted[i] = true;
                    }else if (eventsToCheck[i].action.actionName.Contains("Horizontally") && motionDetection.currentMotion == MotionDetected.LEFT_RIGHT)
                    {
                        eventCompleted[i] = true;
                    }else if (eventsToCheck[i].action.actionName.Contains("Front") && motionDetection.currentMotion == MotionDetected.BACK_FRONT)
                    {
                        eventCompleted[i] = true;
                    }
                    else
                    {
                        eventError[i] = true;
                    }
                }
                else
                {
                    eventError[i] = true;
                }
            }
            CheckEventError();
        
            UpdateEvent();
        }
        
        
    }

    private void  OnGUI()
    {
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
        
        // Remove for Debugging
        return;
        int space = 50;
        int debugTextSize = 50;
        GUIStyle debugTextStyle = new GUIStyle();
        debugTextStyle.fontSize = 50;
        
        
        GUI.Label(new Rect(10, 80 + space * 0, debugTextSize, debugTextSize), "touchCount: " + Input.touchCount.ToString(), debugTextStyle);
        GUI.Label(new Rect(10, 80 + space * 1, debugTextSize, debugTextSize), "swipeDirection: " + swipeDirection.ToString(), debugTextStyle);
        GUI.Label(new Rect(10, 80 + space * 2, debugTextSize, debugTextSize), "centerArea: " + centerArea.areaState.ToString(), debugTextStyle);
        GUI.Label(new Rect(10, 80 + space * 3, debugTextSize, debugTextSize), "bottomLeftArea: " + bottomLeftArea.areaState.ToString(), debugTextStyle);
        GUI.Label(new Rect(10, 80 + space * 4, debugTextSize, debugTextSize), "bottomRightArea: " + bottomRightArea.areaState.ToString(), debugTextStyle);
        GUI.Label(new Rect(10, 80 + space * 5, debugTextSize, debugTextSize), "motion: " + motionDetection.currentMotion.ToString(), debugTextStyle);
        GUI.Label(new Rect(10, 80 + space * 6, debugTextSize, debugTextSize), "motionMagnitude: " + motionDetection.currentMagnitudeLevel.ToString(), debugTextStyle);
        

        for(int i = 0; i < eventCompleted.Length; i++)
        {
            GUI.Label(new Rect(10, 80 + space * (7 + i), debugTextSize, debugTextSize), "event " + i +": " + eventCompleted[i] + " : " + eventsToCheck[i].action.actionName, debugTextStyle);
        }
    }

}
