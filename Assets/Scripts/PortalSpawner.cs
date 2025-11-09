using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [Header("Portal Settings")]
    public GameObject portalPrefab;      // assign in Inspector
    public float spawnInterval = 60f;    // every 1 minute
    public Vector2 xRange = new Vector2(-18f, 18f);
    public float spawnY = 15f;           // spawn just above top border

    private GameObject activePortal;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPortal();
            timer = 0f;
        }
    }

    void SpawnPortal()
    {
        if (activePortal != null)
            Destroy(activePortal);

        Vector3 randomPos = new Vector3(Random.Range(xRange.x, xRange.y), spawnY, 0f);
        activePortal = Instantiate(portalPrefab, randomPos, Quaternion.identity);
        Debug.Log($"🌀 Portal spawned at {randomPos}");
    }
}