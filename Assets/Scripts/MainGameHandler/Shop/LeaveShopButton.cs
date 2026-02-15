
using UnityEngine;

public class LeaveShopButton : EnvironmentButton
{
    [SerializeField] private DialogueSequence LeaveSequence;
    
    public override void Click()
    {
        DialogueManager.dialogueManager.SetActiveDialogueSequence(LeaveSequence);
        DialogueManager.dialogueManager.SetLiveMessage(null);
    }
}
