using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordGraph : MonoBehaviour
{
    private Button[] wordButtons;
    [HideInInspector] public Dictionary<string, LinkedList<Button>> adgacencyGraph;
    [SerializeField] private WordGameManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        manager.wordString = "";
        manager.buttonsClicked = new();
        adgacencyGraph = new();
        wordButtons = GetComponentsInChildren<Button>();
        foreach (Button button in wordButtons)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = GetRandomLetter().ToString();
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out Color newColor))
            {
                button.GetComponent<Image>().color = newColor;
            }
            adgacencyGraph.Add(button.name, new());
        }

        foreach(Button button in wordButtons)
        {
            foreach(Button otherButton in wordButtons)
            {
                Debug.Log((Math.Abs(button.transform.position.y - otherButton.transform.position.y)));
                if ((Math.Abs(button.transform.position.y - otherButton.transform.position.y) <= 60) && (Math.Abs(button.transform.position.x - otherButton.transform.position.x) <= 60) && (otherButton != button))
                {
                    adgacencyGraph[button.name].AddLast(otherButton);
                }
            }
        }
    }

    private static char GetRandomLetter()
    {
        return (char) UnityEngine.Random.Range(65, 91);
    }

    public void OnRegen()
    {
        GenerateBoard();
    }
}
