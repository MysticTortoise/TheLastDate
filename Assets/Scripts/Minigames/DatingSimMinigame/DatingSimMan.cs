using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DatingSimMan", menuName = "Dating Sim/DatingSimMan")]
public class DatingSimMan : ScriptableObject
{
    public string manName;
    public Sprite image;
    public Personality type;
}

public enum Personality
{
    Sporty, Social, Shy, Neither
}
