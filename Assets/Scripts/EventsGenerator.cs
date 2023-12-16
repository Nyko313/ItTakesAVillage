using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsGenerator : MonoBehaviour
{
    [SerializeField]
    private State[] faceState;
    [SerializeField]
    private State[] colorState;
    [SerializeField]
    private State[] behaviourState;
    [SerializeField]
    private Action[] action;

    public struct BabyEventLists
    {
        BabyEvent[] faceEvents;
        BabyEvent[] colorEvents;
        BabyEvent[] behaviourEvents;

        public BabyEventLists(BabyEvent[] faceEvents, BabyEvent[] colorEvents, BabyEvent[] behaviourEvents)
        {
            this.faceEvents = faceEvents;
            this.colorEvents = colorEvents;
            this.behaviourEvents = behaviourEvents;
        }
    }

    public BabyEventLists GenerateEvent()
    {
        BabyEvent[] faceEvents = new BabyEvent[3];
        BabyEvent[] colorEvents = new BabyEvent[3];
        BabyEvent[] behaviourEvents = new BabyEvent[3];

        List<State> faceS = new();
        faceS.AddRange(faceState);
        List<State> colorS = new();
        colorS.AddRange(colorState);
        List<State> behaviourS = new();
        behaviourS.AddRange(behaviourState);
        List<Action> act = new();
        act.AddRange(action);

        for (int i = 0; i < faceEvents.Length; i++)
        {
            faceEvents.
        }

        return new BabyEventLists(faceEvents, colorEvents, behaviourEvents);
    }
}
