using System;
using UnityEngine;

public class EventMessager : MonoBehaviour
{
    public TurroController turroController;
    public TurroModel turroModel;
    public TurroView turroView;

    public event Action ChangeLayer;

    void Awake()
    {
        if (turroController == null)
        {
            var miController = GetComponentInParent<TurroController>();
            if (miController != null) turroController = miController;
        }
        if (turroModel == null)
        {
            var miModel = GetComponentInParent<TurroModel>();
            if (miModel != null) turroModel = miModel;
        }
        if (turroView == null)
        {
            var miView = GetComponentInParent<TurroView>();
            if (miView != null) turroView = miView;
        }
    }

    public void TriggerIdleState()
    {
        turroController.SetState(EnemyState.Idle);
    }

    public void EnableChase()
    {
        Debug.Log($"canChace: {turroModel.canChase}");
        turroModel.canChase = true;
        Debug.Log($"canChace: {turroModel.canChase}");
    }

    public void ShowSprite()
    {
        turroView.ShowSprite();
    }

    public void TriggerShoot()
    {
        turroController.Shoot();
    }

    public void SetEnemyAsDead()
    {
        ChangeLayer?.Invoke();

    }

    private PlayerData _cachedPlayer;

    private void Start()
    {
        // Cache PlayerData since it is constant
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _cachedPlayer = playerObj.GetComponent<PlayerData>();
        }
    }

    // Called via Animation Event when the Melee punch hits
    public void TriggerMelee()
    {
        // Double check range to allow player to dodge at the last second
        if (IsPlayerInRange())
        {
             _cachedPlayer.TakeDamage(turroModel.damage);
             Debug.Log("Melee Hit! Damage Dealt.");
        }
        else
        {
            // If player moved away, break the attack and resume chasing immediately
            turroController.SetState(EnemyState.Move);
        }
    }

    private bool IsPlayerInRange()
    {
        if (_cachedPlayer == null) return false;

        float distance = Vector3.Distance(transform.position, _cachedPlayer.transform.position);
        float range = turroController.meleeDistance + 0.5f; // Tolerance buffer
        
        return distance <= range;
    }

    public void TriggerDestroyState()
    {
        turroController.SetState(EnemyState.Destroy);
    }
}
