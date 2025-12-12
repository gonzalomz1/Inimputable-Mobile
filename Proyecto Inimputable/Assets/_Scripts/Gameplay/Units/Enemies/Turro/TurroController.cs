using System;
using UnityEngine;

public interface IPooledEnemy
{
    void ResetEnemy();
}

public class TurroController : MonoBehaviour, IPooledEnemy
{

    public enum EnemyType { Ranged, Melee }

    [Header("Enemy Type Settings")]
    public EnemyType enemyType;

    [Header("Model & View")]
    public TurroModel turroModel;
    public TurroView turroView;
    [Header("Event Messager")]
    public EventMessager eventMessager;
    
    [Header("Drops")]
    public GameObject[] pickups; // Healing/Ammo prefabs
    [Range(0f, 1f)] public float dropChance = 0.3f; // 30% chance to drop

    [Header("State to Begin")]
    public EnemyState beginState;

    [Header("AI Settings")]
    public float shootingDistance = 7f;
    public float meleeDistance = 2f;
    public float moveSpeed = 2f;
    private Transform playerTransform;
    public EnemyGun gun;

    public event Action Dead;

    void Awake()
    {
        Dead += OnTurroDead;
        eventMessager.ChangeLayer += OnEventMessagerChangeLayer;
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ResetStats();
        SetState(beginState);
    }

    private EnemyState GetTurroState()
    {
        return turroModel.GetTurroState();
    }

    public void SetState(EnemyState newState)
    {
        if (beginState == newState) return;
        turroModel.turroState = newState;
        Animator animator = turroView.GetAnimatorComponent();
        
        // Reset all triggers to avoid conflicts
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Move");
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Melee");

        switch (newState)
        {
            case EnemyState.Inactive:
                break;
            case EnemyState.Spawn:
                animator.SetTrigger("Spawn");
                break;
            case EnemyState.Idle:
                animator.SetTrigger("Idle");
                break;
            case EnemyState.Move:
                animator.SetTrigger("Move");
                break;
            case EnemyState.Shoot:
                animator.SetTrigger("Shoot");
                break;
            case EnemyState.Melee:
                animator.SetTrigger("Melee");
                break;
            case EnemyState.Die:
                turroModel.isDead = true;
                animator.SetTrigger("Die");
                break;
            case EnemyState.Destroy:
                // Cannot Destroy() pooled objects, must disable them.
                gameObject.SetActive(false);
                break;
        }
    }

    void Update() // TODO: check if this can be event-driven instead
    {
        EnemyState turroState = GetTurroState();
        // If one of this states, return.
        if (turroState == EnemyState.Inactive ||
            turroState == EnemyState.Spawn ||
            turroState == EnemyState.Die ||
            turroState == EnemyState.Melee || 
            turroState == EnemyState.Destroy)
            return;

        if(playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (!turroModel.isDead && turroModel.canChase)
        {
            if (enemyType == EnemyType.Ranged)
            {
                // Ranged behavior
                if (distanceToPlayer <= shootingDistance)
                {
                    SetState(EnemyState.Shoot);
                }
                else
                {
                    SetState(EnemyState.Move);
                    MoveTowardsPlayer();
                }
            }
            else if (enemyType == EnemyType.Melee)
            {
                // Melee behavior
                if (distanceToPlayer <= meleeDistance)
                {
                    SetState(EnemyState.Melee);
                }
                else
                {
                    SetState(EnemyState.Move);
                    MoveTowardsPlayer();
                }
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Optional: Look at player while moving
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, meleeDistance);
    }

    public void Shoot()
    {
        gun.Shoot();
    }

    public void Die()
    {
        Dead?.Invoke();
    }

    public void ResetEnemy()
    {
        ResetStats();
        SetState(EnemyState.Spawn);
    }

    public void Spawn()
    {
        ResetStats();
        SetState(EnemyState.Spawn);
    }


    private void ResetStats()
    {
        // Base stats
        int baseHealth = 100;
        int baseDamage = 10;
        float baseSpeed = 2f; 

        if (enemyType == EnemyType.Melee)
        {
            baseSpeed *= 1.25f;
        }
        
        // Apply difficulty buffs
        Color buffColor = Color.white;

        if (SurvivalDifficultyManager.Instance != null)
        {
            SurvivalDifficultyManager.EnemyBuff buff = SurvivalDifficultyManager.Instance.GetRandomBuff();
            // Debug.Log($"[TurroController] Buff Selected: {buff}"); // VERIFY THIS
            
            switch (buff)
            {
                case SurvivalDifficultyManager.EnemyBuff.Tank: // Blue
                    baseHealth *= 2;
                    buffColor = Color.blue;
                    Debug.LogError($"[ELITE SPAWN] TANK (Blue) Spawning! HP: {baseHealth}");
                    break;

                case SurvivalDifficultyManager.EnemyBuff.Speed: // Green
                    baseSpeed *= 1.5f;
                    buffColor = Color.green;
                     Debug.LogError($"[ELITE SPAWN] SPEED (Green) Spawning! Speed: {baseSpeed}");
                    break;

                case SurvivalDifficultyManager.EnemyBuff.Damage: // Red
                    baseDamage *= 2;
                    buffColor = Color.red;
                     Debug.LogError($"[ELITE SPAWN] DAMAGE (Red) Spawning! Dmg: {baseDamage}");
                    break;
                
                case SurvivalDifficultyManager.EnemyBuff.None:
                default:
                    buffColor = Color.white;
                    // Debug.Log("[TurroController] Normal Enemy Spawned");
                    break;
            }
        }
        else
        {
            Debug.LogError("[TurroController] SurvivalDifficultyManager Instance is NULL!");
        }

        // Finalize stats
        turroModel.health = baseHealth;
        turroModel.damage = baseDamage;
        moveSpeed = baseSpeed;

        turroModel.isDead = false;
        turroModel.canChase = false;
        turroModel.isTakingDamage = false;
        if (gameObject.layer == 15) //stompable
        {
            gameObject.layer = 6; // Reset layer if it was changed to Stompable
        }
        
        if (turroView != null)
        {
             turroView.ShowSprite();
             turroView.SetColor(buffColor);
        }

        // Initialize Health Bar
        turroView.InitializeHealthBar(turroModel.health);
    }

    private void OnTurroDead()
    {
        // Loot drop
        if (UnityEngine.Random.value <= dropChance)
        {
            if (Spawner.instance != null)
            {
                // Spawn Generic Item from Pool
                GameObject itemObj = Spawner.instance.SpawnItem("Items", transform.position + Vector3.up * 0.5f, Quaternion.identity);
                
                if (itemObj != null)
                {
                    PickupItem pickup = itemObj.GetComponent<PickupItem>();
                    if (pickup != null)
                    {
                        // Randomize Type (50/50 for now, or customize)
                        PickupItem.PickupType randomType = (UnityEngine.Random.value > 0.5f) ? PickupItem.PickupType.Health : PickupItem.PickupType.Ammo;
                        pickup.Configure(randomType);
                    }
                }
            }
        }

        // Hide Health Bar
        turroView.ToggleHealthBar(false);
        SetState(EnemyState.Die);
    }

    private void OnEventMessagerChangeLayer()
    {
        gameObject.layer = 15; // stompable
    }
}
