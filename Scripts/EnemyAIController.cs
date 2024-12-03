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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAIController : MonoBehaviour
{
    //The max health that the enemy can have.
    public int maxHealth = 150;
    //The enemy's current health, initialized from the start() method.
    private int currentHealth;

    //A reference to the players transform componant to track player position.
    public Transform player;
    //Enemy's movement speed.
    public float moveSpeed = 3f;
    //The range at which the enemy can detect the player.
    public float detectionRange = 10f;
    //The range at which the enemy can attack the player.
    public float attackRange = 2f;
    //The cooldown time of the enemy attacks, so that the player isn't getting constantly spamed with attacks from the enemy.
    public float attackCooldown = 5f;
    //The last time at which the enemy has attacked, so as to manage the cooldown time of enemy attacks.
    private float lastAttackTime = 0f;
    //Reference to the animator componant of the enemy for handleing animations.
    private Animator animator;
    //Reference to the Playerhealth script, so that when called the player takes damage from enemy attacks.
    private PlayerHealth playerHealth; 

    //Boolean flag to check if the enemy is dead.
    private bool isDead = false;

    //Start Method
    void Start()
    {
        //Setting the currentHealth to maxHealth from the start.
        currentHealth = maxHealth;

        //Getting the animator componant that is attached to the enemy.
        animator = GetComponent<Animator>(); 
        
        //Cheaking to see if the player game object is assigned.
        if (player != null)
        {
            //Fetching the PlayerHealth script componant.
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    //Update Method.
    void Update()
    {
        //Checking to see if the enemy is dead if true, cease executed the logic from this update method.
        if (isDead) return;
        //Checking to see if there is no player to track if true, cease executed the logic from this update method.
        if (player == null) return;
        //Calculating the distence from enemy and the player.
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //Checking to see if the player is within detection range.
        if (distanceToPlayer <= detectionRange)
        {
            //Checking to see if the player is also in attack range.
            if (distanceToPlayer <= attackRange)
            {
                //Checking to see if enough time has passed since last attack.
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    //Execute the attacj method.
                    Attack();
                    //Resetting the last attack time.
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                //Checking to see if the player is out of attack range if so executes the FollowPlayer method.
                FollowPlayer();
            }
        }
        else
        {
            //Checking to see if the player is out of dectection range, stops playing the walking animation.
            animator.SetBool("isWalking", false);
        }
    }

    //FollowPlayer Method.
    private void FollowPlayer()
    {
        //Calculating the direction towards the player while also noramlizing it to keep the enemy's speed consistent.
        Vector3 direction = (player.position - transform.position).normalized;
        //Moves the enemy in the direction of the player.
        transform.position += direction * moveSpeed * Time.deltaTime;
        //Unity function to rotate and face the enemy in the players direction.
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        //Enable the walking animation.
        animator.SetBool("isWalking", true);
    }

    //Attack method.
    private void Attack()
    {
        //Checking to see if the player has health that can take damage.
        if (playerHealth != null && playerHealth.CurrentHealth > 0)
        {
            //Triggering the attack animation
            animator.SetTrigger("attack");
            //Debug log to the console to ensure the logic works as intended.
            Debug.Log("Enemy attacks the player!");

            //The ammount of damge that is inflicted on the player by calling the TakeDamage method.
            playerHealth.TakeDamage(5);
        }
    }

    //TakeDamage Method, that passes an int variable damage.
    public void TakeDamage(int damage)
    {
        //Subtracting the damage ammount variable from enemys currentHealth variable.
        currentHealth -= damage;
        //Debug log to the console to ensure the logic works as intended.
        Debug.Log("Enemy takes " + damage + " damage! Current Health: " + currentHealth);
        //Checking to see if the enemys currentHealth less then or equal to zero.
        if (currentHealth <= 0)
        {
            //If true execute the Die method.
            Die();
        }
    }

    //Die Method to handle enemys death animation.
    private void Die()
    {
        //Setting the isDead boolean to true.
        isDead = true;
        //Triggering the death animation of the enemy.
        animator.SetTrigger("die");
        //Destorys the enemy game object after 5 seconds.
        Destroy(gameObject, 5f);
    }
}
