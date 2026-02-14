
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
    public List<DialogueActionBase> DialogueActions = new();
    public List<DialogueOption> Options = new();
}

[CreateAssetMenu(fileName = "NewDialogueSequence", menuName = "Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    public List<DialogueNode> dialogueNodes;
}
