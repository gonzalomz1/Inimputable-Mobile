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

    void Start()
    {
        sensitivityY = sensitivityX * ySensitivityRatio;
    }

    public void TakeDamage(int damage)
    {
        print(currentHealth);
        currentHealth -= damage;
        print(currentHealth);
        uIPlayerStats.SetBarCurrentValue(uIPlayerStats.healthBar, currentHealth);
        if (IsPlayerDead())
        {
            GameObject gameFlowObject = GameObject.FindWithTag("GameFlow");
            gameFlowObject.GetComponent<GameFlowManager>().GameOver();
        }
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
}
