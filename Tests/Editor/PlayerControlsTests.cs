//@Reference https://docs.nunit.org/articles/nunit/intro.html
//@Reference https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.html
//@Reference https://letsmakeagame.net/unity-unit-testing-basics-tutorial
//@Reference https://www.youtube.com/watch?v=dXAQ3L3dySc

using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class PlayerControlsTests
{
    private GameObject playerObject;
    private PlayerControls playerControls;
    private Rigidbody rb;

    // set up the test environment
    [SetUp]
    public void SetUp()
    {
        // Set up test GameObject
        playerObject = new GameObject("Player");
        rb = playerObject.AddComponent<Rigidbody>();
        playerControls = playerObject.AddComponent<PlayerControls>();

        // Create dummy camera
        var cameraObject = new GameObject("Camera");
        playerControls.cameraTransform = cameraObject.transform;
        // manually invoke start to initialize the playercontrols
        playerControls.Invoke("Start", 0f);
    }

    // clean up the test environment after each test
    [TearDown]
    public void TearDown()
    {
        // destroy the player game object to clean up after each test
        Object.DestroyImmediate(playerObject);
        Object.DestroyImmediate(playerControls.cameraTransform.gameObject);
    }

    // testing the movement functionality of the playercontrols
    [UnityTest]
    public IEnumerator MovePlayer_ChangesVelocity()
    {
        // creating vectors along the x-axis
        Vector3 movement = new Vector3(1f, 0f, 0f) * playerControls.walkSpeed;
        // calling the moveplayer method to apply movement
        playerControls.MovePlayer(movement);
        yield return null;
        // asseting that the x and z velocities mathch the direction
        Assert.AreEqual(movement.x, rb.velocity.x, 0.01f);
        Assert.AreEqual(0f, rb.velocity.y, 0.01f);
        Assert.AreEqual(movement.z, rb.velocity.z, 0.01f);
    }

    // testing the rotation functionality
    [UnityTest]
    public IEnumerator RotatePlayer_RotatesTowardsMovementDirection()
    {
        // sets movement direction towards the z-axis
        Vector3 movement = new Vector3(0f, 0f, 1f);

        // Set a starting rotation that clearly isn't facing forward
        playerObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        // capturing the rotation of the player
        Quaternion initialRotation = playerObject.transform.rotation;
        // calling the rotateplayer method to get rotation movement
        playerControls.RotatePlayer(movement);
        yield return null;
        // capturing the rotation of the player
        Quaternion newRotation = playerObject.transform.rotation;
        // logging the new rotations for debugging
        Debug.Log("Initial Rotation: " + initialRotation.eulerAngles);
        Debug.Log("New Rotation: " + newRotation.eulerAngles);

        Assert.AreNotEqual(initialRotation, newRotation);
    }

    // testing the ontriggerenter method
    [Test]
    public void OnTriggerEnter_EnemyReceivesDamage()
    {
        // creating an enemy gameobject with tag of enemy
        GameObject enemy = new GameObject("Enemy");
        enemy.tag = "Enemy";
        // mock script to simulate taking damage
        var mockEnemy = enemy.AddComponent<MockEnemy>();
        // mock box collider to simulate a trigger
        var collider = enemy.AddComponent<BoxCollider>();
        // simulate a collision with the player
        playerControls.OnTriggerEnter(collider);
        // assert that the mock enemy has been damaged
        Assert.IsTrue(mockEnemy.wasDamaged);
        // assert that it matches the attack damage
        Assert.AreEqual(playerControls.attackDamage, mockEnemy.lastDamage);
        // destroy the enemy object after the test
        Object.DestroyImmediate(enemy);
    }

    // mock class to simulate enemy damage handling
    private class MockEnemy : EnemyAIController
    {
        public bool wasDamaged = false;
        public int lastDamage = 0;
        // override TakeDamage method to simulate taking damage
        public override void TakeDamage(int damage)
        {
            // mock the enemy has been damaged
            wasDamaged = true;
            // store the recieved damage value
            lastDamage = damage;
        }
    }
}
