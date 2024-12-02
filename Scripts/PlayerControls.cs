//@Reference https://www.youtube.com/watch?v=DXw9QhsjlME&ab_channel=GameDevExperiments
//@Reference https://www.youtube.com/watch?v=qc0xU2Ph86Q&ab_channel=SingleSaplingGames
//@Reference https://www.youtube.com/watch?v=Esz2MqxhNig&ab_channel=SunnyValleyStudio
//@Reference https://www.youtube.com/watch?v=NeifEiWjaZA&ab_channel=SunnyValleyStudio
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Quaternion.html
//@Reference https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Animator.SetTrigger.html

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour
{
    //The speed of the player when walking.
    public float walkSpeed = 5f;
    //The speed of the player when running.
    public float runSpeed = 10f;
    //The speed at how fast the player can rotate and turn towards the movement direction.
    public float rotationSpeed = 1100f;
    //Reference to the camera transform to align movement with the camera direction.
    public Transform cameraTransform;
    //The ammount of damage the player does when attacking.
    public int attackDamage = 10;
    //Rigidbody componant for handling physics based movement.
    private Rigidbody rb;
    //Reference to the animator componant of the enemy for handleing animations.
    private Animator animator;

    //Start Method
    void Start()
    {
        //Get the rigidbody componant attached to the player for movement.
        rb = GetComponent<Rigidbody>();
        //Getting the animator componant that is attached to the player.
        animator = GetComponent<Animator>();

        //Setting the weight of the blocking layer in order to blend the walking and blocking animations so that they play simutainiously.
        animator.SetLayerWeight(animator.GetLayerIndex("BlockingLayer"), 1.0f);
    }

    //Update Method
    void Update()
    {
        //Getting the horizontal imput from the gamepads leftstick.
        float moveHorizontal = Input.GetAxis("Horizontal");
        //Getting the vertical imput from the gamepads leftstick.
        float moveVertical = Input.GetAxis("Vertical");

        //Getting the input from the LB buttoun on the gamepad.
        bool isRunning = Input.GetButton("Run");

        //Setting the players movement speed based off if the player is running or walking.
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        //Getting the forward direction of the camera.
        Vector3 forward = cameraTransform.forward;
        //Getting the right direction of the camera.
        Vector3 right = cameraTransform.right;
        //ignoring the y axis to keep movement horizontal.
        forward.y = 0;
        
        right.y = 0;
        //Normalizing it to ensure a consistant movement speed
        forward.Normalize();

        right.Normalize();
        //calculate movement direction based off of the position of the camera and player input.
        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized * currentSpeed;

        //Moves the player to the calcuted direction.
        MovePlayer(movement);

        //Checking to see if the player is moving based on the calcualted movement vectors magnitude.
        bool isMoving = movement.magnitude > 0.1f;

        
        if (isMoving)
        {
            //Rotates the player in the direction that they are moving if they are moving.
            RotatePlayer(movement);
        }

        //Getting the input from the LT buttoun on the gamepad.
        bool isBlocking = Input.GetAxis("LT") > 0.1f;

        //Getting the input from the RT buttoun on the gamepad.
        bool isAttacking = Input.GetAxis("RT") > 0.1f;

        //Setting the animations based on the peremeters moving, running, blocking and attacking.
        SetAnimations(isMoving, isRunning, isBlocking, isAttacking);
    }
    
    //Handling player movement using rigidbody for physics based interactions.
    void MovePlayer(Vector3 movement)
    {
        //Setting the rigidbodys velocity to move the player in the desired direction, while keeping the y axis velocity.
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    //Rotates the player in the desired direction.
    void RotatePlayer(Vector3 movement)
    {
        //Only rotates if there in movement.
        if (movement != Vector3.zero)
        {
            //Calculating the target rotation based on movement direction.
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            //Rotates towards the target rotation at the defined rotation speed.
            rb.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    //SetAnimations Method that updates the animations based on the peremeters moving, running, blocking and attacking.
    void SetAnimations(bool isMoving, bool isRunning, bool isBlocking, bool isAttacking)
    {
        //Checking to make sure the animator componant is attached to the player.
        if (animator != null)
        {
            //Set isWalking if the player is moving but not running, but is blocking.
            animator.SetBool("isWalking", isMoving && !isRunning && !isBlocking);
            //Set isRunning if the player is moving, running and not blocking.
            animator.SetBool("isRunning", isMoving && isRunning && !isBlocking);
            //Set is Blocking if the player is moving and blocking.
            animator.SetBool("isBlocking", isBlocking);
            
            if (isAttacking)
            {
                //Triggers the attack animation when the player is attacking.
                animator.SetTrigger("isAttacking"); 
            }
        }
    }

    //Method to detect collisions with other objects.
    void OnTriggerEnter(Collider other)
    {
        //Checking to see if the collide object has the tag Enemy.
        if (other.CompareTag("Enemy"))
        {
            //Debug log to the console to ensure the logic works as intended.
            Debug.Log("Enemy hit detected");
            //Getting the EnemyAIController componant to get the logic that deals damage.
            EnemyAIController enemy = other.GetComponent<EnemyAIController>();

            if (enemy != null)
            {
                //Debug log to the console to ensure the logic works as intended.
                Debug.Log("Calling TakeDamage");
                //Calling the TakeDamage Method.
                enemy.TakeDamage(attackDamage);
            }
        }
    }
}
