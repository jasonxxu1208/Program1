using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    [Header("Optional Auto-Expand")]
    public bool allowAutoExpand = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
            Debug.LogWarning($"Pool with tag {tag} doesn't exist!");
            return null;
        }

        GameObject objectToSpawn = null;
        Queue<GameObject> poolQueue = poolDictionary[tag];

        // Find an inactive object
        int poolCount = poolQueue.Count;
        for (int i = 0; i < poolCount; i++)
        {
            GameObject candidate = poolQueue.Dequeue();
            poolQueue.Enqueue(candidate);

            if (!candidate.activeInHierarchy)
            {
                objectToSpawn = candidate;
                break;
            }
        }

        // If all are active, optionally expand pool
        if (objectToSpawn == null)
        {
            if (allowAutoExpand)
            {
                Pool poolDef = pools.Find(p => p.tag == tag);
                objectToSpawn = Instantiate(poolDef.prefab);
                poolQueue.Enqueue(objectToSpawn);
                Debug.Log($"[ObjectPool] Auto-expanded pool for {tag}");
            }
            else
            {
                Debug.LogWarning($"[ObjectPool] All {tag} objects are active!");
                return null;
            }
        }

        // Activate and position
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }
}