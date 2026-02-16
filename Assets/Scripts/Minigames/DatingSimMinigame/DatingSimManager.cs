using System.Collections;
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
    [SerializeField] private GameObject winPanel;
    [SerializeField] private List<DatingSimQuestion> questions;
    [SerializeField] private List<Button> answerButtons;
    [SerializeField] private TextMeshProUGUI questionTextBox;
    [SerializeField] private TextMeshProUGUI scoreTextBox;
    private StatBlock statChanges;
    private DatingSimMan guy;
    private DatingSimQuestion currentQuestion;
    private int score;

    [SerializeField] private GameObject GoBackScene;

    void Start()
    {
        statChanges = new();
        guyPanel.SetActive(true);
        questionPanel.SetActive(false);
    }
    
    public void OnChooseBF(DatingSimMan man)
    {
        guy = man;
        guyPanel.SetActive(false);
        questionPanel.SetActive(true);
        winPanel.SetActive(false);

        questionPanel.transform.Find("GuyImage").GetComponent<Image>().sprite = man.image;
        
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
        scoreTextBox.text = "Score: " + score;
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

    private IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(5);

        PlayerGlobalHandler.LoadIntoMainGame(GoBackScene);
        yield return null;
    }

    private void EndGame()
    {
        guyPanel.SetActive(false);
        winPanel.SetActive(true);

        winPanel.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "Final Score:" + score;

        if (score > 0)
        {
            winPanel.transform.Find("GuyImage").GetComponent<Image>().sprite = guy.blushImage;
        }
        else
        {
            winPanel.transform.Find("GuyImage").GetComponent<Image>().sprite = guy.image;
        }
        
        statChanges.rizz = (score / 2);
        PlayerGlobalHandler.GlobalHandler.AddStats(statChanges);
        StartCoroutine(EndGameCoroutine());
    }
}
