
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public enum PlayerStat
{
    Money,
    Empathy,
    Smarts,
    Rizz,
    Looks,
    Charm
}

[Serializable]
public class DialogueRequirement
{
    public PlayerStat stat;
    public float number;
    public bool GreaterThan;

    [CanBeNull] public ItemDefinition Item;

    public bool IsMet()
    {
        if (Item)
            return PlayerGlobalHandler.GlobalHandler.heldItems.Contains(Item);
        
        
        
        float num = stat switch
        {
            PlayerStat.Money => PlayerGlobalHandler.GlobalHandler.stats.money,
            PlayerStat.Empathy => PlayerGlobalHandler.GlobalHandler.stats.empathy,
            PlayerStat.Smarts => PlayerGlobalHandler.GlobalHandler.stats.smarts,
            PlayerStat.Rizz => PlayerGlobalHandler.GlobalHandler.stats.rizz,
            PlayerStat.Looks => PlayerGlobalHandler.GlobalHandler.stats.looks,
            PlayerStat.Charm => PlayerGlobalHandler.GlobalHandler.stats.charm,
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
    public bool AnyMet = false;

    public bool RequirementsMet()
    {
        if (Requirements.Count <= 0)
            return true;
        if (AnyMet)
            return Requirements.Any(re => re.IsMet());
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
