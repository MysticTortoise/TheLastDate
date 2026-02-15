
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]

[CreateAssetMenu(fileName = "LoadUnityScene", menuName = "Dialogue/Actions/Load Unity Scene")]
public class DialogueLoadUnityScene : DialogueActionBase
{
    public string SceneName;
    
    public override void Trigger()
    {
        SceneManager.LoadScene(SceneName);
    }
}
