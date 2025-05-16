using UnityEngine;

public class TurroModel : MonoBehaviour
{
    TurroController turroController;
    public int health = 30;
    public int damage = 5;

    public bool isDead = false;
    public bool canChase = false;

    public void TakeDamage(int amount)
    {
        // Cambiar color base a rojo
        health -= amount;
        CheckHealth();
    }

    public void CheckHealth()
    {
        if (health <= 0)
        {
            
        }
    }
}
