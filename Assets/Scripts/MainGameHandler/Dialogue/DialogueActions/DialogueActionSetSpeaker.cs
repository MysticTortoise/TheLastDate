using UnityEngine;

[CreateAssetMenu(fileName = "LoadSceneAction", menuName = "Dialogue/Actions/Load Scene")]
public class DialogueActionSetSpeaker : DialogueActionBase
{
    public int PersonID;
    public DialoguePerson PersonData;
    public override void Trigger()
    {
        DialogueManager.dialogueManager.SetDialoguePersonData(PersonID, PersonData);
    }
}
