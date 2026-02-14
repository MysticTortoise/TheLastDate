
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class DialogueNode
{
    [FormerlySerializedAs("message")] public DialogueMessage Message;
    public string OnFinishedMessage = "";
}

[CreateAssetMenu(fileName = "NewDialogueSequence", menuName = "TLD/Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    public List<DialogueNode> dialogueNodes;
}
