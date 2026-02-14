using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DatingSimQuestion", menuName = "Dating Sim/DatingSimQuestion")]
public class DatingSimQuestion : ScriptableObject
{
    public string question;
    public List<string> answers;
    public List<Personality> questionPersonalities;
}
