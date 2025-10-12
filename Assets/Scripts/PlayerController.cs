using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private float elapsedTime = 0f;
    private float score = 0f;
    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;
    public float maxSpeed = 10f;
    public GameObject boosterFlame;
    Rigidbody2D rb;
    public UIDocument uiDocument;
    private Label scoreText;
    public GameObject explosionEffect;
    private Button restartButton;
    private Button startButton;
    private bool gameStarted = false;
    private Button exitButton;
    private bool isPaused = false;
    private Button continueButton;


    public GameObject bulletPrefab;
    public Transform gunPoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float fireTimer = 0f;
    public float sprintMultiplier = 2f;
    public float sprintActive = 0f;
    public float sprintDuration = 0.5f;     
    public float sprintRate = 2f;      
    private float sprintTimer = 0f;
    private float originalThrust;
    public GameObject shieldPrefab;  
    public Transform shieldPoint;   
    public float shieldRate = 5f;
    private float shieldTimer = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 0f;
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        startButton = uiDocument.rootVisualElement.Q<Button>("StartButton");
        exitButton = uiDocument.rootVisualElement.Q<Button>("ExitButton");
        continueButton = uiDocument.rootVisualElement.Q<Button>("ContinueButton");
        startButton.style.display = DisplayStyle.Flex;
        startButton.clicked += StartGame;
        continueButton.style.display = DisplayStyle.None;
        exitButton.style.display = DisplayStyle.Flex;
        exitButton.clicked += ExitGame;
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
        originalThrust = thrustForce;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        sprintTimer += Time.deltaTime;
        shieldTimer += Time.deltaTime;

        if (Keyboard.current.sKey.isPressed && shieldTimer >= shieldRate)
        {
            ActivateShield();
            shieldTimer = 0f;
        }
        if (Keyboard.current.leftShiftKey.isPressed && sprintTimer >= sprintRate)
        {
            Sprint();
            sprintTimer = 0f;
        }

        if (Keyboard.current.spaceKey.isPressed && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
        
        if (Keyboard.current.escapeKey.isPressed)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        Debug.Log("Score: " + score);
        scoreText.text = "Score: " + score;
        if (Mouse.current.leftButton.isPressed)
        {
            // Calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;
            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlame.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        exitButton.style.display = DisplayStyle.Flex;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void StartGame()
    {
        Time.timeScale = 1f;
        gameStarted = true;
        startButton.style.display = DisplayStyle.None;
        exitButton.style.display = DisplayStyle.None;
    }
    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // freeze time
        continueButton.style.display = DisplayStyle.Flex;
        continueButton.clicked += ResumeGame;
        restartButton.style.display = DisplayStyle.Flex;
        restartButton.clicked += ReloadScene;
        exitButton.style.display = DisplayStyle.Flex;
        exitButton.clicked += ExitGame;
    }

    void ResumeGame()
    {
        isPaused = false;
        continueButton.style.display = DisplayStyle.None;
        restartButton.style.display = DisplayStyle.None;
        exitButton.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
    }

    void Shoot()
    {
        if (bulletPrefab != null && gunPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);

            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            if (rbBullet != null)
            {
                rbBullet.velocity = gunPoint.up * bulletSpeed;
            }
        }
    }

    void Sprint()
    {


        while (sprintActive <= sprintDuration)
        {
            sprintActive += Time.deltaTime;
            thrustForce = originalThrust * sprintMultiplier;
        }
        thrustForce = originalThrust;
        sprintActive = 0f;

    }

    void ActivateShield()
    {
        if (shieldPrefab == null || shieldPoint == null)
            return;
        GameObject shield = Instantiate(shieldPrefab, shieldPoint.position, shieldPoint.rotation);
        shield.transform.SetParent(shieldPoint);
    }
    void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    
}
