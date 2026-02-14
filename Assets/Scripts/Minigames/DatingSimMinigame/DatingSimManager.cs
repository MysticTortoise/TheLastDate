using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DatingSimManager : MonoBehaviour
{
    [SerializeField] private GameObject guyPanel;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private List<DatingSimQuestion> questions;
    [SerializeField] private List<Button> answerButtons;
    [SerializeField] private TextMeshProUGUI questionTextBox;
    private DatingSimMan guy;
    private DatingSimQuestion currentQuestion;
    private int score;

    void Start()
    {
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
        Debug.Log("Game Done");
    }
}
