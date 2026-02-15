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
    [SerializeField] private TextMeshProUGUI timerTextBox;
    [SerializeField] private float time;
    private StatBlock statChanges;
    void Start()
    {
        statChanges = new();
        dictionary = new();
        string[] lines = words.text.Split("\n");
        foreach(string line in lines)
        {
            dictionary.Add(line);
        }
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
            button.GetComponent<Image>().color = Color.grey;
            wordString = wordString.Substring(0, wordString.Length - 1);
            buttonsClicked.Remove(button);
        }
    }

    public void SubmitWord()
    {
        if (dictionary.Contains(wordString))
        {
           score += wordString.Length;
        }
        graph.OnRegen();
        wordString = "";
        scoreTextBox.text = "Score: " + score;
    }

    void Update()
    {
        time -= Time.deltaTime;
        timerTextBox.text = "Time: " + time;
        if (time <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        if (score == 0)
        {
            statChanges.smarts = -2;
        }
        else if (score < 10)
        {
            statChanges.smarts = -1;  
        }
        else if (score <= 20)
        {
            statChanges.smarts = 0;
        }
        else if (score <= 30)
        {
            statChanges.smarts = 1;
        }
        else if (score <= 40)
        {
            statChanges.smarts = 2;
        }
        else if (score <= 50)
        {
            statChanges.smarts = 3;
        }
        PlayerGlobalHandler.GlobalHandler.AddStats(statChanges);
        Debug.Log("Game Over");
    }
}
