using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameHandler : MonoBehaviour
{

    [SerializeField] private EventsGenerator eventsGenerator;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private TimeController timeController;
    [SerializeField] private Health health;

    [SerializeField] private SpriteRenderer faceSprite;
    [SerializeField] private SpriteRenderer[] headSprite;
    [SerializeField] private SpriteRenderer bodySprite;

    private Sprite defaultFace;
    private Color defaultColor;
    
    //qualcosa per il movimento


    private BabyEvent[] faceEvents;
    private BabyEvent[] colorEvents;
    private BabyEvent[] behaviourEvents;

    private BabyEvent[] roundEvents;


    // mi serve per la creazione mauale del evento
    [SerializeField]
    private States faceState;

    private void Start()
    {
        EventsGenerator.BabyEventLists eventLists = eventsGenerator.GenerateEvent();
        faceEvents = eventLists.faceEvents;
        colorEvents = eventLists.colorEvents;
        behaviourEvents = eventLists.behaviourEvents;
        defaultColor = headSprite[0].color;
        defaultFace = faceSprite.sprite;
        StartRound();
    }

    private void Update()
    {
        //Test();
    }

    private void Test() // Testa una generazione  array random di eventi
    {
        Debug.Log("-------------------");
        BabyEvent[] ev = PickEvents();

        foreach (var e in ev)
        {
            Debug.Log(e.action.actionName + "   ---   " + e.state.stateName);
        }
    }

    //questo da collegarea la routine/level handler
    public void StartRound(int eventsQuantity = 1) // fa avviare il round  manuale di eventi in questo caso
    {
        //FOR NOW
        //roundEvents = new[] { new BabyEvent(new Action("DoubleSwipeUp", null), faceState.states.ElementAt(1) ), new BabyEvent(new Action("SingleSwipeRight", null), null) };
        roundEvents = PickEvents(2);
        //NOT FOR NOW
        //PickEvents(eventsQuantity) number must be defined 

        StateInitializer();
        inputHandler.StartCheckingEvents(roundEvents);

        //if action a new action is resolved
            //RUN Green border
            //StateDeactivator();
    }

    private void FinishRound()
    {
        faceSprite.sprite = defaultFace;
        headSprite[0].color = defaultColor;
        headSprite[1].color = defaultColor;
    }
    
    private void StateInitializer()
    {
     foreach(BabyEvent ev in roundEvents)
        {

            switch (ev.state.type)
            {
                case State.Type.Face:
                    faceSprite.sprite = ev.state.faceSprite;
                    break;
                case State.Type.Color:
                    headSprite[0].color = ev.state.skinColor;
                    headSprite[1].color = ev.state.skinColor;
                    break;
                case State.Type.Behaviour:
                    // TODO: Add Behaviour
                    break;
            }
        }
    }


    //-------- Round Action -----------
    public void TimeFinished() // stop checking 
    {
        inputHandler.StopCheckEvents(); 
        Debug.Log("Time Finished");
        FinishRound();
    }

    public void WrongAction()
    {
        health.takeDamage();
        timeController.switchPhase();
        Debug.Log("Wrong action");
        FinishRound();
    }

    public void AllActionDone()
    {
        //RUN HAPPY ANIMATION gold border plus happy face
        timeController.switchPhase();
        Debug.Log("All Action Done");
        FinishRound();
    }

    //------------- Set Up actions ----------
    private BabyEvent[] PickEvents(int eventsQuantity = 1)
    {
        Random rand = new Random();
        
        BabyEvent[] ret = new BabyEvent[eventsQuantity];
        
        List<BabyEvent> faceEv = new();
        faceEv.AddRange(faceEvents);
        List<BabyEvent> colorEv = new();
        colorEv.AddRange(colorEvents);
        List<BabyEvent> behaviourEv = new();
        behaviourEv.AddRange(behaviourEvents);

        for (int i = 0; i < ret.Length; i++)
        {
            switch (i)
            {
                case 0:
                    ret[i] = faceEv[rand.Next(faceEv.Count)];
                    faceEv.Remove(ret[i]);
                    break;
                case 1:
                    ret[i] = colorEv[rand.Next(colorEv.Count)];
                    colorEv.Remove(ret[i]);
                    break;
                case 2:
                    ret[i] = behaviourEv[rand.Next(behaviourEv.Count)];
                    colorEv.Remove(ret[i]);
                    break;
            }
        }

        return ret;
    }

    
}
