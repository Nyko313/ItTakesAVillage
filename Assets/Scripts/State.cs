using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : ScriptableObject
{
    string stateName;
    Texture image;
    enum Type
    {
        Type1,
        Type2,
        Type3
    }
    Type type;
}
