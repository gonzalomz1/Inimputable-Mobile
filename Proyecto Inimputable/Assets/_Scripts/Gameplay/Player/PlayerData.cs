using UnityEngine;

public class PlayerData : Player
{
    [Header("Flags")]
    public bool isDashing = false;
    public bool isMoving = false;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public Vector2 currentMoveInput = Vector2.zero;

    [Header("Camera Data")]
    public float lookSensitivity = 0.15f;
    [HideInInspector] public float cameraPitch = 0f; // Vertical
    public float currentYaw;
    public float currentPitch;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth = 50f;
    public float mana = 100f;
    public float inimputability = 100f;
    public float dashStrength = 1f;
    public float dashDistance = 10f;
    
}
