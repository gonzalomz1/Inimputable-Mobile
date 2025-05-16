using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TurroModel : MonoBehaviour
{
    TurroController turroController;
    TurroView turroView;
    public int health = 150;
    public int damage = 5;

    public bool isDead = false;
    public bool canChase = false;
    [SerializeField] private float damageCooldown = 0.2f;
    private bool isTakingDamage = false;
    void Awake()
    {
        turroController = GetComponent<TurroController>();
        turroView = GetComponent<TurroView>();
    }

    public void TakeDamage(int amount)
    {
        if (isTakingDamage) return;
        if (CanTakeDamage())
        {
            health -= amount;
            StartCoroutine(DamageCooldownRoutine());
            turroView.FlashDamageColor();
            CheckHealth();
        }
        else return;


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
        return (
            turroController.turroState != EnemyState.Idle ||
            turroController.turroState != EnemyState.Shoot ||
            turroController.turroState != EnemyState.Move);
    }

    private IEnumerator DamageCooldownRoutine()
{
    isTakingDamage = true;
    yield return new WaitForSeconds(damageCooldown);
    isTakingDamage = false;
}
}
