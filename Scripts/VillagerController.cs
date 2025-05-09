//@Reference https://docs.unity3d.com/ScriptReference/Quaternion.LookRotation.html
//@Reference https://docs.unity3d.com/ScriptReference/Quaternion.Slerp.html
//@Reference https://docs.unity3d.com/ScriptReference/Vector3.Distance.html
//@Reference https://docs.unity3d.com/ScriptReference/Animator.SetBool.html
//@Reference https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
//@Reference https://stackoverflow.com/questions/20329460/unity3d-slerp-rotation-speed

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This script allows a villager NPC to rotate and face the player when within a certain detection range.
public class VillagerController : MonoBehaviour
{
    // Reference to the player's Transform in the scene
    public Transform player;

    // Distance within which the villager will detect and react to the player
    public float detectionRange = 10f;

    // Reference to the Animator component attached to the villager
    private Animator animator;

    // Flag to check if the villager is dead, preventing interaction
    private bool isDead = false;

    // Called when the script instance is being loaded
    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    // Called once per frame
    void Update()
    {
        // If the villager is dead or the player reference is missing, do nothing
        if (isDead || player == null) return;

        // Calculate the distance between the villager and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the specified detection range
        if (distanceToPlayer <= detectionRange)
        {
            // Get the direction from the villager to the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Ignore vertical difference to keep rotation on a flat plane
            direction.y = 0f;

            // If the direction is not zero, rotate the villager to face the player
            if (direction != Vector3.zero)
            {
                // Get the desired rotation to look at the player
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                // Smoothly interpolate from current rotation to the desired rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
            }

            // Set the walking animation to false, assuming the villager stops when noticing the player
            animator.SetBool("isWalking", false);
        }
    }
}
