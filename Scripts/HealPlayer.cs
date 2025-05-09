//@Reference https://www.youtube.com/watch?v=bRcMVkJS3XQ&ab_channel=MoreBBlakeyyy
//@Reference https://docs.unity3d.com/2021.3/Documentation/Manual/collider-interactions-example-scripts.html

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Heal_Player : MonoBehaviour
{
    //The ammount of health that this object will restore to the player health bar when collided with.
    public int healAmount = 20;

    //This method is triggered when another object enters the collider attached to this object.
    public void OnTriggerEnter(Collider other)
    {
        //Checking to see if the object has the tag Player.
        if (other.CompareTag("Player"))
        {
            //getting the playerHealth componant from the colliding object.
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            //Checking to see if the playerHealth componant exists on the colliding object.
            if (playerHealth != null)
            {
                //Call the healing method on the PlayerHealth script and passes the health amount.
                playerHealth.HealFromFood(healAmount);
                //Destroys the game object once the collision has been made.
                Destroy(gameObject);
            }
        }
    }
}
