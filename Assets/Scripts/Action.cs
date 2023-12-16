using System;
using UnityEngine;

[System.Serializable]
public class Action
{
    public string actionName;
    public Texture2D texture;

    public Action(string actionName, Texture2D texture)
    {
        this.actionName = actionName;
        this.texture = texture;
    }
}