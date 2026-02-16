
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionOnEvent : MonoBehaviour
{
    [SerializeField] private string SceneName;

    public void DoTransition()
    {
        SceneManager.LoadScene(SceneName);
    }
}
