//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-9.0
//@Reference https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.
//@Reference https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
//@Reference https://docs.unity3d.com/ScriptReference/GameObject.html
//@Reference https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/classes
//@Reference https://www.geeksforgeeks.org/singleton-design-pattern/
//@Reference https://docs.unity3d.com/Manual/coroutines.html
//@Reference https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
//@Reference https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
//@Reference https://www.youtube.com/watch?v=7GD5D1viVtc&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=4oCc0btj_ys&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=J50L85WUtnw&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=3&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=JEi3wHZfbNA&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=4&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=bxCdjDceBbw&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=5&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=a8y6Ul-nX9o&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=6&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=-X72WioCsPg&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=7&ab_channel=xOctoManx

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    // Singleton instance so the QuestManager can be accessed globally
    public static QuestManager Instance;

    // List of all quests in the game
    public List<Quest> quests = new List<Quest>();

    // Tracks the index of the current quest
    private int currentQuestIndex = 0;

    // Singleton setup: ensure only one QuestManager exists
    public void Awake()
    {
        if (Instance == null) Instance = this;
        // Destroy duplicate
        else Destroy(gameObject); 
    }

    // Adds a new quest to the quest list
    public void AddQuest(Quest quest)
    {
        quests.Add(quest);
    }

    // Gets the currently active quest
    public Quest GetCurrentQuest()
    {
        if (currentQuestIndex < quests.Count)
            return quests[currentQuestIndex];
        return null;
    }

    // Advances to the next quest, or ends the game if all quests are completed
    public void AdvanceQuest()
    {
        // Increment the index to move to the next quest
        currentQuestIndex++;

        // Check if the current quest index exceeds the number of available quests
        if (currentQuestIndex >= quests.Count)
        {
            // Log that all quests are completed
            Debug.Log("All quests completed!");

            // Load the Game Over scene since all quests are completed
            LoadGameOverScene();
            // Exit the method as there are no more quests to process
            return;  
        }

        // Log the advancement to the next quest
        Debug.Log("Quest advanced! Current Quest Index: " + currentQuestIndex);

        // Get the next quest based on the updated quest index
        Quest nextQuest = GetCurrentQuest();

        // Check if a valid next quest is found
        if (nextQuest != null)
        {
            // Log the name of the now active quest
            Debug.Log("Now active: " + nextQuest.questName);
        }
    }

    // Called when an enemy is killed; checks if it completes a Kill objective
    public void RegisterKill(string enemyTag)
    {
        // Get the current quest to check its progress
        var quest = GetCurrentQuest();

        // Check if the quest is null or already completed, in which case no further action is needed
        if (quest == null || quest.isCompleted) return;

        // Loop through each objective in the current quest
        foreach (var obj in quest.objectives)
        {
            // Check if the objective type is 'Kill' and if the target matches the enemy's tag
            if (obj.type == Quest.ObjectiveType.Kill && obj.targetTag == enemyTag)
            {
                // Increment the progress of the 'Kill' objective
                obj.currentAmount++;

                // Log the updated kill progress for the specific enemy tag
                Debug.Log($"Kill registered for tag {enemyTag}: {obj.currentAmount}/{obj.requiredAmount}");

                // Check if the quest is completed after updating the objective progress
                if (quest.CheckCompletion())
                {
                    // Log the completion of the quest
                    Debug.Log($"Quest '{quest.questName}' completed!");

                    // Advance to the next quest since this one is completed
                    AdvanceQuest();
                }

                // Exit the method as the quest progression has been handled
                return;
            }
        }
    }

    // Called when talking to an NPC; checks if it completes a Talk objective
    public void RegisterTalk(string targetTag)
    {
        // Get the current quest to check its progress
        var quest = GetCurrentQuest();

        // Check if the quest is null or already completed, in which case no further action is needed
        if (quest == null || quest.isCompleted) return;

        // Loop through each objective in the current quest
        foreach (var obj in quest.objectives)
        {
            // Check if the objective type is 'Talk' and if the target matches the specified tag
            if (obj.type == Quest.ObjectiveType.Talk && obj.targetTag == targetTag)
            {
                // Immediately complete the 'Talk' objective by setting currentAmount to requiredAmount
                obj.currentAmount = obj.requiredAmount;

                // Log that the player has talked to the target
                Debug.Log($"Talked to {targetTag}");

                // Check if the quest is completed after updating the objective progress
                if (quest.CheckCompletion())
                {
                    // Log the completion of the quest
                    Debug.Log($"Quest '{quest.questName}' completed!");

                    // Advance to the next quest since this one is completed
                    AdvanceQuest();
                }

                // Exit the method as the quest progression has been handled
                return;
            }
        }
    }

    // Called when the player enters a specific area
    public void RegisterAreaEntry(string targetTag)
    {
        // Get the current quest to check its progress
        var quest = GetCurrentQuest();

        // Check if the quest is null or already completed, in which case no further action is needed
        if (quest == null || quest.isCompleted) return;

        // Loop through each objective in the current quest
        foreach (var obj in quest.objectives)
        {
            // Check if the objective type is 'EnterArea' and if the target matches the specified tag
            if (obj.type == Quest.ObjectiveType.EnterArea && obj.targetTag == targetTag)
            {
                // Immediately complete the 'EnterArea' objective by setting currentAmount to requiredAmount
                obj.currentAmount = obj.requiredAmount;

                // Log that the player has entered the specified area
                Debug.Log($"Entered area: {targetTag}");

                // Check if the quest is completed after updating the objective progress
                if (quest.CheckCompletion())
                {
                    // Log the completion of the quest
                    Debug.Log($"Quest '{quest.questName}' completed!");

                    // Advance to the next quest since this one is completed
                    AdvanceQuest();
                }

                // Exit the method as the quest progression has been handled
                return;
            }
        }
    }

    // Starts the coroutine to load the game over scene after a delay
    public void LoadGameOverScene()
    {
        StartCoroutine(DelayedGameOverScene());
    }

    // Waits for a few seconds before loading the "GameOver" scene
    private IEnumerator DelayedGameOverScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameOver");
    }

    // Property to get the current quest index (for saving)
    public int CurrentQuestIndex => currentQuestIndex;

    // Loads the quest progress from saved data
    public void LoadQuestProgress(int questIndex, List<GameManager.GameData.QuestProgressData> progressData)
    {
        // Set the current quest index to the provided quest index from the saved game data
        currentQuestIndex = questIndex;

        // Loop through the progress data and quests to restore the progress of each quest
        for (int i = 0; i < progressData.Count && i < quests.Count; i++)
        {
            // Get the current quest and its associated saved progress data
            var quest = quests[i];
            var questData = progressData[i];

            // Restore whether the quest is completed based on saved data
            quest.isCompleted = questData.isCompleted;

            // Loop through each objective in the quest to restore its progress
            for (int j = 0; j < questData.objectiveProgress.Count && j < quest.objectives.Count; j++)
            {
                // Restore the current amount of each objective based on saved data
                quest.objectives[j].currentAmount = questData.objectiveProgress[j];
            }
        }

        // Log that the quest progress has been successfully restored
        Debug.Log("Quest progress restored.");

        }
    }
