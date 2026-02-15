
using System;
using UnityEngine;
using UnityEngine.UI;

public class MapHandler : MonoBehaviour
{
    private Button button;
    [SerializeField] private GameObject Scene;

    private void Start()
    {
        button = transform.Find("Button").GetComponent<Button>();
    }

    private void Update()
    {
        button.interactable = MainPlayerHandler.PlayerHandler.CanInteract();
    }

    public void Button()
    {
        MainPlayerHandler.PlayerHandler.DoSceneTransition(Scene);
    }
}
