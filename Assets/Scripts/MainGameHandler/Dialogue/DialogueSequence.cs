
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class DialogueOption
{
    public string Option;
    public DialogueSequence Sequence;
}

[System.Serializable]
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
