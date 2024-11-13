using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThirdPersonCamera : MonoBehaviour
{
    //The transform of the player that the camera will follow.
    public Transform player;
    //The offset position of the camera relitive of the player.
    public Vector3 playerOffset = new Vector3(2, 3, -5);
    //The speed at which the camera follows the player.
    public float followPlayerSpeed = 10f;
    //The speed at which the camera rotates to keep the player centered.
    public float rotationPlayerSpeed = 5f;
    //Speed at which the camera rotates horizontally based off player input.
    public float horizontalPlayerspeed = 100f;

    //Tracks horizontal input for the rotation from the rightstick of the gamepad.
    private float horizontalPlayerInput;
    //The current y axis rotation of the camera, adjusted as the player rotates.
    private float currentPlayerYaw = 0f;

    
    void LateUpdate()
    {
        //Checking to see if the player reference has been assigned.
        if (player == null)
        {
            
            return; 
        }

        //Get horizontal input from the rightstick on the gamepad for camera rotation.
        horizontalPlayerInput = Input.GetAxis("RightStickHorizontal");

        //Update camera's yaw based on player input and speed.
        currentPlayerYaw += horizontalPlayerInput * horizontalPlayerspeed * Time.deltaTime;

        //Create a rotation around the y axis based on current yaw.
        Quaternion playerRotation = Quaternion.Euler(0, currentPlayerYaw, 0);

        //Calculate the new camera position with the offset applied to the players position.
        Vector3 playerPosition = player.position + playerRotation * playerOffset;

        //Smoothly move the camera to the players position to avoid unwanted camera behaviour such as shakiness or stutters.
        transform.position = Vector3.Lerp(transform.position, playerPosition, followPlayerSpeed * Time.deltaTime);

        //Smoothly rotate the camera to look at the player, whilst maintaining focus on the player.
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotationPlayerSpeed * Time.deltaTime);
    }
}
