using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMovement : MonoBehaviour
{
    public float lifetime = 2f;
    void Start()
    {
         Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
