using UnityEngine;

public class TurroModel : MonoBehaviour
{
    public int health = 30;

    public void TakeDamage(int amount)
    {
        health -= amount;
    }

}
