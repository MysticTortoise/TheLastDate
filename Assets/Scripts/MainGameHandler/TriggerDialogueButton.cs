
using UnityEngine;

public class TriggerDialogueButton : EnvironmentButton
{
    [SerializeField] private DialogueSequence Sequence;

    public override void Click()
    {
        DialogueManager.dialogueManager.SetActiveDialogueSequence(Sequence);
    }
}
