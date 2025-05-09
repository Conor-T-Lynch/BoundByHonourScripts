//@Reference https://docs.nunit.org/articles/nunit/intro.html
//@Reference https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.reflection?view=net-6.0
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.html
//@Reference https://letsmakeagame.net/unity-unit-testing-basics-tutorial
//@Reference https://www.youtube.com/watch?v=dXAQ3L3dySc

using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class EnemyAIControllerTests
{
    // Declare a GameObject to represent the enemy
    GameObject enemyObject;

    // Declare a reference to the EnemyAIController script
    EnemyAIController enemyAI;

    // Setup method that runs before each test
    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject for the enemy and attach the EnemyAIController
        enemyObject = new GameObject("Enemy");
        enemyAI = enemyObject.AddComponent<EnemyAIController>();

        // Create a GameObject for the player and assign it to the enemyAI's player reference
        GameObject playerObject = new GameObject("Player");
        enemyAI.player = playerObject.transform;

        // Manually call Start() method to simulate the scene setup
        enemyAI.Start();
    }

    // TearDown method that runs after each test to clean up the environment
    [TearDown]
    public void TearDown()
    {
        // Destroy the player object and the enemy object after each test
        Object.DestroyImmediate(enemyAI.player.gameObject);
        Object.DestroyImmediate(enemyObject);
    }

    // Test case to ensure taking damage reduces enemy health
    [Test]
    public void TakeDamage_ReducesHealth()
    {
        // Apply 100 damage to the enemy
        enemyAI.TakeDamage(100);

        // Assert that the enemy is not dead
        Assert.IsFalse(IsEnemyDead(enemyAI));
    }

    // UnityTest case to ensure the enemy dies when health reaches zero
    [UnityTest]
    public IEnumerator TakeDamage_KillsEnemy_WhenHealthReachesZero()
    {
        // Silence expected error and log messages during the test
        LogAssert.Expect(LogType.Log, "Enemy takes 300 damage! Current Health: -50");
        LogAssert.Expect(LogType.Log, "Enemy died!");
        LogAssert.Expect(LogType.Warning, "Animator not assigned on EnemyAIController.");
        LogAssert.Expect(LogType.Error, "QuestManager is not initialized. Please check your scene.");
        LogAssert.Expect(LogType.Error, "Destroy may not be called from edit mode! Use DestroyImmediate instead.\nDestroying an object in edit mode destroys it permanently.");

        // Apply 300 damage to kill the enemy
        enemyAI.TakeDamage(300);

        // Wait for a frame to ensure the state has updated
        yield return null;

        // Assert that the enemy is dead after receiving enough damage
        Assert.IsTrue(IsEnemyDead(enemyAI));
    }

    // Helper method to check the private 'isDead' field in the EnemyAIController script
    private bool IsEnemyDead(EnemyAIController ai)
    {
        // Use reflection to access the non-public 'isDead' field and check its value
        var field = typeof(EnemyAIController).GetField("isDead", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)field.GetValue(ai);
    }
}
