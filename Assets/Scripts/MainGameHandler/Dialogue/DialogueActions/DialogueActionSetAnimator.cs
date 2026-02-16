
using UnityEngine;

[CreateAssetMenu(fileName = "AnimateAction", menuName = "Dialogue/Actions/Set Animator Int")]
public class DialogueActionSetAnimator : DialogueActionBase
{
    public string AnimatorTag;
    public string Name;
    public int Value;
    
    public override void Trigger()
    {
        GameTag.GetFirstObjectWith(AnimatorTag).GetComponent<Animator>().SetInteger(Name, Value);
    }
}
