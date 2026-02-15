
using UnityEngine;

public class GoToAreaButton : EnvironmentButton
{
    [SerializeField] private GameObject Scene;

    public override void Click()
    {
        MainPlayerHandler.PlayerHandler.DoSceneTransition(Scene);
    }
}
