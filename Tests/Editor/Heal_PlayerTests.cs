//@Reference https://docs.nunit.org/articles/nunit/intro.html
//@Reference https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.reflection?view=net-6.0
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.html
//@Reference https://letsmakeagame.net/unity-unit-testing-basics-tutorial
//@Reference https://www.youtube.com/watch?v=dXAQ3L3dySc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using UnityEngine.TestTools;

public class Heal_PlayerTests
{
    // Declare GameObject for the player
    private GameObject playerObject;

    // Declare PlayerHealth to hold the health component
    private PlayerHealth playerHealth;

    // Declare GameObject for the healing object
    private GameObject healObject;

    // Declare Heal_Player script reference
    private Heal_Player healPlayer;

    [SetUp]
    public void SetUp()
    {
        // Create the Player GameObject and attach a BoxCollider to simulate physical interaction
        playerObject = new GameObject("Player", typeof(BoxCollider));

        // Assign the "Player" tag to the player object
        playerObject.tag = "Player";

        // Add the PlayerHealth component to handle the player's health logic
        playerHealth = playerObject.AddComponent<PlayerHealth>();

        // Create the health bar fill (RectTransform and Image components)
        var fill = new GameObject("Fill", typeof(RectTransform), typeof(Image));
        // Parent it to the player object
        fill.transform.SetParent(playerObject.transform);  

        // Set health bar fill
        playerHealth.healthBarFill = fill.GetComponent<RectTransform>();

        // Create the health bar background (Image component)
        var background = new GameObject("Background", typeof(Image));
        // Parent it to the player object
        background.transform.SetParent(playerObject.transform); 

        // Set background image
        playerHealth.healthBarBackground = background.GetComponent<Image>();

        // Create the label for the health bar (TextMeshProUGUI component)
        var label = new GameObject("Label", typeof(TextMeshProUGUI));
        // Parent it to the player object
        label.transform.SetParent(playerObject.transform);  

        // Set label
        playerHealth.healthBarLabel = label.GetComponent<TextMeshProUGUI>();

        // Initialize the PlayerHealth component
        playerHealth.Start();

        // Set the current health to 80 so that healing can occur
        playerHealth.GetType().GetField("currentHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerHealth, 80);

        // Create the HealObject GameObject with a BoxCollider to simulate healing interaction
        healObject = new GameObject("HealObject", typeof(BoxCollider));

        // Add Heal_Player component to handle healing logic
        healPlayer = healObject.AddComponent<Heal_Player>();

        // Set the heal amount to 20
        healPlayer.healAmount = 20;
    }

    [Test]
    public void OnTriggerEnter_HealsPlayerAndDestroysHealObject()
    {
        // Expect Unity to complain about using Destroy in edit mode, which is expected behavior
        LogAssert.Expect(LogType.Error, "Destroy may not be called from edit mode! Use DestroyImmediate instead.\nDestroying an object in edit mode destroys it permanently.");

        // Simulate the player entering the healing trigger area
        healPlayer.OnTriggerEnter(playerObject.GetComponent<Collider>());

        // Assert that the player's health is now 100 after being healed
        Assert.AreEqual(100, playerHealth.CurrentHealth);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up by destroying the player and heal object after tests
        Object.DestroyImmediate(playerObject);
        Object.DestroyImmediate(healObject);
    }
}
