using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameHandler : MonoBehaviour
{

    [SerializeField] private EventsGenerator eventsGenerator;
    [SerializeField] private InputHandler inputHandler;

    private BabyEvent[] faceEvents;
    private BabyEvent[] colorEvents;
    private BabyEvent[] behaviourEvents;

    private void Start()
    {
        EventsGenerator.BabyEventLists eventLists = eventsGenerator.GenerateEvent();
        faceEvents = eventLists.faceEvents;
        colorEvents = eventLists.colorEvents;
        behaviourEvents = eventLists.behaviourEvents;
    }

    private void Test()
    {
        Debug.Log("-------------------");
        BabyEvent[] ev = PickEvents();

        foreach (var e in ev)
        {
            Debug.Log(e.action.actionName + "   ---   " + e.state.stateName);
        }
    }

    private void FixedUpdate()
    {
        Test();
    }

    public BabyEvent[] PickEvents(int eventsQuantity = 1)
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
