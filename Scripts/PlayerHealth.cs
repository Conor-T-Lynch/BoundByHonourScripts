using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    //The maxHealth of the player. 
    public int maxHealth = 100;
    //Players current health during gameplay.
    private int currentHealth;

    //This the RectTransform UI element for the healthbars fill, visually repersenting the players remaining health.
    public RectTransform healthBarFill;
    //Thihs is the healthbar background with its colour set to red, visually repersenting the players lost health.
    public Image healthBarBackground; 
    //Variable to store the fill width of the healthbar fill and to calculate remaining health visually.
    private float fullWidth;

    //Reference to the animator componant of the player for handling animations.
    private Animator animator;

    //Boolean flag to check if the player is dead.
    private bool isDead = false;

    //A public int so that other scripts can access the PlayerHealth scripts currenthealth.
    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    //Start Method
    void Start()
    {
        //Setting the currentHealth to maxHealth from the start.
        currentHealth = maxHealth;
        //Using the RectTrnasform sizeDelta function to capture the size of the healthBarFill to decrease later.
        fullWidth = healthBarFill.sizeDelta.x; 

        //Links the animator componant to play animations.
        animator = GetComponent<Animator>();

        //Calling the UpdateHealthBar method so that the Health bar is full at the start but changes due to the logic in the the method.
        UpdateHealthBar();

        //Aligning the pivot of the RectTransform so that the health decreces from left to right
        healthBarFill.pivot = new Vector2(1f, 0.5f);
        //Aligning the anchorMin of the RectTransform so that the health decreces from left to right
        healthBarFill.anchorMin = new Vector2(1f, 0.5f);
        //Aligning the anchorMax of the RectTransform so that the health decreces from left to right
        healthBarFill.anchorMax = new Vector2(1f, 0.5f);
    }

    //TakeDamage Method, that passes an int variable damage.
    public void TakeDamage(int damage)
    {
        //Checking to see if the player is dead if true, cease executing the logic from this TakeDamage method.
        if (isDead) return;

        //Subtracting the damage ammount variable from player currentHealth variable.
        currentHealth -= damage;
        //Checking to make sure that the players health does not drop below zero.
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //Calling the UpdateHealthBar method in order to make the changes to the healthBarFill componant, such as updating the visual element for the health bar from green to orange to red.
        UpdateHealthBar();

        //Checking to see if the players health is less then or equal to zero and then plays the death animation
        if (currentHealth <= 0)
        {
            //Debug log to the console to ensure the logic works as intended.
            Debug.Log("Player is dead!");
            //Calling the PlayDeathAnimation method.
            PlayDeathAnimation();
            //Calling the DisablePlayerMovement method.
            DisablePlayerMovement();
            //Setting the isDead boolean to true.
            isDead = true;
        }
    }

    //Heal Method, that passes an int variable ammount.
    public void Heal(int amount)
    {
        //Checking to see if the player is dead if true, cease executing the logic from this heal method.
        if (isDead) return;
        //Adding the heal ammount to players currentHealth variable.
        currentHealth += amount;
        //Keeping the player health from dropping below zero.
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //Update the healthBarFill as nessesary.
        UpdateHealthBar();
    }

    //Healing method for such game objects sush as apples or pears ect.
    public void HealFromFood(int healAmount)
    {
        //Checking to see if the player is dead if true, cease executing the logic from this heal method.
        if (isDead) return;

        //Calling the Heal method, and reusing the logic and passing the healAmount public int from the HealPlayer class.
        Heal(healAmount);
    }

    //UpdateHealthBar Method.
    private void UpdateHealthBar()
    {
        
        if (healthBarFill != null)
        {
            //Calculating the percentage of the players health and adjusts the players healthbar width as needed.
            float healthPercentage = (float)currentHealth / maxHealth;
            healthBarFill.sizeDelta = new Vector2(fullWidth * healthPercentage, healthBarFill.sizeDelta.y);

            //Changes the healthBarFill colour based off players remaining health.
            if (healthPercentage > 0.5f)
            {   //Green.
                healthBarFill.GetComponent<Image>().color = Color.green; 
            }
            else if (healthPercentage > 0.25f)
            {   //Orange.
                healthBarFill.GetComponent<Image>().color = new Color(1f, 0.647f, 0f); 
            }
            else
            {   //Red.
                healthBarFill.GetComponent<Image>().color = Color.red; 
            }
        }
    }

    //PlayDeathAnimation Method.
    private void PlayDeathAnimation()
    {
        //Checking to make sure the animator componant is attached to the player.
        if (animator != null)
        {
            //Triggering the death animation
            animator.SetTrigger("isDead");
        }
        else
        {
            //Debug log to the console to ensure the logic works as intended.
            Debug.LogWarning("Animator component not found on the player!");
        }
    }

    //DisablePlayerMovement Method.
    private void DisablePlayerMovement()
    {
        //Creating a variable to get the PlayerControls script.
        var playerMovement = GetComponent<PlayerControls>();
        if (playerMovement != null)
        {
            //Disables the player controls after death.
            playerMovement.enabled = false;
        }
        else
        {
            //Debug log to the console to ensure the logic works as intended.
            Debug.LogWarning("PlayerMovement component not found on the player!");
        }
    }
}