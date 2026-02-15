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
            button.GetComponent<Image>().color = Color.grey;
            adgacencyGraph.Add(button.name, new());
        }

        foreach(Button button in wordButtons)
        {
            foreach(Button otherButton in wordButtons)
            {
                if ((Math.Abs(button.transform.position.y - otherButton.transform.position.y) <= 65) && (Math.Abs(button.transform.position.x - otherButton.transform.position.x) <= 114) && (otherButton != button))
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
