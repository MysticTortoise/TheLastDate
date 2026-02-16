
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject DateScene;

    public void GoBack()
    {
        PlayerGlobalHandler.GlobalHandler.stats.charm = 0;
        PlayerGlobalHandler.LoadIntoMainGame(DateScene);
    }

    public void TitleScreen()
    {
        SceneManager.LoadScene("Title");
    }
}
