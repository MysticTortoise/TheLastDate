using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class DatingSimManager : MonoBehaviour
{
    [SerializeField] private GameObject guyPanel;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private List<DatingSimQuestion> questions;
    [SerializeField] private List<Button> answerButtons;
    [SerializeField] private TextMeshProUGUI questionTextBox;
    private StatBlock statChanges;
    private DatingSimMan guy;
    private DatingSimQuestion currentQuestion;
    private int score;
    private PlayerGlobalHandler globalHandler;

    void Start()
    {
        globalHandler = FindAnyObjectByType<PlayerGlobalHandler>();
        statChanges = new();
        guyPanel.SetActive(true);
        questionPanel.SetActive(false);
    }
    
    public void OnChooseBF(DatingSimMan man)
    {
        guy = man;
        guyPanel.SetActive(false);
        questionPanel.SetActive(true);
        
        int questionIndex = Random.Range(0, questions.Count);
        currentQuestion = questions[questionIndex];
        questionTextBox.text = currentQuestion.question;
        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
        }
    }

    public void AnswerQuestion(int answerIndex)
    {
        if (currentQuestion.questionPersonalities[answerIndex] == guy.type) {score++;}
        else {score--;}
        questions.Remove(currentQuestion);
        Debug.Log(score);

        if (questions.Count > 0)
        {
            int questionIndex = Random.Range(0, questions.Count);
            currentQuestion = questions[questionIndex];
            questionTextBox.text = questions[questionIndex].question;
            for (int i = 0; i < answerButtons.Count; i++)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = questions[questionIndex].answers[i];
            }
        }
        else EndGame();
    }

    private void EndGame()
    {
        switch (score)
        {
        case (<= -2):
            statChanges.rizz = -3;
            break;
        case (<= 0):
            statChanges.rizz = -2;
            break;
        case (<=2):
            statChanges.rizz = -1;
            break;
        case (<=4):
            statChanges.rizz = 0;
            break;
        case(<=6):
            statChanges.rizz = 1;
            break;
        case (<= 8):
            statChanges.rizz = 2;
            break;
        case (<= 10):
            statChanges.rizz = 3;
            break;
        }
        globalHandler.AddStats(statChanges);
        Debug.Log("Game Done");
    }
}
