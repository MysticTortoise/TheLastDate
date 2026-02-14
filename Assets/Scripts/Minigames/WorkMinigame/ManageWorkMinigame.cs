using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ManageWorkMinigame : MonoBehaviour
{
    [SerializeField] private List<EmployeeProfile> employees;
    private int employeeIndex;
    private int score;
    [SerializeField] private TextMeshProUGUI nameTextBox;
    [SerializeField] private TextMeshProUGUI ageTextBox;
    [SerializeField] private TextMeshProUGUI positionTextBox;
    [SerializeField] private TextMeshProUGUI yearsExperienceTextBox;
    [SerializeField] private TextMeshProUGUI descriptionTextBox;
    void Start()
    {
        employees = new();
        employeeIndex = 0;
        nameTextBox.text = employees[0].employeeName;
        ageTextBox.text = employees[0].age;
        positionTextBox.text = employees[0].position;
        yearsExperienceTextBox.text = employees[0].yearsExperience;
        descriptionTextBox.text = employees[0].behaviorDescription;
    }

    public void OnFire()
    {
        score += employees[employeeIndex].fireScore;
        Debug.Log("Empathy Lost" + employees[employeeIndex].fireScore);
        employeeIndex++;
        if (employeeIndex < employees.Count)
        {
        nameTextBox.text = employees[employeeIndex].employeeName;
        ageTextBox.text = employees[employeeIndex].age;
        positionTextBox.text = employees[employeeIndex].position;
        yearsExperienceTextBox.text = employees[employeeIndex].yearsExperience;
        descriptionTextBox.text = employees[employeeIndex].behaviorDescription;
        }
        else {EndGame();}
    }

    public void OnKeep()
    {
        score += employees[employeeIndex].maintainScore;
        Debug.Log("Empathy Gained" + employees[employeeIndex].fireScore);
        employeeIndex++;
        if (employeeIndex < employees.Count)
        {
        nameTextBox.text = employees[employeeIndex].employeeName;
        ageTextBox.text = employees[employeeIndex].age;
        positionTextBox.text = employees[employeeIndex].position;
        yearsExperienceTextBox.text = employees[employeeIndex].yearsExperience;
        descriptionTextBox.text = employees[employeeIndex].behaviorDescription;
        }
        else {EndGame();}
    }

    private void EndGame()
    {
        Debug.Log("Minigame Is Over");
    }
}
