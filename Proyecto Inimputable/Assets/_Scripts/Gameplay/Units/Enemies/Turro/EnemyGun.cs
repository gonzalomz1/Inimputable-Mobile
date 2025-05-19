using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 5;
    public GameObject player;

    //public Transform forwardCube;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Shoot()
    {
        if (player == null) return;
        bulletSpawnPoint.LookAt(player.transform);

        Vector3 spawnPos = bulletSpawnPoint.position + bulletSpawnPoint.forward * 0.5f;
        var bullet = Instantiate(bulletPrefab, spawnPos, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;


    }

}
