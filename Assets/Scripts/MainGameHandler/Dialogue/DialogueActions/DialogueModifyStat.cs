
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ModifyStat", menuName = "Dialogue/Actions/Modify Stat")]
public class DialogueModifyStat : DialogueActionBase
{
    public PlayerStat Stat;
    public float Amount;

    public override void Trigger()
    {
        var statBlock = new StatBlock();
        switch (Stat)
        {
            case PlayerStat.Money:
                statBlock.money = Amount;
                break;
            case PlayerStat.Empathy:
                statBlock.empathy = (int)Amount;
                break;
            case PlayerStat.Smarts:
                statBlock.smarts = (int)Amount;
                break;
            case PlayerStat.Rizz:
                statBlock.rizz = (int)Amount;
                break;
            case PlayerStat.Looks:
                statBlock.looks = (int)Amount;
                break;
            case PlayerStat.Charm:
                statBlock.charm = (int)Amount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        PlayerGlobalHandler.GlobalHandler.AddStats(statBlock);
    }
}
