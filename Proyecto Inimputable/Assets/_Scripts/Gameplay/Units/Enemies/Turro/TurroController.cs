using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledEnemy
{
    void ResetEnemy();
}
public class TurroController : MonoBehaviour, IPooledEnemy
{

    [Header("Model & View")]
    public TurroModel turroModel;
    public TurroView turroView;

    [Header("Current State")]
    public EnemyState turroState;

    [Header("AI Settings")]
    public float shootingDistance = 7f;
    public float moveSpeed = 2f;
    private Transform playerTransform;
    public EnemyGun gun;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetState(turroState);
    }

    public void SetState(EnemyState newState)
    {
        if (turroState == newState) return; // avoid unnecesary calls.
        Animator animator = turroView.animator;
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Move");
        animator.ResetTrigger("Shoot");

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
            case EnemyState.Die:
                turroModel.isDead = true;
                animator.SetTrigger("Die");
                break;
            case EnemyState.Destroy:
                Destroy(this.gameObject);
                break;
        }
    }

    void Update() // podria ser llamado de animacion en vez de update.
    {
        // If one of this states, return.
        if (turroState == EnemyState.Inactive ||
            turroState == EnemyState.Spawn ||
            turroState == EnemyState.Die ||
            turroState == EnemyState.Destroy)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (!turroModel.isDead && turroModel.canChase)
        {
            if (distanceToPlayer <= shootingDistance)
            {
                SetState(EnemyState.Shoot);
                turroState = EnemyState.Shoot;
            }
            else
            {
                SetState(EnemyState.Move);
                turroState = EnemyState.Move;
                MoveTowardsPlayer();
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingDistance);
    }

    public void Shoot()
    {
        gun.Shoot();
    }

    public void Die()
    {
        SetState(EnemyState.Die);
        turroState = EnemyState.Die;
    }

    public void ResetEnemy()
    {
        SetState(EnemyState.Spawn);
    }

    public void Spawn()
    {
        SetState(EnemyState.Spawn);
    }
}
