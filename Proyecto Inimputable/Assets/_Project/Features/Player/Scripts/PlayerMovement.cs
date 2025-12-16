using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.Diagnostics;

public class PlayerMovement : Player
{
    public CharacterController characterController;
    public Transform player;
    public Transform cam;
    public PlayerData playerData;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float gravity = -9.81f;

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        playerData.InputProcessed += OnInputProcessed;       
    }

    private void OnInputProcessed(Vector2 inputAmount, float playerSpeed,float aimX, float aimY)
    {
        Move(inputAmount, playerSpeed);
        RotateCamera(aimX, aimY);
    }

    public void Move(Vector2 moveInput, float speed)
    {

        Vector3 move = cam.right * moveInput.x + cam.forward * moveInput.y;
        move.y = 0f;

        characterController.Move(move * speed * Time.deltaTime);
    }

    public void RotateCamera(float yaw, float pitch)
    {
        player.localRotation = Quaternion.Euler(0f, yaw, 0f);
        cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public void Dash()
    {
        if (playerData.isDashing) return;

        Debug.Log("Player dashing");

    }

    void Update()
    {
        if (enabled != true) return;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        // First, we save the current state of the CharacterController (enabled or disabled).
        bool wasEnabled = characterController.enabled;
        
        // We MUST disable the CharacterController briefly. 
        // Why? Because if it's enabled, it will try to overwrite our manual position changes in the next frame.
        characterController.enabled = false;
        
        // Now safely move the player to the new position.
        player.position = position;

        // -- Rotation Logic --
        // In this game, the Player Body rotates Left/Right (Yaw), 


        // but the Camera rotates Up/Down (Pitch).
        
        // 1. Get the Y rotation (Yaw) from the target rotation and apply it to the body.
        float yaw = rotation.eulerAngles.y;
        player.localRotation = Quaternion.Euler(0f, yaw, 0f);

        // 2. Get the X rotation (Pitch) for the camera.
        float pitch = rotation.eulerAngles.x;

        // Unity angles go from 0 to 360. If the angle is > 180 (like 350 degrees), 
        // we convert it to a negative number (like -10 degrees) so it makes sense for our math.
        if (pitch > 180) pitch -= 360;

        cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // -- Important Sync --
        // We need to tell the PlayerData "Hey, we are looking this way now!"
        // If we don't update these variables, as soon as you touch the screen, 
        // the camera will snap back to where you were looking before the teleport.
        playerData.aimX = yaw;
        playerData.aimY = pitch;
        
        // Force the physics engine to update everything right now so there are no glitches.
        Physics.SyncTransforms(); 
        
        // Restore the CharacterController to its original state.
        characterController.enabled = wasEnabled;
    }
}