
using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [CanBeNull] private DialogueSequence activeSequence;
    private int currentSequenceProgress;
    private bool inSequence => activeSequence != null;
    private DialogueNode currentNode => activeSequence?.dialogueNodes[currentSequenceProgress];
    
    private int currentMessageProgress;

    private TextMeshProUGUI speakerText;
    private TextMeshProUGUI dialogueText;


    [SerializeField] [CanBeNull] private DialogueSequence TestDialogueSequence;


    private void Start()
    {
        speakerText = transform.Find("DialogueBoxHeader").gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
        dialogueText = transform.Find("DialogueBox").gameObject.GetComponentInChildren<TextMeshProUGUI>(true);

        speakerText.text = "";
        dialogueText.text = "";

        if (TestDialogueSequence != null)
        {
            SetActiveDialogueSequence(TestDialogueSequence);
        }
    }

    public void SetActiveDialogueSequence(DialogueSequence sequence)
    {
        activeSequence = sequence;
        currentSequenceProgress = 0;
        ShowDialogueMessage();
    }

    // Shows the actve dialogue message
    private void ShowDialogueMessage()
    {
        speakerText.text = currentNode.Message.SpeakerTitle;
        dialogueText.text = "";
        
        currentMessageProgress = 0;
        dialogueTimer = 0;
    }

    private void FinishConversation()
    {
        activeSequence = null;
    }
    
    public void AdvanceDialogue(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        if (!inSequence)
            return;

        if (!string.IsNullOrWhiteSpace(currentNode.OnFinishedMessage))
        {
            BroadcastMessage(currentNode.OnFinishedMessage);
        }
        
        currentSequenceProgress++;

        if (currentSequenceProgress >= activeSequence!.dialogueNodes.Count)
        {
            FinishConversation();
        }
        else
        {
            ShowDialogueMessage();
        }
    }
    

    private void UpdateDisplay()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(inSequence);
        }

        if (!inSequence)
            return;
        
        speakerText.text = currentNode.Message.SpeakerTitle;
        dialogueText.text = currentNode.Message.Message[..currentMessageProgress];
    }

    private float dialogueTimer;
    [SerializeField] private float dialogueWaitTime;
    private void Update()
    {
        if (inSequence)
        {
            // Progress dialogue message;
            dialogueTimer += Time.deltaTime;
            float speakTime = dialogueWaitTime;
            
            while (dialogueTimer > speakTime && currentMessageProgress < currentNode.Message.GetLength())
            {
                dialogueTimer -= speakTime;
                currentMessageProgress++;
            }
        }
        
        UpdateDisplay();
    }
}
