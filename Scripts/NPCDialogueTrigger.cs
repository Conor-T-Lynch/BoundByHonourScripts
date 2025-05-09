//@Reference https://github.com/antonelamatanovich/Dialogue-System
//@Reference https://gamedevtraum.com/en/game-and-app-development-with-unity/unity-tutorials-and-solutions/unity-fundamental-series/6-ontriggerenter-and-ontriggerexit-in-unity-detect-objects/
//@Reference https://www.youtube.com/watch?v=bRcMVkJS3XQ&ab_channel=MoreBBlakeyyy
//@Reference https://docs.unity3d.com/2021.3/Documentation/Manual/collider-interactions-example-scripts.html
//@Reference https://www.youtube.com/watch?v=_nRzoTzeyxU&ab_channel=Brackeys
//@Reference https://docs.unity3d.com/ScriptReference/Random.Range.html
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-9.0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    // List of random dialogue lines the NPC might say
    public List<DialogueLine> dialogueOptions = new List<DialogueLine>
    {
        new DialogueLine("Villager", "Please help her! These bandits came out of nowhere."),
        new DialogueLine("Villager", "You must stop the bandits before it’s too late!"),
        new DialogueLine("Villager", "The village is in danger. Please, we need your help!")
    };

    // Flag to ensure dialogue only triggers once per player interaction
    private bool hasTalked = false;

    // Flag to check if the player is within range of the NPC
    private bool playerInRange = false;

    // Time before the player can interact with the NPC again
    private float cooldownTime = 5f;

    // Called once per frame
    void Update()
    {
        // Check if the player is in range and presses the interaction button (Y button / JoystickButton3)
        if (playerInRange && !hasTalked && Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            // Set flag to prevent multiple dialogue triggers in the same interaction
            hasTalked = true;

            // Pick a random line from the list of dialogue options
            DialogueLine randomLine = dialogueOptions[Random.Range(0, dialogueOptions.Count)];

            // Create a list containing the single selected dialogue line
            List<DialogueLine> singleLineDialogue = new List<DialogueLine> { randomLine };

            // Trigger the dialogue system to show the selected line
            DialogueManager.Instance.StartDialogue(singleLineDialogue);

            // Start cooldown timer before the player can talk again
            StartCoroutine(ResetDialogueCooldown());
        }
    }

    // Coroutine that resets the dialogue cooldown after a set time
    private IEnumerator ResetDialogueCooldown()
    {
        // Wait for the specified cooldown time
        yield return new WaitForSeconds(cooldownTime);

        // Allow the player to interact again after cooldown
        hasTalked = false;
    }

    // Triggered when the player enters the NPC's trigger area
    void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger area
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // Triggered when the player exits the NPC's trigger area
    void OnTriggerExit(Collider other)
    {
        // Check if the player has exited the trigger area
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
