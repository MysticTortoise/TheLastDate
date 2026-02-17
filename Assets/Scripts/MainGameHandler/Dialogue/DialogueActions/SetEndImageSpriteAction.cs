

using UnityEngine;

[CreateAssetMenu(fileName = "SetEndImage", menuName = "Dialogue/Actions/Set End Image")]
public class SetEndImageSpriteAction : DialogueActionBase
{
    public Sprite Image;

    public override void Trigger()
    {
        PlayerGlobalHandler.GlobalHandler.stats = new StatBlock
        {
            empathy = 0,
            charm = 0,
            looks = 0,
            money = 0,
            rizz = 0,
            smarts = 0,
        };
        EndScreenManager.EndBGSprite = Image;
    }
}
