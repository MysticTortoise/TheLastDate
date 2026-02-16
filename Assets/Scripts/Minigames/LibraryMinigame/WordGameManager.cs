using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordGameManager : MonoBehaviour
{
    private WordGraph graph;
    [HideInInspector] public List<Button> buttonsClicked;
    [HideInInspector] public string wordString;
    private int score;
    [SerializeField] private TextMeshProUGUI scoreTextBox;
    [SerializeField] private TextAsset words;
    private HashSet<string> dictionary;
    [SerializeField] private Button backButton;
    private GoBackToGameScript backToGame;
    private StatBlock statChanges;
    private int smartsChanges;
    void Start()
    {
        statChanges = new();
        dictionary = new();
        string[] lines = words.text.Split("\n");
        foreach(string line in lines)
        {
            dictionary.Add(line);
        }
        backToGame = backButton.GetComponent<GoBackToGameScript>();
        buttonsClicked = new();
        wordString = "";
        score = 0;
        graph = GetComponentInChildren<WordGraph>();
    }

    public void OnClick(Button button)
    {
        if ((buttonsClicked.Count == 0) || (graph.adgacencyGraph[buttonsClicked[buttonsClicked.Count - 1].name].Contains(button) && !buttonsClicked.Contains(button)))
        {
            button.GetComponent<Image>().color = Color.green;
            wordString += button.GetComponentInChildren<TextMeshProUGUI>().text;
            buttonsClicked.Add(button);
        }
        else if (buttonsClicked[buttonsClicked.Count - 1] == button)
        {
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out Color newColor))
            {
                button.GetComponent<Image>().color = newColor;
            }
            wordString = wordString.Substring(0, wordString.Length - 1);
            buttonsClicked.Remove(button);
        }
    }

    public void SubmitWord()
    {
        if (dictionary.Contains(wordString))
        {
            score += wordString.Length;
            if (score >= 10)
            {
                score = Math.Abs(score%10);
                statChanges.smarts = 1;
                PlayerGlobalHandler.GlobalHandler.AddStats(statChanges);
                smartsChanges++;
            }
        }
        else
        {
            score -= 5;
            if (score < 0)
            {
                score = Math.Abs(score%10);
                statChanges.smarts = -1;
                PlayerGlobalHandler.GlobalHandler.AddStats(statChanges);
                smartsChanges--;
            }
        }
        graph.OnRegen();
        wordString = "";
        scoreTextBox.text = "Change In Intelligence: " + smartsChanges;
    }
}

