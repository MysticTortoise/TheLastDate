
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject DateScene;

    public static Sprite EndBGSprite;

    private void Start()
    {
        transform.Find("BGImage").GetComponent<Image>().sprite = EndBGSprite;
    }

    public void GoBack()
    {
        PlayerGlobalHandler.GlobalHandler.stats.charm = 0;
        PlayerGlobalHandler.LoadIntoMainGame(DateScene);
    }

    public void TitleScreen()
    {
        Destroy(PlayerGlobalHandler.GlobalHandler.gameObject);
        PlayerGlobalHandler.GlobalHandler = null;
        SceneManager.LoadScene("Title");
    }
}
