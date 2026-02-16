using UnityEngine;

[CreateAssetMenu(fileName = "SetEndImage", menuName = "Dialogue/Actions/Set Music")]
public class DialogueActionMusic : DialogueActionBase
{
    public AudioClip Clip;

    public override void Trigger()
    {
        MusicHandler.SetMusic(Clip);
    }
}