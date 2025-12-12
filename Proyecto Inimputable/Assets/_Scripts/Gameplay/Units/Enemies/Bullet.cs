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

    private PlayerData _cachedPlayer;

    void Start()
    {
        // Try to cache player on start
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _cachedPlayer = playerObj.GetComponent<PlayerData>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            // Use cached component if available, otherwise find it. 
            // This prevents doing GetComponent calls on every single bullet hit, saving performance.
            if (_cachedPlayer == null)
            {
                _cachedPlayer = collision.gameObject.GetComponent<PlayerData>();
            }

            if (_cachedPlayer != null)
            {
                _cachedPlayer.TakeDamage(damage);
            }

            ReturnToPool();
        }
        else
        {
            // Hit a wall or obstacle
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