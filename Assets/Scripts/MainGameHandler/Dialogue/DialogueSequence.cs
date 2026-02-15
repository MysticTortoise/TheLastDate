
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public enum PlayerStat
{
    Money,
    Empathy,
    Smarts,
    Rizz,
    Looks
}

[Serializable]
public class DialogueRequirement
{
    public PlayerStat stat;
    public float number;
    public bool GreaterThan;

    public bool IsMet()
    {
        float num = stat switch
        {
            PlayerStat.Money => MainPlayerHandler.PlayerHandler.playerStats.money,
            PlayerStat.Empathy => MainPlayerHandler.PlayerHandler.playerStats.empathy,
            PlayerStat.Smarts => MainPlayerHandler.PlayerHandler.playerStats.smarts,
            PlayerStat.Rizz => MainPlayerHandler.PlayerHandler.playerStats.rizz,
            PlayerStat.Looks => MainPlayerHandler.PlayerHandler.playerStats.looks,
            _ => throw new ArgumentOutOfRangeException()
        };

        if (GreaterThan)
        {
            return num >= number;
        }
        else
        {
            return num <= number;
        }
    }
}

[Serializable]
public class DialogueOption
{
    public string Option;
    public DialogueSequence Sequence;
    public List<DialogueRequirement> Requirements;

    public bool RequirementsMet()
    {
        return Requirements.All(re => re.IsMet());
    }
}

[Serializable]
public class DialogueNode
{
    [FormerlySerializedAs("message")] public DialogueMessage Message;
    [FormerlySerializedAs("DialogueActions")] public List<DialogueActionBase> DialogueActionsStart = new();
    public List<DialogueActionBase> DialogueActionsEnd = new();
    [FormerlySerializedAs("StartOptions")] public List<DialogueOption> Options = new();
}

[CreateAssetMenu(fileName = "NewDialogueSequence", menuName = "Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    public List<DialogueNode> dialogueNodes;
}
