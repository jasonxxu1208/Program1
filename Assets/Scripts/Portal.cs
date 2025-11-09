using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Movement Settings")]
    public float fallSpeed = 2.0f; // match rocket’s base speed

    void Update()
    {
        // move vertically downward
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);

        // deactivate or destroy if out of bounds
        if (transform.position.y < -15f)
        {
            Destroy(gameObject);
        }
    }

}
