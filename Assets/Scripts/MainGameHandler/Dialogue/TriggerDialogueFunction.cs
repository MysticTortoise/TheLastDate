
using UnityEngine;

public class TriggerDialogueFunction : MonoBehaviour
{
    [SerializeField] private DialogueSequence Sequence;

    public void Trigger()
    {
        DialogueManager.dialogueManager.SetActiveDialogueSequence(Sequence);
    }
}
