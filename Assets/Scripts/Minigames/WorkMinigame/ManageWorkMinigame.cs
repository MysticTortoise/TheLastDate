using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine; 
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
    private int employeeIndex;
    private int currKeepScore;
    private int currFireScore;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        employeeIndex = Random.Range(0, employees.Count);
        nameTextBox.text = employees[employeeIndex].employeeName;
        ageTextBox.text = employees[employeeIndex].age;
        positionTextBox.text = employees[employeeIndex].position;
        yearsExperienceTextBox.text = employees[employeeIndex].yearsExperience;
        descriptionTextBox.text = employees[employeeIndex].behaviorDescription;
        currKeepScore = employees[employeeIndex].maintainScore;
        currFireScore = employees[employeeIndex].fireScore;
        employees.RemoveAt(employeeIndex);
    }

    public void OnFire()
    {
        score += currFireScore;
        Debug.Log("Empathy Changed :" + currFireScore);
        scoreTextBox.text = "Score: " + score;
        animator.enabled = true;
    }

    public void OnKeep()
    {
        score += currKeepScore;
        Debug.Log("Empathy Changed :" + currKeepScore);
        scoreTextBox.text = "Score: " + score;
        animator.enabled = true;
    }

    public void ResetInfo()
    {
        if (employees.Count > 0)
        {
        employeeIndex = Random.Range(0, employees.Count);
        nameTextBox.text = employees[employeeIndex].employeeName;
        ageTextBox.text = employees[employeeIndex].age;
        positionTextBox.text = employees[employeeIndex].position;
        yearsExperienceTextBox.text = employees[employeeIndex].yearsExperience;
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
    }

    private void EndGame()
    {
        Debug.Log("Minigame Is Over");
    }
}
