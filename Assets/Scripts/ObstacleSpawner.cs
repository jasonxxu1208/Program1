using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obstaclePrefab;
    public float spawnInterval = 5f;
    private float timer = 0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }
    void SpawnObstacle()
    {
        Vector2 spawnPos = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }
}
