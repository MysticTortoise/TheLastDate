using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "EmployeeProfile", menuName = "Employees/EmployeeProfile")]
public class EmployeeProfile : ScriptableObject
{
    public Sprite personImage;
    public Sprite profileImage;
    public string age;
    public string employeeName;
    public string position;
    public string yearsExperience;
    [TextArea]
    public string behaviorDescription;
    public int fireScore;
    public int maintainScore;
    public int moneySaved;
}