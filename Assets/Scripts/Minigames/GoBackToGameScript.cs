
using UnityEngine;

public class GoBackToGameScript : MonoBehaviour
{
    [SerializeField] private GameObject ReturnArea;

    public void GoBack()
    {
        PlayerGlobalHandler.LoadIntoMainGame(ReturnArea);
    }
}
