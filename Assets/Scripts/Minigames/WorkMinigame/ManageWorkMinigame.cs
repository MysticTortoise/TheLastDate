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
    public int employeeIndex;
    void Start()
    {
        employeeIndex = Random.Range(0, employees.Count);
        nameTextBox.text = employees[employeeIndex].employeeName;
        ageTextBox.text = employees[employeeIndex].age;
        positionTextBox.text = employees[employeeIndex].position;
        yearsExperienceTextBox.text = employees[employeeIndex].yearsExperience;
        descriptionTextBox.text = employees[employeeIndex].behaviorDescription;
    }

    public void OnFire()
    {
        if (employees.Count > 0)
        {
        employeeIndex = Random.Range(0, employees.Count);
        nameTextBox.text = employees[employeeIndex].employeeName;
        ageTextBox.text = employees[employeeIndex].age;
        positionTextBox.text = employees[employeeIndex].position;
        yearsExperienceTextBox.text = employees[employeeIndex].yearsExperience;
        descriptionTextBox.text = employees[employeeIndex].behaviorDescription;
        score += employees[employeeIndex].fireScore;
        Debug.Log("Empathy Lost" + employees[employeeIndex].fireScore);
        employees.RemoveAt(employeeIndex);
        }
        else {EndGame();}
    }

    public void OnKeep()
    {
        if (employees.Count > 0)
        {
        employeeIndex = Random.Range(0, employees.Count);
        nameTextBox.text = employees[employeeIndex].employeeName;
        ageTextBox.text = employees[employeeIndex].age;
        positionTextBox.text = employees[employeeIndex].position;
        yearsExperienceTextBox.text = employees[employeeIndex].yearsExperience;
        descriptionTextBox.text = employees[employeeIndex].behaviorDescription;
        score += employees[employeeIndex].maintainScore;
        Debug.Log("Empathy Gained" + employees[employeeIndex].maintainScore);
        employees.RemoveAt(employeeIndex);
        }
        else {EndGame();}
    }

    private void EndGame()
    {
        Debug.Log("Minigame Is Over");
    }
}
