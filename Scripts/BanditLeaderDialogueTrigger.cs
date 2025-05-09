//@Reference https://github.com/antonelamatanovich/Dialogue-System
//@Reference https://gamedevtraum.com/en/game-and-app-development-with-unity/unity-tutorials-and-solutions/unity-fundamental-series/6-ontriggerenter-and-ontriggerexit-in-unity-detect-objects/
//@Reference https://www.youtube.com/watch?v=bRcMVkJS3XQ&ab_channel=MoreBBlakeyyy
//@Reference https://docs.unity3d.com/2021.3/Documentation/Manual/collider-interactions-example-scripts.html
//@Reference https://www.youtube.com/watch?v=_nRzoTzeyxU&ab_channel=Brackeys
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-9.0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditLeaderDialogueTrigger : MonoBehaviour
{
    // A list of dialogue lines representing the conversation between the Bandit Leader and the Hero.
    public List<DialogueLine> dialogueLines = new List<DialogueLine>
    {
        new DialogueLine("Bandit Leader", "You should have never come here, hero! This is our turf!"),
        new DialogueLine("Hero", "I won't let you destroy this village."),
        new DialogueLine("Bandit Leader", "Then you die here, fool!")
    };

    // Flag to ensure the dialogue is triggered only once.
    private bool hasTalked = false;

    // Indicates whether the player is within the trigger area.
    private bool playerInRange = false;

    // Reference to the EnemyAIController component to control enemy behavior.
    private EnemyAIController enemyAI;

    void Start()
    {
        // Obtain the EnemyAIController component attached to this GameObject.
        enemyAI = GetComponent<EnemyAIController>();
    }

    void Update()
    {
        // Check if the player is in range, the dialogue hasn't been triggered yet,
        // and the player presses the 'Y' button (JoystickButton3).
        if (playerInRange && !hasTalked && Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            // Set the flag to true to prevent re-triggering the dialogue.
            hasTalked = true;

            // Disable the enemy AI to prevent movement or attacks during the dialogue.
            enemyAI.DisableAI();

            // Initiate the dialogue sequence using the DialogueManager.
            DialogueManager.Instance.StartDialogue(dialogueLines, () =>
            {
                // Callback function executed after the dialogue ends.
                // Re-enable the enemy AI to resume normal behavior.
                enemyAI.EnableAI();
            });
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // When another collider enters the trigger area, check if it's the player.
        if (other.CompareTag("Player"))
        {
            // Set the flag to true to indicate the player is within range.
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // When another collider exits the trigger area, check if it's the player.
        if (other.CompareTag("Player"))
        {
            // Set the flag to false to indicate the player has left the range.
            playerInRange = false;
        }
    }
}
