//@Reference https://www.youtube.com/watch?v=b-WZEBLNCik&ab_channel=GDTitans
//@Reference https://www.youtube.com/watch?v=db0KWYaWfeM&ab_channel=CodeMonkey
//@Reference https://www.youtube.com/watch?v=yWcupAmA-lY&ab_channel=UnityGuy
//@Reference https://www.youtube.com/watch?v=Esz2MqxhNig&ab_channel=SunnyValleyStudio
//@Reference https://www.youtube.com/watch?v=NeifEiWjaZA&ab_channel=SunnyValleyStudio
//@Reference https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Animator.SetTrigger.html
//@Reference https://www.codeproject.com/Articles/1210435/Day-Creating-Player-Health-System-and-Health-UI
//@Reference https://www.youtube.com/watch?v=_1Oou4459Us&ab_channel=NightRunStudio
//@Reference https://www.youtube.com/watch?v=5r6RmddoR80&ab_channel=chonk
//@Reference https://docs.unity3d.com/ScriptReference/Time.html
//@Reference https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform.html
//@Reference https://connect-prd-cdn.unity.com/20190702/696ca78d-5ca8-481d-901c-9d788f051cea_Lesson_Plan_4.2___Follow_the_Player.pdf
//@Reference https://www.theslidefactory.com/post/quaternion-basics-in-unity3d#:~:text=Euler%20rotations%20use%20a%20set,a%20rotation%20in%203D%20space
//@Reference https://www.youtube.com/watch?v=4Wh22ynlLyk&ab_channel=PressStart
//@Reference https://www.youtube.com/watch?v=_pKZ73mfIlM&ab_channel=RehopeGames

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAIController : MonoBehaviour
{
    // sThe max health that the enemy can have.
    public int maxHealth = 250;
    // The enemy's current health, initialized from the start() method.
    private int currentHealth;

    // A reference to the players transform componant to track player position.
    public Transform player;
    // Enemy's movement speed.
    public float moveSpeed = 3f;
    // The range at which the enemy can detect the player.
    public float detectionRange = 10f;
    // The range at which the enemy can attack the player.
    public float attackRange = 2f;
    // The cooldown time of the enemy attacks, so that the player isn't getting constantly spamed with attacks from the enemy.
    public float attackCooldown = 5f;
    // The last time at which the enemy has attacked, so as to manage the cooldown time of enemy attacks.
    private float lastAttackTime = 2f;
    // AudioSource for playing sounds
    private AudioSource audioSource;
    // Reference to the sword attack sound effect
    public AudioClip swordAttackSound;
    // Reference to the animator componant of the enemy for handleing animations.
    private Animator animator;
    // Reference to the Playerhealth script, so that when called the player takes damage from enemy attacks.
    private PlayerHealth playerHealth; 

    // Boolean flag to check if the enemy is dead.
    private bool isDead = false;
    private bool dialogueFinished = false;

    public string enemyTagForQuest;

    // Start Method
    public void Start()
    {
        // Setting the currentHealth to maxHealth from the start.
        currentHealth = maxHealth;

        // Getting the animator componant that is attached to the enemy.
        animator = GetComponent<Animator>();
        // Get the AudioSource component attached to the player
        audioSource = GetComponent<AudioSource>();

        // Cheaking to see if the player game object is assigned.
        if (player != null)
        {
            // Fetching the PlayerHealth script componant.
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    // Update Method.
    public void Update()
    {
        // Checking to see if the enemy is dead if true, cease executed the logic from this update method.
        if (isDead) return;
        // Checking to see if there is no player to track if true, cease executed the logic from this update method.
        if (player == null) return;
        // Calculating the distence from enemy and the player.
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // Checking to see if the player is within detection range.
        if (distanceToPlayer <= detectionRange)
        {
            // Checking to see if the player is also in attack range.
            if (distanceToPlayer <= attackRange)
            {
                // Checking to see if enough time has passed since last attack.
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    // Execute the attacj method.
                    Attack();
                    // Resetting the last attack time.
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                // Checking to see if the player is out of attack range if so executes the FollowPlayer method.
                if (dialogueFinished)
                {
                    FollowPlayer();
                }
            }
        }
        else
        {
            // Checking to see if the player is out of dectection range, stops playing the walking animation.
            animator.SetBool("isWalking", false);
        }
    }

    // FollowPlayer Method.
    public void FollowPlayer()
    {
        // Calculating the direction towards the player while also noramlizing it to keep the enemy's speed consistent.
        Vector3 direction = (player.position - transform.position).normalized;
        // Moves the enemy in the direction of the player.
        transform.position += direction * moveSpeed * Time.deltaTime;
        // Unity function to rotate and face the enemy in the players direction.
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        // Enable the walking animation.
        animator.SetBool("isWalking", true);
    }

    // Attack method.
    public void Attack()
    {
        // Checking to see if the player has health that can take damage.
        if (playerHealth != null && playerHealth.CurrentHealth > 0)
        {
            // Triggering the attack animation
            animator.SetTrigger("attack");
            // Play the sword sound effect when the attack animation is triggered
            PlaySwordSound();
            // Debug log to the console to ensure the logic works as intended.
            Debug.Log("Enemy attacks the player!");
            // The ammount of damge that is inflicted on the player by calling the TakeDamage method.
            playerHealth.TakeDamage(5);
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


    // TakeDamage Method, that passes an int variable damage.
    public virtual void TakeDamage(int damage)
    {
        // Subtracting the damage ammount variable from enemys currentHealth variable.
        currentHealth -= damage;
        // Debug log to the console to ensure the logic works as intended.
        Debug.Log("Enemy takes " + damage + " damage! Current Health: " + currentHealth);
        // Checking to see if the enemys currentHealth less then or equal to zero.
        if (currentHealth <= 0)
        {
            // If true execute the Die method.
            Die();
        }
    }

    // Die Method to handle enemys death animation.
    private void Die()
    {
        // Prevents death logic from being triggered again
        if (isDead) return;

        isDead = true;
        // Log to confirm if Die() is being called multiple times
        Debug.Log("Enemy died!");  

        // Triggering the death animation of the enemy.
        if (animator != null)
        {
            animator.SetTrigger("die");
        }
        else
        {
            Debug.LogWarning("Animator not assigned on EnemyAIController.");
        }

        // Check if QuestManager is initialized
        if (QuestManager.Instance != null) 
        {
            if (!string.IsNullOrEmpty(enemyTagForQuest))
            {
                QuestManager.Instance.RegisterKill(enemyTagForQuest);
            }
        }

        else
        {
            Debug.LogError("QuestManager is not initialized. Please check your scene.");
        }

        // Destroys the enemy game object after 5 seconds.
        Destroy(gameObject, 5f);
    }

    // Method to be called when dialogue finishes, allowing the enemy to start following the player again.
    public void EnableAI()
    {
        // Now the enemy can follow the player
        dialogueFinished = true; 
    }

    // Method to be called to disable AI during dialogue.
    public void DisableAI()
    {
        // Prevent the enemy from following the player during dialogue
        dialogueFinished = false;
    }

}
