using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsGenerator : MonoBehaviour
{

    [SerializeField] State[] states;

    

    public Action GenerateAction()
    {
        System.Random rand = new System.Random();
        Action action= new Action();

        action.State = states[rand.Next(states.Length)];

        return action;
    }
}
