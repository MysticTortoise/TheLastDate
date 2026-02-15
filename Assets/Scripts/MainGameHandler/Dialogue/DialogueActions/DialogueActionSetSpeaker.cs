using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LoadSceneAction", menuName = "Dialogue/Actions/Set Speaker")]
public class DialogueActionSetSpeaker : DialogueActionBase
{
    public int PersonID;
     public DialoguePersonData PersonData;
    public override void Trigger()
    {
        DialogueManager.dialogueManager.SetDialoguePersonData(PersonID, PersonData);
    }
}
