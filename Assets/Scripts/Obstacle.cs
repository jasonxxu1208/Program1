using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    Rigidbody2D rb;
    public float minSpeed = 50f;
    public float maxSpeed = 150f;
    public float maxSpinSpeed = 10f;

    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);


        rb = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = Random.insideUnitCircle;
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        rb.AddForce(randomDirection * randomSpeed);
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
