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

public class TraitorDialogueTrigger : MonoBehaviour
{
    // A list of dialogue lines between the traitor and the hero
    public List<DialogueLine> dialogueLines = new List<DialogueLine>
    {
        new DialogueLine("Traitor", "Fool. The villagers are spineless. I did what had to be done."),
        new DialogueLine("Hero", "You won’t get away with this.")
    };

    // Ensures dialogue only plays once
    private bool hasTalked = false;
    // Tracks whether the player is in range to trigger dialogue
    private bool playerInRange = false;
    // Reference to the enemy's AI controller
    private EnemyAIController enemyAI;   

    void Start()
    {
        // Get a reference to the attached EnemyAIController component
        enemyAI = GetComponent<EnemyAIController>();
    }

    void Update()
    {
        // If the player is in range and hasn't triggered the dialogue yet, need to press y to trigger dialogue
        if (playerInRange && !hasTalked && Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            hasTalked = true;

            // Temporarily disable enemy AI during dialogue
            enemyAI.DisableAI();

            // Start the dialogue sequence using DialogueManager
            DialogueManager.Instance.StartDialogue(dialogueLines, () =>
            {
                // Callback after the dialogue is finished
                // Re-enable the enemy's AI so it becomes hostile again
                enemyAI.EnableAI();
            });
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // When the player enters the trigger collider, set the in-range flag
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // When the player exits the trigger collider, clear the in-range flag
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
