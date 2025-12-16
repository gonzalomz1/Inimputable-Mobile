using System;
using UnityEngine;

public class PlayerData : Player
{
    [Header("Player Presenter")]
    [SerializeField] private PlayerPresenter playerPresenter;
    [Header("Player Movement")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Flags")]
    public bool isDashing = false;
    public bool isMoving = false;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public Vector2 currentMoveInput = Vector2.zero;

    [Header("Camera Data")]
    public float sensitivityX = 5f;
    public float sensitivityY; // on start, we change the value
    public float ySensitivityRatio = 0.6f;

    public float aimX = 0f;
    public float aimY = 0f;
    public bool isAimAssistActive;

    [Header("Stats")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int mana = 100;
    public int inimputability = 100;
    public float dashStrength = 1f;
    public float dashDistance = 10f;

    public UIPlayerStats uIPlayerStats;

    public event Action<Vector2, float, float, float> InputProcessed;

    void Start()
    {
        sensitivityY = sensitivityX * ySensitivityRatio;

        SubscribeToPlayerPresenterEvents();
    }

    private void SubscribeToPlayerPresenterEvents()
    {
        playerPresenter.SendInputData += OnPlayerPresenterSendInputDataEvent;
    }

    public void TakeDamage(int damage)
    {
        print(currentHealth);
        currentHealth -= damage;
        print(currentHealth);
        uIPlayerStats.SetBarCurrentValue(uIPlayerStats.healthBar, currentHealth);
        if (IsPlayerDead())
        {
            playerPresenter.PlayerDead();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"[PlayerData] Healed {amount} HP. Current: {currentHealth}/{maxHealth}");
        
        if (uIPlayerStats != null)
        {
             uIPlayerStats.SetBarCurrentValue(uIPlayerStats.healthBar, currentHealth);
        }
        else
        {
            Debug.LogError("[PlayerData] uIPlayerStats reference is NULL! Cannot update Health Bar.");
        }
    }

    public void Revive(Vector3 position, Quaternion rotation)
    {
        currentHealth = maxHealth;
        isAimAssistActive = false;
        
        // Update UI
        if (uIPlayerStats != null)
            uIPlayerStats.SetBarCurrentValue(uIPlayerStats.healthBar, currentHealth);

        // Reset Position
        // Ensure position isn't overridden by physics/navmesh
        if (playerMovement != null)
        {
            playerMovement.Teleport(position, rotation);
        }
        else
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        // Reset Input
        currentMoveInput = Vector2.zero;
        
        Debug.Log("[PlayerData] Player Revived at " + position);
    }

    public bool IsPlayerDead()
    {
        return currentHealth <= 0;
    }

    public void ChangeSensitivity(float amount)
    {
        sensitivityX = amount;
        sensitivityY = sensitivityX * ySensitivityRatio;
    }

    public float GetHealthRatio()
    {
        if (maxHealth <= 0) return 0f;
        return Mathf.Clamp01((float)currentHealth / maxHealth);
    }

    private void OnPlayerPresenterSendInputDataEvent(Vector2 amount)
    {
        ModifyMovementAmount(amount);
    }

    public void ModifyMovementAmount(Vector2 amount)
    {
        currentMoveInput = amount;
        InputProcessed?.Invoke(amount, moveSpeed, aimX, aimY);
    }

    public void SetPlayerPhysicalState(bool isEnabled)
    {
        playerMovement.enabled = isEnabled;
    }
}
