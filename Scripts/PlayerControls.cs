//@Reference https://www.youtube.com/watch?v=DXw9QhsjlME&ab_channel=GameDevExperiments
//@Reference https://www.youtube.com/watch?v=qc0xU2Ph86Q&ab_channel=SingleSaplingGames
//@Reference https://www.youtube.com/watch?v=Esz2MqxhNig&ab_channel=SunnyValleyStudio
//@Reference https://www.youtube.com/watch?v=NeifEiWjaZA&ab_channel=SunnyValleyStudio
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Quaternion.html
//@Reference https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Animator.SetTrigger.html
//@Reference https://discussions.unity.com/t/moving-character-relative-to-camera/614923
//@Reference https://stackoverflow.com/questions/73590195/unity-accessing-rigidbody-component-with-input-system
//@Reference https://docs.unity3d.com/ScriptReference/Component.GetComponent.html
//@Reference https://www.youtube.com/watch?v=FD4dFZGhrQA&ab_channel=RSDevelopment
//@Reference https://docs.unity3d.com/2021.3/Documentation/Manual/collider-interactions-example-scripts.html
//@Reference https://www.youtube.com/watch?v=_1Oou4459Us&ab_channel=NightRunStudio
//@Reference https://docs.unity3d.com/ScriptReference/Time.html
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform.html
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.html
//@Reference https://www.theslidefactory.com/post/quaternion-basics-in-unity3d#:~:text=Euler%20rotations%20use%20a%20set,a%20rotation%20in%203D%20space
//@Reference https://docs.unity3d.com/ScriptReference/Quaternion.RotateTowards.html
//@Reference https://www.youtube.com/watch?v=4Wh22ynlLyk&ab_channel=PressStart

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour
{
    // The speed of the player when walking.
    public float walkSpeed = 5f;
    // The speed of the player when running.
    public float runSpeed = 10f;
    // The speed at which the player can rotate toward the movement direction.
    public float rotationSpeed = 1100f;
    // Reference to the camera transform to align movement with the camera direction.
    public Transform cameraTransform;
    // The amount of damage the player deals when attacking.
    public int attackDamage = 20;
    // AudioSource for playing sounds
    private AudioSource audioSource;
    // Reference to the sword attack sound effect
    public AudioClip swordAttackSound;
    // Rigidbody component for handling physics-based movement.
    private Rigidbody rb;
    // Reference to the Animator component of the player.
    private Animator animator;

    public void Start()
    {
        // Get the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
        // Get the Animator component attached to the player.
        animator = GetComponent<Animator>();
        // Get the AudioSource component attached to the player
        audioSource = GetComponent<AudioSource>();

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            // Set the weight of the blocking layer to blend walking and blocking animations simultaneously.
            animator.SetLayerWeight(animator.GetLayerIndex("BlockingLayer"), 1.0f);

        }
    }

    public void Update()
    {
        if (cameraTransform == null) return;

        // Get horizontal input from the gamepad's right stick.
        float moveHorizontal = Input.GetAxis("Horizontal");
        // Get vertical input from the gamepad's left stick.
        float moveVertical = Input.GetAxis("Vertical");

        // Check if the player is holding the run button.
        bool isRunning = Input.GetButton("Run");

        // Determine the movement speed based on whether the player is running.
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Get the forward and right directions from the camera.
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Keep movement on the horizontal plane.
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Calculate movement direction based on input and camera orientation.
        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized * currentSpeed;

        // Apply movement to the player.
        MovePlayer(movement);

        // Determine if the player is moving.
        bool isMoving = movement.magnitude > 0.1f;

        // Rotate the player toward the movement direction.
        if (isMoving)
        {
            RotatePlayer(movement);
        }

        // Get input from the LT button for blocking.
        bool isBlocking = Input.GetAxis("LT") > 0.1f;

        // Get input from the RT button for attacking.
        bool isAttacking = Input.GetAxis("RT") > 0.1f;

        // Update animation parameters based on movement and actions.
        SetAnimations(isMoving, isRunning, isBlocking, isAttacking);
    }

    // Handles player movement using Rigidbody for physics-based interaction.
    public void MovePlayer(Vector3 movement)
    {
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    // Rotates the player in the direction of movement.
    public void RotatePlayer(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Updates the animation states based on the current actions.
    public void SetAnimations(bool isMoving, bool isRunning, bool isBlocking, bool isAttacking)
    {
        if (animator != null)
        {
            // Walking if moving, not running, and not blocking.
            animator.SetBool("isWalking", isMoving && !isRunning && !isBlocking);
            // Running if moving, running, and not blocking.
            animator.SetBool("isRunning", isMoving && isRunning && !isBlocking);
            // Blocking if blocking input is active.
            animator.SetBool("isBlocking", isBlocking);

            if (isAttacking)
            {
                // Trigger the attack animation.
                animator.SetTrigger("isAttacking");
                // Play the sword sound effect when the attack animation is triggered
                PlaySwordSound();
            }
        }
    }

    // Method to play the sword attack sound
    private void PlaySwordSound()
    {
        if (audioSource != null && swordAttackSound != null)
        {
            audioSource.PlayOneShot(swordAttackSound);
        }
    }

    // Detects trigger collisions with other objects.
    public void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "Enemy".
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit detected");

            // Get the EnemyAIController component from the enemy.
            EnemyAIController enemy = other.GetComponent<EnemyAIController>();

            if (enemy != null)
            {
                Debug.Log("Calling TakeDamage");
                // Apply damage to the enemy.
                enemy.TakeDamage(attackDamage);
            }
        }
    }
}
