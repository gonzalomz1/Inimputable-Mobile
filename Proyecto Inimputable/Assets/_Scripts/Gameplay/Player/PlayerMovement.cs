using UnityEngine;
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

    public void Move(Vector2 moveInput, float speed)
    {
        // Convertimos moveInput a un vector 3D relativo a la cámara
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
        //playerData.isDashing = true;

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

}