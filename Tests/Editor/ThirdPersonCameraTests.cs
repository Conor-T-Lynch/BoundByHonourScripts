//@Reference https://docs.nunit.org/articles/nunit/intro.html
//@Reference https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.html
//@Reference https://letsmakeagame.net/unity-unit-testing-basics-tutorial
//@Reference https://www.youtube.com/watch?v=dXAQ3L3dySc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
public class ThirdPersonCameraTests
{
    // Declare GameObject for the camera
    private GameObject cameraObject;
    // Declare ThirdPersonCamera to hold the camera component
    private ThirdPersonCamera thirdPersonCamera;
    // Declare GameObject for the player
    private GameObject playerObject;
    // Declare Transform to hold the transform component
    private Transform playerTransform;

    [SetUp]
    public void SetUp()
    {
        // Create player GameObject and add necessary components
        playerObject = new GameObject("Player");
        playerTransform = playerObject.transform;

        // Set initial player position
        playerTransform.position = new Vector3(10, 0, 10);

        // Create camera GameObject and attach ThirdPersonCamera script
        cameraObject = new GameObject("Camera");
        thirdPersonCamera = cameraObject.AddComponent<ThirdPersonCamera>();

        // Set camera's player reference to the player transform
        thirdPersonCamera.player = playerTransform;

        // Set initial values for testing
        thirdPersonCamera.playerOffset = new Vector3(2, 3, -5);
        thirdPersonCamera.followPlayerSpeed = 10f;
        thirdPersonCamera.rotationPlayerSpeed = 5f;
        thirdPersonCamera.horizontalPlayerspeed = 100f;

        // Rotate the camera to face the correct direction (facing the player)
        cameraObject.transform.rotation = Quaternion.Euler(0, 180, 0); // Adjust to match the expected facing direction

        // Log player and camera positions for debugging
        Debug.Log($"Initial Player Position: {playerTransform.position}");
        Debug.Log($"Initial Camera Rotation: {cameraObject.transform.rotation}");
    }

    [Test]
    public void CameraFollowsPlayerCorrectly()
    {
        // Move the player to a test position
        playerTransform.position = new Vector3(10, 0, 10);

        // Ensure the camera is set to a neutral rotation before the test
        cameraObject.transform.rotation = Quaternion.identity;

        // Capture the initial camera position before calling LateUpdate
        Vector3 initialCameraPosition = cameraObject.transform.position;

        // Simulate the LateUpdate method (to test if camera follows player)
        thirdPersonCamera.LateUpdate();

        // Calculate the expected camera position based on the player's position and the offset
        Vector3 expectedPosition = playerTransform.position + thirdPersonCamera.playerOffset;

        // Logging for debugging
        Debug.Log("Initial Camera Position: " + initialCameraPosition);
        Debug.Log("Expected Camera Position: " + expectedPosition);

        // Assert that the camera position has changed to reflect the player's movement
        // We check that the camera moved closer to the expected position by comparing its difference
        Assert.IsTrue(Vector3.Distance(cameraObject.transform.position, expectedPosition) < Vector3.Distance(initialCameraPosition, expectedPosition));
    }

    [UnityTest]
    public IEnumerator CameraRotatesTowardsPlayerInput()
    {
        // Simulating player movement towards a direction (using right-stick input for rotation)
        thirdPersonCamera.horizontalPlayerInput = 1f; // Simulate right-stick input to rotate right

        // Set an initial camera rotation
        Quaternion initialRotation = cameraObject.transform.rotation;

        // Calling UpdateCamera method to simulate one frame of camera update (similar to RotatePlayer)
        thirdPersonCamera.LateUpdate();
        yield return null;

        // Capture the new camera rotation after applying input
        Quaternion newRotation = cameraObject.transform.rotation;

        // Logging the initial and new rotations for debugging
        Debug.Log("Initial Camera Rotation: " + initialRotation.eulerAngles);
        Debug.Log("New Camera Rotation: " + newRotation.eulerAngles);

        // Assert that the camera's rotation has changed
        Assert.AreNotEqual(initialRotation, newRotation);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up created GameObjects
        Object.DestroyImmediate(cameraObject);
        Object.DestroyImmediate(playerObject);
    }
}
