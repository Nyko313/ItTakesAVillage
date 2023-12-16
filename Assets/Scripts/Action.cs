using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Action", menuName = "BabyEvent/Action")]
public class Action : ScriptableObject
{
    public string actionName;
    public Texture2D icon;
}
