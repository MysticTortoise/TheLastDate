
using UnityEngine;

[CreateAssetMenu(fileName = "LoadSceneAction", menuName = "Dialogue/Actions/Load Scene")]
public class DialogueActionLoadScene : DialogueActionBase
{
    public GameObject Scene;
    public override void Trigger()
    {
        Debug.Log("LOAD SCENE");
    }
}
