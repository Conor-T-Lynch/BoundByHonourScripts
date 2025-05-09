//@Reference https://github.com/antonelamatanovich/Dialogue-System
//@Reference https://gamedevbeginner.com/singletons-in-unity-the-right-way/
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1?view=net-8.0
//@Reference https://www.youtube.com/watch?v=_nRzoTzeyxU&ab_channel=Brackeys
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-9.0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    // Static reference to this instance so it can be accessed globally.
    public static DialogueManager Instance;

    // UI panel that displays the dialogue.
    public GameObject dialoguePanel;

    // Text component that shows the current line of dialogue.
    public TextMeshProUGUI dialogueText;

    // Queue to hold the sequence of dialogue lines to be displayed.
    private Queue<DialogueLine> dialogueLines = new Queue<DialogueLine>();

    // Optional callback action to be called when dialogue finishes.
    private System.Action onDialogueComplete;

    // Indicates whether a dialogue is currently active.
    private bool isDialogueActive = false;

    // Small delay to avoid accidental skipping of dialogue.
    private float inputCooldown = 0.2f;

    // Tracks how much time has passed since the dialogue started.
    private float timeSinceDialogueStarted = 0f;

    void Awake()
    {
        // Ensure only one instance of DialogueManager exists.
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Make sure the dialogue panel is initially hidden.
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // If a dialogue is active, track the time and check for input to continue.
        if (isDialogueActive)
        {
            timeSinceDialogueStarted += Time.deltaTime;

            // If the Y button (JoystickButton3) is pressed and cooldown has passed, show next line.
            if (Input.GetKeyDown(KeyCode.JoystickButton3) && timeSinceDialogueStarted >= inputCooldown)
            {
                NextLine();
            }
        }
    }

    // Starts the dialogue sequence with a list of dialogue lines and optional callback.
    public void StartDialogue(List<DialogueLine> lines, System.Action onComplete = null)
    {
        // Clear any previous dialogue.
        dialogueLines.Clear();

        // Enqueue each dialogue line.
        foreach (DialogueLine line in lines)
        {
            dialogueLines.Enqueue(line);
        }

        // Store the callback to call after the dialogue ends.
        onDialogueComplete = onComplete;

        // Show the dialogue UI and reset state.
        dialoguePanel.SetActive(true);
        isDialogueActive = true;
        timeSinceDialogueStarted = 0f;

        // Show the first line of dialogue.
        NextLine();
    }

    // Displays the next line of dialogue, or ends dialogue if no lines remain.
    public void NextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Dequeue the next line and show it in the dialogue panel.
        DialogueLine line = dialogueLines.Dequeue();
        dialogueText.text = line.speaker + ": " + line.line;
    }

    // Ends the dialogue, hides the UI panel, and triggers the completion callback.
    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        // Invoke the callback if it exists.
        onDialogueComplete?.Invoke(); 
    }

    // Returns true if a dialogue is currently active.
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    internal void StartDialogue(List<string> list, Action value)
    {
        throw new NotImplementedException();
    }
}
