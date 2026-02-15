using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
public class ManageWorkMinigame : MonoBehaviour
{
    [SerializeField] private List<EmployeeProfile> employees;
    private int score;
    [SerializeField] private TextMeshProUGUI nameTextBox;
    [SerializeField] private TextMeshProUGUI ageTextBox;
    [SerializeField] private TextMeshProUGUI positionTextBox;
    [SerializeField] private TextMeshProUGUI yearsExperienceTextBox;
    [SerializeField] private TextMeshProUGUI descriptionTextBox;
    [SerializeField] private TextMeshProUGUI scoreTextBox;
    private int moneySaved;
    private int employeeIndex;
    private int currKeepScore;
    private int currFireScore;
    private int currMoneyValue;
    private StatBlock statsChanged;
    private Animator animator;
    public bool canClick;
    void Start()
    {
        canClick = true;
        statsChanged = new();
        moneySaved = 0;
        animator = GetComponent<Animator>();
        employeeIndex = Random.Range(0, employees.Count);
        nameTextBox.text = employees[employeeIndex].employeeName;
        ageTextBox.text = employees[employeeIndex].age;
        positionTextBox.text = employees[employeeIndex].position;
        yearsExperienceTextBox.text = employees[employeeIndex].yearsExperience + " Years Experience";
        descriptionTextBox.text = employees[employeeIndex].behaviorDescription;
        currKeepScore = employees[employeeIndex].maintainScore;
        currFireScore = employees[employeeIndex].fireScore;
        currMoneyValue = employees[employeeIndex].moneySaved;
        employees.RemoveAt(employeeIndex);
    }

    public void OnFire()
    {
        if (canClick)
        {
            canClick =false;
            score += currFireScore;
            moneySaved += currMoneyValue;
            Debug.Log("Empathy Changed :" + currFireScore);
            scoreTextBox.text = "Score: " + score;
            animator.enabled = true;
        }
    }

    public void OnKeep()
    {
        if (canClick)
        {
            canClick = false;
            score += currKeepScore;
            Debug.Log("Empathy Changed :" + currKeepScore);
            scoreTextBox.text = "Score: " + score;
            animator.enabled = true;
        }
    }

    public void ResetInfo()
    {
        if (employees.Count > 0)
        {
        employeeIndex = Random.Range(0, employees.Count);
        nameTextBox.text = employees[employeeIndex].employeeName;
        ageTextBox.text = employees[employeeIndex].age;
        positionTextBox.text = employees[employeeIndex].position;
        yearsExperienceTextBox.text = employees[employeeIndex].yearsExperience + " Years Experience";
        descriptionTextBox.text = employees[employeeIndex].behaviorDescription;
        currKeepScore = employees[employeeIndex].maintainScore;
        currFireScore = employees[employeeIndex].fireScore;
        employees.RemoveAt(employeeIndex);
        }
        else {EndGame();}
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
        canClick = true;
    }

    private void EndGame()
    {
        if (score <= -30)
        {
            statsChanged.empathy = -3;
            scoreTextBox.fontSize = 0.4f;
            scoreTextBox.text = "You Lost 3 Empathy and Made $" + moneySaved;
        }
        else if (score <= -20)
        {
            statsChanged.empathy = -2;
            scoreTextBox.fontSize = 0.4f;
            scoreTextBox.text = "You Lost 2 Empathy and Made $" + moneySaved;
        }
        else if (score < -10)
        {
            statsChanged.empathy = -1;
            scoreTextBox.fontSize = 0.4f;
            scoreTextBox.text = "You Lost 1 Empathy and Made $" + moneySaved;  
        }
        else if (score <= 0)
        {
            statsChanged.empathy = 0;
            scoreTextBox.fontSize = 0.4f;
            scoreTextBox.text = "You Gained no Empathy and Made $" + moneySaved;
        }
        else if (score <= 10)
        {
            statsChanged.empathy = 1;
            scoreTextBox.fontSize = 0.4f;
            scoreTextBox.text = "You Gained 1 Empathy and Made $" + moneySaved;
        }
        else if (score <= 20)
        {
            statsChanged.empathy = 2;
            scoreTextBox.fontSize = 0.4f;
            scoreTextBox.text = "You Gained 2 Empathy and Made $" + moneySaved;
        }
        else if (score <= 30)
        {
            statsChanged.empathy = 3;
            scoreTextBox.fontSize = 0.4f;
            scoreTextBox.text = "You Gained 3 Empathy and Made $" + moneySaved;
        }
        statsChanged.money = moneySaved;
        PlayerGlobalHandler.GlobalHandler.AddStats(statsChanged);
        Debug.Log("Minigame Is Over");
    }
}
