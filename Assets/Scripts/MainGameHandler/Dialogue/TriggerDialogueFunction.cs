
using UnityEngine;

public class TriggerDialogueFunction : MonoBehaviour
{
    [SerializeField] private DialogueSequence Sequence;

    public void Trigger()
    {
        if (DialogueManager.dialogueManager == null) return;
        if (DialogueManager.dialogueManager.isDialogueUp) return;
        
        DialogueManager.dialogueManager.SetActiveDialogueSequence(Sequence);
    }
}
