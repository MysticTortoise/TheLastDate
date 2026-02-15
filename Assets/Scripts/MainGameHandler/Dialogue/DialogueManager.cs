
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class DialoguePersonData
{
    public float xPosition;
    public Sprite texture;
}

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
    private bool hasOptions => !inLiveMessage && currentNode.Options.Count > 0;
    
    private int currentMessageProgress;

    private TextMeshProUGUI speakerText;
    private TextMeshProUGUI dialogueText;

    private Animator animator;


    [SerializeField] [CanBeNull] private DialogueSequence TestDialogueSequence;
    private DialogueButton[] buttons;

    private Dictionary<int, DialoguePersonInstance> dialoguePeople = new();
    private GameObject dialoguePersonTemplate;

    public static DialogueManager dialogueManager;

    public bool isDialogueUp => inSequence || dialogueBoxOpen;
    public bool blockInput => isDialogueUp && !inLiveMessage;

    [CanBeNull] private string liveMessage = null;
    private string liveSpeaker;
    public bool inLiveMessage => liveMessage != null;

    private string GetMessage()
    {
        if (inLiveMessage)
            return liveMessage;
        if (inSequence)
            return currentNode.Message.Message;
        
        return "";
    }

    private void Start()
    {
        dialogueText = transform.Find("DialogueBox").Find("Text").GetComponent<TextMeshProUGUI>();
        speakerText = transform.Find("DialogueBox").Find("DialogueBoxHeader").gameObject.GetComponentInChildren<TextMeshProUGUI>(true);

        speakerText.text = "";
        dialogueText.text = "";

        buttons = GetComponentsInChildren<DialogueButton>();

        if (TestDialogueSequence != null)
        {
            SetActiveDialogueSequence(TestDialogueSequence);
        }

        animator = GetComponent<Animator>();
        dialogueManager = this;

        dialoguePersonTemplate = transform.parent.Find("DialoguePeople").GetChild(0).gameObject;
    }

    public void SetDialoguePersonData(int id, DialoguePersonData data)
    {
        if (data.texture == null)
        {
            dialoguePeople[id].Disappear();
            dialoguePeople.Remove(id);
            return;
        }

        if (!dialoguePeople.ContainsKey(id))
        {
            dialoguePeople[id] = Instantiate(dialoguePersonTemplate, dialoguePersonTemplate.transform.parent).GetComponent<DialoguePersonInstance>();
        }

        var person = dialoguePeople[id];
        person.gameObject.SetActive(true);
        person.targetXPosition = data.xPosition;
        person.GetComponent<Image>().sprite = data.texture;
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

        foreach (DialogueActionBase action in currentNode.DialogueActionsStart)
            action.Trigger();

        if (string.IsNullOrWhiteSpace(currentNode.Message.Message))
        {
            FinishConversation();
        }
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

        var oldSeq = activeSequence;
        
        foreach (DialogueActionBase action in currentNode.DialogueActionsEnd)
            action.Trigger();

        if (oldSeq != activeSequence)
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
        animator.SetBool(OpenAnim, inSequence || inLiveMessage);

        if (!dialogueBoxOpen)
        {
            dialogueText.text = "";
        }
        
        if (!inSequence && !inLiveMessage)
            return;
        
        speakerText.text = inLiveMessage? liveSpeaker : currentNode.Message.SpeakerTitle;
        dialogueText.text = GetMessage()[..currentMessageProgress];
    }

    public void DialogueBoxOpened()
    {
        if(inSequence || inLiveMessage)
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
                button.GetComponent<Button>().interactable = currentNode.Options[buttonIndex].RequirementsMet();
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
        if ((inSequence || inLiveMessage) && dialogueBoxOpen)
        {
            // Progress dialogue message;
            dialogueTimer += Time.deltaTime;
            float speakTime = dialogueWaitTime;
            
            while (dialogueTimer > speakTime && currentMessageProgress < GetMessage().Length)
            {
                dialogueTimer -= speakTime;
                currentMessageProgress++;

                if (currentMessageProgress >= GetMessage().Length)
                {
                    TypingFinished();
                }
            }
        }
        
        UpdateDisplay();
    }

    public void SetLiveMessage([CanBeNull] string message, string speaker = "")
    {
        if (liveMessage != message)
        {
            currentMessageProgress = 0;
            dialogueTimer = 0;
        }
        liveMessage = message;
        liveSpeaker = speaker;
    }
}
