//@Reference https://docs.nunit.org/articles/nunit/intro.html
//@Reference https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.html
//@Reference https://letsmakeagame.net/unity-unit-testing-basics-tutorial
//@Reference https://www.youtube.com/watch?v=dXAQ3L3dySc

using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealthTests
{
    // Declare GameObject for the player
    private GameObject player;
    // Declare PlayerHealth to test health functionality
    private PlayerHealth playerHealth;

    // Method to set up objects before each test
    [SetUp]  
    public void SetUp()
    {
        // Create a new GameObject for the player and add RectTransform for positioning
        var gameObject = new GameObject("Player", typeof(RectTransform));

        // Add PlayerHealth script to the player object
        playerHealth = gameObject.AddComponent<PlayerHealth>();

        // Create a child object for the health bar fill (RectTransform and Image components)
        var fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
        // Set fill object as a child of the player
        fillObject.transform.SetParent(gameObject.transform);
        // Set fill reference for health bar
        playerHealth.healthBarFill = fillObject.GetComponent<RectTransform>();  

        // Create a sibling object for the background of the health bar (Image component)
        var backgroundObject = new GameObject("Background", typeof(Image));
        // Set background object as a sibling of the fill
        backgroundObject.transform.SetParent(gameObject.transform);
        // Set background reference
        playerHealth.healthBarBackground = backgroundObject.GetComponent<Image>();  

        // Create a text object for displaying health (TextMeshProUGUI component)
        var textObject = new GameObject("HealthText", typeof(TextMeshProUGUI));
        // Set text object as a child of the player
        textObject.transform.SetParent(gameObject.transform);
        // Set label reference for health text
        playerHealth.healthBarLabel = textObject.GetComponent<TextMeshProUGUI>();  

        // Initialize the PlayerHealth component by calling Start
        playerHealth.Start();
    }

    // Method to clean up objects after each test
    [TearDown]  
    public void TearDown()
    {
        // Destroy the player object after the test
        Object.DestroyImmediate(player);
    }

    // Test to verify health is reduced when taking damage
    [Test]  
    public void TakeDamage_ReducesHealth()
    {
        // Store the player's current health before damage
        int initialHealth = playerHealth.CurrentHealth;

        // Call TakeDamage to reduce the health by 20
        playerHealth.TakeDamage(20);

        // Assert that the player's current health is less than the initial health
        Assert.Less(playerHealth.CurrentHealth, initialHealth);
    }

    // Test to verify health is set to zero and player is flagged as dead when taking excessive damage
    [Test]  
    public void TakeDamage_SetsHealthToZeroAndFlagsDead()
    {
        // Inflict 999 damage to simulate player death
        playerHealth.TakeDamage(999);

        // Assert that the player's health is now zero
        Assert.AreEqual(0, playerHealth.CurrentHealth);

        // Store current health and attempt healing
        int currentHealth = playerHealth.CurrentHealth;
        playerHealth.Heal(20);

        // Assert that the player's health does not increase when dead
        Assert.AreEqual(currentHealth, playerHealth.CurrentHealth, "Should not heal when dead");
    }

    // Test to verify health increases up to max value when healing
    [Test]  
    public void Heal_IncreasesHealth_UpToMax()
    {
        // Damage the player by 50 to simulate a decrease in health
        playerHealth.TakeDamage(50);

        // Store the player's health after taking damage
        int damagedHealth = playerHealth.CurrentHealth;

        // Heal the player by 30
        playerHealth.Heal(30);

        // Assert that health is increased after healing
        Assert.Greater(playerHealth.CurrentHealth, damagedHealth);

        // Assert that health does not exceed the maximum allowed value
        Assert.LessOrEqual(playerHealth.CurrentHealth, playerHealth.maxHealth);
    }

    // Test to ensure healing does nothing when player is dead
    [Test]  
    public void Heal_DoesNothing_WhenDead()
    {
        // Inflict 999 damage to simulate player death
        playerHealth.TakeDamage(999);

        // Store the player's health at the moment of death
        int healthAtDeath = playerHealth.CurrentHealth;

        // Attempt healing
        playerHealth.Heal(50);

        // Assert that health does not change when the player is dead
        Assert.AreEqual(healthAtDeath, playerHealth.CurrentHealth);
    }

    // Test to ensure damage does nothing when player is already dead
    [Test]  
    public void TakeDamage_DoesNothing_WhenAlreadyDead()
    {
        // Inflict 999 damage to simulate player death
        playerHealth.TakeDamage(999);

        // Store the player's health at the moment of death
        int healthAtDeath = playerHealth.CurrentHealth;

        // Attempt to inflict more damage
        playerHealth.TakeDamage(50);

        // Assert that health does not change after the player is dead
        Assert.AreEqual(healthAtDeath, playerHealth.CurrentHealth);
    }
}
