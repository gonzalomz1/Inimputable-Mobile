using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log($"collision: {collision}");
            Debug.Log($"collision.gameObject: {collision.gameObject}");
            Debug.Log($"collision.gameObject: {collision.gameObject.GetComponent<PlayerData>()}");
            collision.gameObject.GetComponent<PlayerData>().TakeDamage(5);
            Destroy(gameObject);
        }
    }
}
