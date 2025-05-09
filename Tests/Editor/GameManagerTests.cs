//@Reference https://docs.nunit.org/articles/nunit/intro.html
//@Reference https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.reflection?view=net-6.0
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.html
//@Reference https://letsmakeagame.net/unity-unit-testing-basics-tutorial
//@Reference https://www.youtube.com/watch?v=dXAQ3L3dySc

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using static GameManager;
using System.Linq;

public class GameManagerTests
{
    private GameObject testGameObject;
    private GameManager gameManager;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and add the GameManager component
        testGameObject = new GameObject("GameManager");

        // Assign the GameManager component to the test GameObject
        gameManager = testGameObject.AddComponent<GameManager>();

        // Create a mock FeedbackText object for testing
        GameObject feedbackTextObject = new GameObject("FeedbackText");

        // Add TextMeshProUGUI component (a concrete subclass of TMP_Text) to simulate UI text
        feedbackTextObject.AddComponent<TextMeshProUGUI>();

        // Assign the feedbackText to GameManager for testing purposes
        gameManager.feedbackText = feedbackTextObject.GetComponent<TextMeshProUGUI>();

        // Simulate a Player GameObject for the test scenario
        GameObject player = new GameObject("Player");

        // Ensure the player object has the "Player" tag
        player.tag = "Player";

        // Add a Rigidbody component to simulate the player object
        player.AddComponent<Rigidbody>();

        // Ensure that the QuestManager singleton is initialized
        if (QuestManager.Instance == null)
        {
            // Create and initialize QuestManager if it doesn't exist
            new GameObject("QuestManager").AddComponent<QuestManager>();
        }
    }

    [Test]
    public void QuestManager_SingletonIsAssigned()
    {
        // Arrange: Create and initialize GameManager and QuestManager
        GameManager gameManager = new GameObject("GameManager").AddComponent<GameManager>();
        QuestManager questManager = new GameObject("QuestManager").AddComponent<QuestManager>();

        // Manually assign the singleton instance of QuestManager
        QuestManager.Instance = questManager;

        // Assert: Verify that the singleton instance is correctly assigned
        Assert.AreEqual(questManager, QuestManager.Instance, "QuestManager singleton instance should be assigned correctly.");
    }

    [Test]
    public void SaveAndLoadGame_WorksCorrectly()
    {
        // Arrange: Create and initialize a new GameManager and QuestManager
        GameManager gameManager = new GameObject("GameManager").AddComponent<GameManager>();
        QuestManager questManager = new GameObject("QuestManager").AddComponent<QuestManager>();

        // Create a new quest
        Quest quest = new Quest();

        // Set the name of the quest
        quest.questName = "Test Quest";

        // Set the description for the quest
        quest.description = "Test quest for saving and loading";

        // Set the quest as incomplete initially
        quest.isCompleted = false;

        // Add an objective to the quest
        quest.objectives.Add(new Quest.Objective
        {
            // Set the objective type to "Kill"
            type = Quest.ObjectiveType.Kill,

            // Set the target tag for the objective
            targetTag = "TestEnemy",

            // Set the required kills for completion
            requiredAmount = 2,

            // Set initial progress to 0
            currentAmount = 0
        });

        // Add the quest to QuestManager
        questManager.AddQuest(quest);

        // Simulate a kill to advance quest progress (but not enough to complete)
        questManager.RegisterKill("TestEnemy");

        // Assert: Verify the quest progress is correctly updated
        Assert.AreEqual(1, quest.objectives[0].currentAmount, "Quest should have progress after killing an enemy.");

        // Now save the game state using the GameManager
        gameManager.SaveGame();

        // Act: Load the game state to verify if the progress persists
        gameManager.LoadGame();

        // Assert: Check if the quest progress was correctly restored after loading
        Assert.AreEqual(1, quest.objectives[0].currentAmount, "Quest progress should be restored after loading the game.");
        Assert.IsFalse(quest.isCompleted, "Quest should not be completed after loading the game.");
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the GameManager test object after tests
        if (testGameObject != null)
        {
            // Destroy the GameManager test object
            Object.DestroyImmediate(testGameObject);  
        }

        // Clean up save files if they exist
        // Get the path to the save file
        string path = gameManager.saveFilePath;  

        // Check if save file exists
        if (File.Exists(path))
        {
            // Delete the save file to clean up
            File.Delete(path);
        }
    }
}
