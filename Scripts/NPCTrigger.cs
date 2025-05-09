//@Reference https://github.com/antonelamatanovich/Dialogue-System
//@Reference https://gamedevtraum.com/en/game-and-app-development-with-unity/unity-tutorials-and-solutions/unity-fundamental-series/6-ontriggerenter-and-ontriggerexit-in-unity-detect-objects/
//@Reference https://www.youtube.com/watch?v=bRcMVkJS3XQ&ab_channel=MoreBBlakeyyy
//@Reference https://docs.unity3d.com/2021.3/Documentation/Manual/collider-interactions-example-scripts.html
//@Reference https://www.youtube.com/watch?v=_nRzoTzeyxU&ab_channel=Brackeys
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-9.0
//@Reference https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Serializable.html
//@Reference https://docs.unity3d.com/6000.0/Documentation/Manual/script-serialization-rules.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    // Tag used to identify this NPC in the quest system
    public string npcTag = "Villager";

    // Predefined list of dialogue lines between the hero and the villager
    public List<DialogueLine> dialogueOptions = new List<DialogueLine>
    {
        new DialogueLine("Hero", "What has happened here?"),
        new DialogueLine("Villager", "One of our own betrayed us... they joined the bandits in the camp near town.")
    };

    // Tracks whether the player is in range of the NPC
    private bool playerInRange = false;

    // Ensures the dialogue only triggers once per interaction
    private bool hasSpoken = false;
    void Update()
    {
        // Check if player is in range, has not triggered the dialogue yet, and pressed the Y button
        if (playerInRange && !hasSpoken && Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            // Attempt to find the DialogueManager in the scene
            DialogueManager dm = FindObjectOfType<DialogueManager>();

            // If DialogueManager is found, begin the dialogue sequence
            if (dm != null)
            {
                // Prevents re-triggering
                hasSpoken = true;
                dm.StartDialogue(dialogueOptions, () =>
                {
                    // Register the conversation as a completed quest action when dialogue ends
                    QuestManager.Instance.RegisterTalk(npcTag);
                });
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Set flag to true when the player enters the NPC's trigger zone
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Reset flag when the player leaves the NPC's trigger zone
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}

[System.Serializable]
public class DialogueLine
{
    // Name of the speaker in the dialogue
    public string speaker;

    // The actual line of dialogue to display
    public string line;

    // Constructor for initializing dialogue lines
    public DialogueLine(string speaker, string line)
    {
        this.speaker = speaker;
        this.line = line;
    }
}
