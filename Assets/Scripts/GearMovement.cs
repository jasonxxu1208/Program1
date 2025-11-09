using UnityEngine;

public class GearMovement : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float minRotationSpeed = 30f;   // degrees per second
    public float maxRotationSpeed = 120f;  // degrees per second

    private float rotationSpeed;

    void OnEnable()
    {
        // Random spin speed and direction
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        if (Random.value < 0.5f)
            rotationSpeed *= -1;
    }

    void Update()
    {
        // Continuous spin (independent of movement)
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}