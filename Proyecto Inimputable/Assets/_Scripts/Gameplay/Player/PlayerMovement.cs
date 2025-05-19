using UnityEngine;

public class PlayerMovement : Player 
{
    public CharacterController controller;
    public Transform player;
    public Transform cam;
    public PlayerData playerData;

    public void Move(Vector2 moveInput, float speed)
    {
        // Convertimos moveInput a un vector 3D relativo a la cámara
        Vector3 move = cam.right * moveInput.x + cam.forward * moveInput.y;
        move.y = 0f;

        controller.Move(move * speed * Time.deltaTime);
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

}