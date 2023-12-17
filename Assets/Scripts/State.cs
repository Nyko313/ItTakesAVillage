using System;
using UnityEngine;

[System.Serializable]
public class State
{
    public enum Type
    {
        Face,
        Color,
        Behaviour
    }
    
    public string stateName;
    public Sprite faceSprite;
    public Color skinColor;
    public Type type;
}