using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LoadSceneAction", menuName = "Dialogue/Actions/Pick Sequence")]
public class DialoguePickSequence : DialogueActionBase
{
    [System.Serializable]
    public class PossibleSequence
    {
        public List<DialogueRequirement> Requirements;
        public DialogueSequence Sequence;
    }
    
    public List<PossibleSequence> PossibleRequirements;

    private bool IsOptionValid(List<DialogueRequirement> requirements)
    {
        if (requirements.Count <= 0)
            return true;
        return requirements.All(requirement => requirement.IsMet());
    }
    
    public override void Trigger()
    {
        for (int i = 0; i < PossibleRequirements.Count; i++)
        {
            PossibleSequence possiblePair = PossibleRequirements[i];
            if (!IsOptionValid(possiblePair.Requirements)) continue;
            
            DialogueManager.dialogueManager.SetActiveDialogueSequence(possiblePair.Sequence);
            return;
        }
        //int rand = Random.Range(0, PossibleSequences.Count);
        //DialogueManager.dialogueManager.SetActiveDialogueSequence(PossibleSequences[rand]);
    }
}