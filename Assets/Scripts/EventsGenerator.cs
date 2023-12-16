using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventsGenerator : MonoBehaviour
{
    [SerializeField]
    private States faceState;
    [SerializeField]
    private States colorState;
    [SerializeField]
    private States behaviourState;
    [SerializeField]
    private Actions action;

    public struct BabyEventLists
    {
        public BabyEvent[] faceEvents;
        public BabyEvent[] colorEvents;
        public BabyEvent[] behaviourEvents;

        public BabyEventLists(BabyEvent[] faceEvents, BabyEvent[] colorEvents, BabyEvent[] behaviourEvents)
        {
            this.faceEvents = faceEvents;
            this.colorEvents = colorEvents;
            this.behaviourEvents = behaviourEvents;
        }
    }
    public BabyEventLists GenerateEvent()
    {
        System.Random rand = new System.Random();
        // TODO: Change array size with playerNumber/3
        BabyEvent[] faceEvents = new BabyEvent[3];
        BabyEvent[] colorEvents = new BabyEvent[3];
        BabyEvent[] behaviourEvents = new BabyEvent[3];

        List<State> faceS = new();
        faceS.AddRange(faceState.states);
        List<State> colorS = new();
        colorS.AddRange(colorState.states);
        List<State> behaviourS = new();
        behaviourS.AddRange(behaviourState.states);
        List<Action> act = new();
        act.AddRange(action.actions);
        
        // Face Events generation
        for (int i = 0; i < faceEvents.Length; i++)
        {
            State rStates = faceS.ElementAt(rand.Next(faceS.Count));
            Action rActions = act.ElementAt(rand.Next(act.Count));
            faceEvents[i] = new(rActions, rStates);
            faceS.Remove(rStates);
            act.Remove(rActions);
        }
        
        // Color Events Generation
        for (int i = 0; i < colorEvents.Length; i++)
        {
            State rStates = colorS.ElementAt(rand.Next(colorS.Count));
            Action rActions = act.ElementAt(rand.Next(act.Count));
            colorEvents[i] = new(rActions, rStates);
            colorS.Remove(rStates);
            act.Remove(rActions);
        }
        
        // Behaviour Events Generation
        for (int i = 0; i < behaviourEvents.Length; i++)
        {
            State rStates = behaviourS.ElementAt(rand.Next(behaviourS.Count));
            Action rActions = act.ElementAt(rand.Next(act.Count));
            behaviourEvents[i] = new(rActions, rStates);
            behaviourS.Remove(rStates);
            act.Remove(rActions);
        }

        return new BabyEventLists(faceEvents, colorEvents, behaviourEvents);
    }
}
