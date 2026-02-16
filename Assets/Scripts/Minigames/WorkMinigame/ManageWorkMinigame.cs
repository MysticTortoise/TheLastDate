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
    [SerializeField] private TextMeshProUGUI moneyTextBox;
    private int empathyScore;
    private int employeeIndex;
    private int currKeepScore;
    private int currFireScore;
    private int currMoneyValue;
    private int totalMoneyValue;
    private StatBlock statsChanged;
    private Animator animator;
    private GoBackToGameScript backButton;
    private bool canClick;
    void Start()
    {
        canClick = true;
        statsChanged = new();
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
        totalMoneyValue = 0;
        backButton = FindAnyObjectByType<GoBackToGameScript>();
        employees.RemoveAt(employeeIndex);
    }

    public void OnFire()
    {
        if (canClick)
        {
            canClick =false;
            score += currFireScore;
            if (score >= 10)
            {
                score -= 10;
                statsChanged.empathy = 1;
                PlayerGlobalHandler.GlobalHandler.AddStats(statsChanged);
                statsChanged = new();
                empathyScore++;
            }
            if (score < 0)
            {
                score += 10;
                statsChanged.empathy = -1;
                PlayerGlobalHandler.GlobalHandler.AddStats(statsChanged);
                statsChanged = new();
                empathyScore--;
            }
            totalMoneyValue += currMoneyValue;
            statsChanged.money = currMoneyValue;
            PlayerGlobalHandler.GlobalHandler.AddStats(statsChanged);
            statsChanged = new();
            Debug.Log("Empathy Changed :" + currFireScore);
            scoreTextBox.text = "Change in Empathy: " + empathyScore;
            moneyTextBox.text = "Money Saved: $" + totalMoneyValue;
            animator.enabled = true;
        }
    }

    public void OnKeep()
    {
        if (canClick)
        {
            canClick = false;
            score += currKeepScore;
            if (score >= 10)
            {
                score -= 10;
                statsChanged.empathy = 1;
                PlayerGlobalHandler.GlobalHandler.AddStats(statsChanged);
                statsChanged = new();
                empathyScore++;
            }
            if (score < 0)
            {
                score += 10;
                statsChanged.empathy = -1;
                PlayerGlobalHandler.GlobalHandler.AddStats(statsChanged);
                statsChanged = new();
                empathyScore--;
            }
            scoreTextBox.text = "Change in Empathy: " + empathyScore;
            moneyTextBox.text = "Money Saved: $" + totalMoneyValue;
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
        else
        {
            backButton.GoBack();
        }
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
        canClick = true;
    }
}
