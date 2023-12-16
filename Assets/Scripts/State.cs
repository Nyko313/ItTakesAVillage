using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : ScriptableObject
{
    public enum Category
    {
        Face,
        Color,
        Behaviour
    }

    public string stateName;
    public Texture stateImg;
    public Texture gameOverImg;
    public Category category;
}
