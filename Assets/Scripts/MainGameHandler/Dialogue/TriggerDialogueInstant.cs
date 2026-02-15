
using System;
using UnityEngine;

public class TriggerDialogueInstant : MonoBehaviour
{
    [SerializeField] private DialogueSequence Sequence;

    private void Update()
    {
        if (DialogueManager.dialogueManager == null) return;
        if (DialogueManager.dialogueManager.isDialogueUp) return;
        
        DialogueManager.dialogueManager.SetActiveDialogueSequence(Sequence);
        Destroy(this);
    }
}
