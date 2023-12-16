using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyEvent
{
    public Action action;
    public State state;

    public BabyEvent(Action action, State state)
    {
        this.action = action;
        this.state = state;
    }
}
