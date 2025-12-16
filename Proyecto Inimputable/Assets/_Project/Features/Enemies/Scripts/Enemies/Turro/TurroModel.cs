using System;
using System.Collections;
using UnityEngine;

public class TurroModel : MonoBehaviour
{
    TurroController turroController;
    TurroView turroView;
    [Header("Current State")]
    public EnemyState turroState;
    [Header("Stats")]
    public int health = 150;
    public int damage = 5;

    [SerializeField] private float damageCooldown = 0.2f;
    
    [Header("Flags")]
    public bool isDead = false;
    public bool canChase = false;
    public bool isStompable => turroState == EnemyState.Dead;
    public bool isTakingDamage = false;

    void Awake()
    {
        turroController = GetComponent<TurroController>();
        turroView = GetComponent<TurroView>();
        turroController.Dead += OnModelTurroDead;
    }

    public void TakeDamage(int amount)
    {
        if (isTakingDamage) return;
        if (CanTakeDamage())
        {
            health -= amount;
            StartCoroutine(DamageCooldownRoutine());
            turroView.FlashDamageColor();
            turroView.TakeDamageSound();
            turroView.UpdateHealthBar(health);
            CheckHealth();
        }
        else return;
    }

    public EnemyState GetTurroState()
    {
        return turroState;
    }

    public void CheckHealth()
    {
        if (health <= 0)
        {
            turroController.Die();
        }
    }

    public bool CanTakeDamage()
    {
        return

            CheckTrueForTakeDamage();

    }

    private bool CheckTrueForTakeDamage()
    {
        return (
        turroState == EnemyState.Idle ||
        turroState == EnemyState.Move ||
        turroState == EnemyState.Shoot ||
        turroState == EnemyState.Melee
        );
    }

    private IEnumerator DamageCooldownRoutine()
    {
        isTakingDamage = true;
        yield return new WaitForSeconds(damageCooldown);
        isTakingDamage = false;
    }

    private void OnModelTurroDead()
    {

    }
}
