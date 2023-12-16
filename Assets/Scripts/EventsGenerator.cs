using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        System.Random rand = new System.Random();
        // TODO: Change array size with playerNumber/3
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
        
        // Face Events generation
        for (int i = 0; i < faceEvents.Length; i++)
        {
            State rState = faceS.ElementAt(rand.Next(faceS.Count));
            Action rAction = act.ElementAt(rand.Next(act.Count));
            faceEvents[i] = new(rAction, rState);
            faceS.Remove(rState);
            act.Remove(rAction);
        }
        
        // Color Events Generation
        for (int i = 0; i < colorEvents.Length; i++)
        {
            State rState = colorS.ElementAt(rand.Next(colorS.Count));
            Action rAction = act.ElementAt(rand.Next(act.Count));
            colorEvents[i] = new(rAction, rState);
            colorS.Remove(rState);
            act.Remove(rAction);
        }
        
        // Behaviour Events Generation
        for (int i = 0; i < behaviourEvents.Length; i++)
        {
            State rState = behaviourS.ElementAt(rand.Next(behaviourS.Count));
            Action rAction = act.ElementAt(rand.Next(act.Count));
            behaviourEvents[i] = new(rAction, rState);
            behaviourS.Remove(rState);
            act.Remove(rAction);
        }

        return new BabyEventLists(faceEvents, colorEvents, behaviourEvents);
    }
}
