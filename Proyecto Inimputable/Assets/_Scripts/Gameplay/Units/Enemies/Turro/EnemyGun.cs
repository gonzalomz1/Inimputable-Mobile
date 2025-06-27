using UnityEngine;
public class EnemyGun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 5f;
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Shoot()
    {
        if (player == null) return;

        bulletSpawnPoint.LookAt(player.transform);
        Vector3 spawnPos = bulletSpawnPoint.position + bulletSpawnPoint.forward * 0.5f;

        GameObject bullet = ObjectPooler.Instance.SpawnFromPool("EnemyBullet", spawnPos, bulletSpawnPoint.rotation);
        if (bullet != null)
        {
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
            }
        }
    }
}