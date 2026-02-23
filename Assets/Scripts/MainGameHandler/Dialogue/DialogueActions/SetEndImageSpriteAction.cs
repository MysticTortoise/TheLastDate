

using UnityEngine;

[CreateAssetMenu(fileName = "SetEndImage", menuName = "Dialogue/Actions/Set End Image")]
public class SetEndImageSpriteAction : DialogueActionBase
{
    public Sprite Image;

    public override void Trigger()
    {
        EndScreenManager.EndBGSprite = Image;
    }
}
