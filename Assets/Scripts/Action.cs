using System;
using UnityEngine;

[System.Serializable]
public class Action
{
    public string actionName;
    public Sprite sprite;

    public Action(string actionName, Sprite sprite)
    {
        this.actionName = actionName;
        this.sprite = sprite;
    }
}