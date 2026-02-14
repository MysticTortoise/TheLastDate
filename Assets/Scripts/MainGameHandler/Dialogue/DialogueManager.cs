
using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static readonly int OpenAnim = Animator.StringToHash("Open");
    private static readonly int OptionsAvailableAnim = Animator.StringToHash("OptionsAvailable");
    [CanBeNull] private DialogueSequence activeSequence;
    private int currentSequenceProgress;
    private bool inSequence => activeSequence != null;
    private bool dialogueBoxOpen = false;
    private bool typingFinished = false;
    private DialogueNode currentNode => activeSequence?.dialogueNodes[currentSequenceProgress];
    private bool hasOptions => currentNode.Options.Count > 0;
    
    private int currentMessageProgress;

    private TextMeshProUGUI speakerText;
    private TextMeshProUGUI dialogueText;

    private Animator animator;


    [SerializeField] [CanBeNull] private DialogueSequence TestDialogueSequence;
    private DialogueButton[] buttons;

    private void Start()
    {
        speakerText = transform.Find("DialogueBoxHeader").gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
        dialogueText = transform.Find("DialogueBox").gameObject.GetComponentInChildren<TextMeshProUGUI>(true);

        speakerText.text = "";
        dialogueText.text = "";

        buttons = GetComponentsInChildren<DialogueButton>();

        if (TestDialogueSequence != null)
        {
            SetActiveDialogueSequence(TestDialogueSequence);
        }

        animator = GetComponent<Animator>();
    }

    public void SetActiveDialogueSequence(DialogueSequence sequence)
    {
        if (!inSequence)
            dialogueBoxOpen = false;
        
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
        typingFinished = false;
    }

    private void FinishConversation()
    {
        activeSequence = null;
        dialogueBoxOpen = false;
    }
    
    public void AdvanceDialogue(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        if (!inSequence)
            return;
        if (hasOptions)
            return;
        
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
        animator.SetBool(OpenAnim, inSequence);

        if (!dialogueBoxOpen)
        {
            dialogueText.text = "";
        }
        
        if (!inSequence)
            return;
        
        speakerText.text = currentNode.Message.SpeakerTitle;
        dialogueText.text = currentNode.Message.Message[..currentMessageProgress];
    }

    public void DialogueBoxOpened()
    {
        dialogueBoxOpen = true;
    }

    private void TypingFinished()
    {
        animator.SetBool(OptionsAvailableAnim, hasOptions);
        typingFinished = true;

        if (hasOptions)
        {
            foreach (DialogueButton button in buttons)
            {
                int buttonIndex = button.OptionIndex;
                if (buttonIndex >= currentNode.Options.Count)
                {
                    button.gameObject.SetActive(false);
                    continue;
                }
                
                button.gameObject.SetActive(true);
                button.SetText(currentNode.Options[buttonIndex].Option);
            }
        }
    }

    public void ChooseOption(int option)
    {
        if (!hasOptions || !typingFinished)
            return;
        DialogueSequence sequence = currentNode.Options[option].Sequence;
        SetActiveDialogueSequence(sequence);
        animator.SetBool(OptionsAvailableAnim, false);
    }

    private float dialogueTimer;
    [SerializeField] private float dialogueWaitTime;
    private void Update()
    {
        if (inSequence && dialogueBoxOpen)
        {
            // Progress dialogue message;
            dialogueTimer += Time.deltaTime;
            float speakTime = dialogueWaitTime;
            
            while (dialogueTimer > speakTime && currentMessageProgress < currentNode.Message.GetLength())
            {
                dialogueTimer -= speakTime;
                currentMessageProgress++;

                if (currentMessageProgress >= currentNode.Message.GetLength())
                {
                    TypingFinished();
                }
            }
        }
        
        UpdateDisplay();
    }
}
