using UnityEngine;

public class EdgeSpawner : MonoBehaviour
{
    [Header("Spawn Area Settings")]
    public Vector2 spawnArea = new Vector2(19f, 15f); // match your scene limits
    public float spawnDistance = 1f; // how far outside screen they appear

    [Header("Spawn Rates (Seconds)")]
    public float componentSpawnRate = 1.5f;
    public float obstacleSpawnRate = 3.0f;

    [Header("Speed Settings")]
    public float minSpeed = 2f;
    public float maxSpeed = 4f;

    private float componentTimer = 0f;
    private float obstacleTimer = 0f;

    void Update()
    {
        componentTimer += Time.deltaTime;
        obstacleTimer += Time.deltaTime;

        // Spawn components
        if (componentTimer >= componentSpawnRate)
        {
            componentTimer = 0f;
            SpawnObject("Component");
        }

        // Spawn obstacles
        if (obstacleTimer >= obstacleSpawnRate)
        {
            obstacleTimer = 0f;
            SpawnObject("Obstacle");
        }
    }

    void SpawnObject(string tagToSpawn)
    {
        // randomly pick which side to spawn from (0 = top, 1 = left, 2 = right)
        int side = Random.Range(0, 3);
        Vector2 spawnPos = Vector2.zero;
        Vector2 moveDir = Vector2.zero;

        switch (side)
        {
            case 0: // TOP
                spawnPos = new Vector2(Random.Range(-spawnArea.x, spawnArea.x), spawnArea.y + spawnDistance);
                // move downward-left or downward-right
                moveDir = new Vector2(Random.Range(-0.7f, 0.7f), -1f).normalized;
                break;

            case 1: // LEFT
                spawnPos = new Vector2(-spawnArea.x - spawnDistance, Random.Range(-spawnArea.y, spawnArea.y));
                // move right-up or right-down
                moveDir = new Vector2(1f, Random.Range(-0.7f, 0.7f)).normalized;
                break;

            case 2: // RIGHT
                spawnPos = new Vector2(spawnArea.x + spawnDistance, Random.Range(-spawnArea.y, spawnArea.y));
                // move left-up or left-down
                moveDir = new Vector2(-1f, Random.Range(-0.7f, 0.7f)).normalized;
                break;
        }

        // get object from pool
        GameObject obj = ObjectPool.Instance.SpawnFromPool(tagToSpawn, spawnPos, Quaternion.identity);
        if (obj == null) return;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.drag = 0;
            rb.angularDrag = 0;
        }

        float speed = Random.Range(minSpeed, maxSpeed);
        rb.velocity = moveDir * speed;
        rb.angularVelocity = Random.Range(-60f, 60f);
    }
}