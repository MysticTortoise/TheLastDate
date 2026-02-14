
using System;
using TMPro;
using UnityEngine;

public class DialogueButton : MonoBehaviour
{
    public int OptionIndex;

    private TextMeshProUGUI text;
    private DialogueManager dialogueManager;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();
    }

    public void SetText(string message)
    {
        text.text = message;
    }
    public void Clicked()
    {
        dialogueManager.ChooseOption(OptionIndex);
    }
}
