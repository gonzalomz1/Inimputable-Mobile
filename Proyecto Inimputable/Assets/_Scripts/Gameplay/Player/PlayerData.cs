using UnityEngine;

public class PlayerData : Player
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public Vector2 currentMoveInput = Vector2.zero;

    [Header("Camera Data")]
    public float lookSensitivity = 0.15f;
    [HideInInspector] public float cameraPitch = 0f; // Vertical
    public float currentYaw;
    public float currentPitch;
    
}
