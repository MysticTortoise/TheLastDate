using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LoadSceneAction", menuName = "Dialogue/Actions/Random Sequence")]
public class DialoguePickRandomSequence : DialogueActionBase
{
    public List<DialogueSequence> PossibleSequences;
    public override void Trigger()
    {
        int rand = Random.Range(0, PossibleSequences.Count);
        DialogueManager.dialogueManager.SetActiveDialogueSequence(PossibleSequences[rand]);
    }
}