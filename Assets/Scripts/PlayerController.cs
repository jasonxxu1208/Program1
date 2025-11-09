using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;
using System;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 4f;
    public GameObject boosterFlame;
    Rigidbody2D rb;
    public UIDocument uiDocument;
    public GameObject explosionEffect;
    private Button restartButton;
    private Button startButton;
    public bool gameStarted = false;
    private Button exitButton;
    public bool isPaused = false;
    private Button continueButton;


    public GameObject bulletPrefab;
    public Transform gunPoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float fireTimer = 0f;
    public GameObject shieldPrefab;  
    public Transform shieldPoint;   
    public float shieldRate = 5f;
    private float shieldTimer = 0f;
    public int maxCargo = 10;             // maximum components rocket can carry
    private int currentCargo = 0;         // current amount carried

    private ProgressBar cargoBar; 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 0f;
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
        continueButton.style.display = DisplayStyle.None;
        continueButton.clicked += ResumeGame;
        cargoBar = uiDocument.rootVisualElement.Q<ProgressBar>("CargoBar");
        currentCargo = 0;
        UpdateCargoBar();
        SetComponentCollidersActive(true);
        if (cargoBar != null)
        {
            cargoBar.lowValue = 0;
            cargoBar.highValue = maxCargo;
            cargoBar.value = 0;
            cargoBar.title = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        shieldTimer += Time.deltaTime;

        if (Keyboard.current.jKey.isPressed && shieldTimer >= shieldRate)
        {
            ActivateShield();
            shieldTimer = 0f;
        }
       if (Keyboard.current.spaceKey.isPressed && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
        
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
<<<<<<< Updated upstream
            if (isPaused) ResumeGame();
            else PauseGame();
=======
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
>>>>>>> Stashed changes
        }

        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            moveInput.y += 1;
            boosterFlame.SetActive(true);
        }
        if (Keyboard.current.sKey.isPressed)
        {
            moveInput.y -= 1;
            boosterFlame.SetActive(true);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            moveInput.x -= 1;
            boosterFlame.SetActive(true);
        } 
        if (Keyboard.current.dKey.isPressed)
        {
            moveInput.x += 1;
            boosterFlame.SetActive(true);
        } 

        moveInput.Normalize();
        rb.velocity = moveInput * moveSpeed * (1f - ((float)currentCargo / maxCargo)*0.5f);
        Debug.Log("current speed" + rb.velocity.magnitude);
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
        restartButton.style.display = DisplayStyle.Flex;
        exitButton.style.display = DisplayStyle.Flex;
<<<<<<< Updated upstream
        exitButton.clicked += ExitGame;
=======
        saveButton.style.display = DisplayStyle.Flex;
>>>>>>> Stashed changes
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
    
<<<<<<< Updated upstream
    
=======
    void SaveGame()
    {
        SaveSystem.SaveGame(transform);
        ExitGame();
    }

    void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data != null)
        {
            transform.position = new Vector3(data.playerX, data.playerY);

            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Obstacle"))
                Destroy(o);

            foreach (ObstacleData o in data.obstacles)
            {
                GameObject newObstacle = Instantiate(Resources.Load<GameObject>("Obstacle"));
                newObstacle.transform.position = new Vector3(o.x, o.y, 0);
                newObstacle.transform.localScale = new Vector3(o.size, o.size, 1);
            }

        }
        ResumeGame();
        startButton.style.display = DisplayStyle.None;
        loadButton.style.display = DisplayStyle.None;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Component"))
        {
            if (currentCargo < maxCargo)
            {
                currentCargo++;
                UpdateCargoBar();
                Debug.Log("Collected component! " + currentCargo + "/" + maxCargo);

                other.gameObject.SetActive(false); // deactivate (return to pool)

                // If we've just filled up cargo, disable all component colliders
                if (currentCargo >= maxCargo)
                {
                    Debug.Log("Cargo full — disabling component collection!");
                    SetComponentCollidersActive(false);
                }
            }
            else
            {
                Debug.Log("Cargo full! Ignoring component.");
            }
        }
        if (other.CompareTag("Portal"))
        {
            ExitGame();
        }
    }

    void UpdateCargoBar()
    {
        if (cargoBar != null)
        {
            cargoBar.value = currentCargo;
        }
    }
    void SetComponentCollidersActive(bool state)
    {
        // Get all active and inactive objects with tag "Component"
        GameObject[] components = GameObject.FindGameObjectsWithTag("Component");

        foreach (GameObject comp in components)
        {
            Collider2D col = comp.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = state;
        }
    }
>>>>>>> Stashed changes
}
