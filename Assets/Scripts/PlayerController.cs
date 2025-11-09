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
    public float moveSpeed = 4f;
    public GameObject boosterFlame;
    private Rigidbody2D rb;
    public UIDocument uiDocument;
    public GameObject explosionEffect;

    private Button restartButton;
    private Button startButton;
    private Button exitButton;
    private Button continueButton;

    public bool gameStarted = false;
    public bool isPaused = false;

    public GameObject bulletPrefab;
    public Transform gunPoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float fireTimer = 0f;

    public GameObject shieldPrefab;
    public Transform shieldPoint;
    public float shieldRate = 5f;
    private float shieldTimer = 0f;

    public int maxCargo = 10;
    private int currentCargo = 0;
    private ProgressBar cargoBar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 0f;

        // UI setup
        var root = uiDocument.rootVisualElement;
        restartButton = root.Q<Button>("RestartButton");
        startButton = root.Q<Button>("StartButton");
        exitButton = root.Q<Button>("ExitButton");
        continueButton = root.Q<Button>("ContinueButton");

        startButton.style.display = DisplayStyle.Flex;
        startButton.clicked += StartGame;

        continueButton.style.display = DisplayStyle.None;
        continueButton.clicked += ResumeGame;

        exitButton.style.display = DisplayStyle.Flex;
        exitButton.clicked += ExitGame;

        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;

        cargoBar = root.Q<ProgressBar>("CargoBar");
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
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
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
        rb.velocity = moveInput * moveSpeed * (1f - ((float)currentCargo / maxCargo) * 0.5f);

        Debug.Log("Current speed: " + rb.velocity.magnitude);
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
        Time.timeScale = 0f;
        continueButton.style.display = DisplayStyle.Flex;
        restartButton.style.display = DisplayStyle.Flex;
        exitButton.style.display = DisplayStyle.Flex;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Component"))
        {
            if (currentCargo < maxCargo)
            {
                currentCargo++;
                UpdateCargoBar();
                Debug.Log("Collected component! " + currentCargo + "/" + maxCargo);

                other.gameObject.SetActive(false);

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
        GameObject[] components = GameObject.FindGameObjectsWithTag("Component");

        foreach (GameObject comp in components)
        {
            Collider2D col = comp.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = state;
        }
    }
}
