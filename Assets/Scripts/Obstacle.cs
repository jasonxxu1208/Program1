using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float maxSpinSpeed = 90f;
    private Rigidbody2D rb;

    void OnEnable()
    {
        // Randomize size when activated (not just once at Start)
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.angularVelocity = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        }
    }
}
