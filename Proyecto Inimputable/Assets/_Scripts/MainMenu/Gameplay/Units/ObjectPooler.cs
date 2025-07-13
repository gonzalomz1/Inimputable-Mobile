using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        Queue<GameObject> objectPool = poolDictionary[tag];
        GameObject[] poolArray = objectPool.ToArray();

        // Intentar encontrar un objeto inactivo, comenzando por un índice aleatorio
        int startIndex = Random.Range(0, poolArray.Length);

        for (int i = 0; i < poolArray.Length; i++)
        {
            int index = (startIndex + i) % poolArray.Length;
            GameObject candidate = poolArray[index];

            if (!candidate.activeInHierarchy)
            {
                candidate.SetActive(true);
                candidate.transform.position = position;
                candidate.transform.rotation = rotation;

                // Encolar nuevamente para mantener el orden rotatorio
                objectPool.Enqueue(objectPool.Dequeue());
                return candidate;
            }

            // Rota el pool para evitar repeticiones
            objectPool.Enqueue(objectPool.Dequeue());
        }

        Debug.LogWarning("No inactive objects available in pool: " + tag);
        return null;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Trying to return to non-existent pool: " + tag);
            return;
        }

        obj.SetActive(false);
    }
}