using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 3;
    public string poolTag = "EnemyBullet"; // El tag usado en ObjectPooler

    private Coroutine returnCoroutine;

    void OnEnable()
    {
        // Comenzar el temporizador para regresar al pool
        returnCoroutine = StartCoroutine(ReturnAfterTime(lifeTime));
    }

    void OnDisable()
    {
        // Cancelar el temporizador si se desactiva manualmente antes
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerData>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            ReturnToPool();
        }
        else
        {
            // Si impacta cualquier otra cosa, también vuelve al pool
            ReturnToPool();
        }
    }

    private IEnumerator ReturnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        ObjectPooler.Instance.ReturnToPool(poolTag, gameObject);
    }
}