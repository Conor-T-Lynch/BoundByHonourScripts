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

public class EnemyDialogueTrigger : MonoBehaviour
{
    // A list of possible one-liner dialogues that an enemy might say.
    public List<DialogueLine> dialogueOptions = new List<DialogueLine>
    {
        new DialogueLine("Enemy", "Hey hero, this is our town now."),
        new DialogueLine("Enemy", "You best leave if you know what’s good for you."),
        new DialogueLine("Enemy", "You should have stayed away, hero.")
    };

    // Ensures the enemy only speaks once per interaction.
    private bool hasTalked = false;

    // Keeps track of whether the player is in the interaction zone.
    private bool playerInRange = false;

    // Reference to the enemy AI controller script.
    private EnemyAIController enemyAI;

    void Start()
    {
        // Get the EnemyAIController component attached to this enemy.
        enemyAI = GetComponent<EnemyAIController>();
    }

    void Update()
    {
        // Checks if the player is in range, the enemy hasn't spoken yet,
        // and the player presses the Y button (JoystickButton3).
        if (playerInRange && !hasTalked && Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            hasTalked = true;

            // Temporarily disable the enemy's AI behavior while speaking.
            enemyAI.DisableAI();

            // Pick a random line from the list of enemy dialogue options.
            DialogueLine randomLine = dialogueOptions[Random.Range(0, dialogueOptions.Count)];

            // Wrap the chosen line in a list for compatibility with DialogueManager.
            List<DialogueLine> singleLineDialogue = new List<DialogueLine> { randomLine };

            // Start the dialogue using the DialogueManager, then re-enable AI afterward.
            DialogueManager.Instance.StartDialogue(singleLineDialogue, () =>
            {
                // Callback to re-enable the enemy's AI after the dialogue finishes.
                enemyAI.EnableAI();
            });
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // When the player enters the enemy's trigger zone, mark them as in range.
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // When the player exits the trigger zone, mark them as out of range.
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
