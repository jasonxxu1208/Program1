using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Combat Settings")]
    public GameObject bulletPrefab;
    public Transform gunPoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float fireTimer = 0f;

    [Header("Shield Settings")]
    public GameObject shieldPrefab;
    public Transform shieldPoint;
    public float shieldRate = 5f;
    private float shieldTimer = 0f;

    [Header("Cargo Settings")]
    public int maxCargo = 10;
    private int currentCargo = 0;
    private ProgressBar cargoBar;

    [Header("UI & FX")]
    public UIDocument uiDocument;
    public GameObject explosionEffect;

    private Button startButton;
    private Button restartButton;
    private Button continueButton;
    private Button exitButton;
    private Button saveButton;
    private Button loadButton;

    private bool isPaused = false;
    private bool gameStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Start paused until player clicks Start
        Time.timeScale = 0f;
        SetupUI();
        UpdateCargoBar();
        SetComponentCollidersActive(true);
    }

    void SetupUI()
    {
        var root = uiDocument.rootVisualElement;

        startButton = root.Q<Button>("StartButton");
        restartButton = root.Q<Button>("RestartButton");
        continueButton = root.Q<Button>("ContinueButton");
        exitButton = root.Q<Button>("ExitButton");
        saveButton = root.Q<Button>("SaveButton");
        loadButton = root.Q<Button>("LoadButton");
        cargoBar = root.Q<ProgressBar>("CargoBar");

        // Assign button callbacks
        startButton.clicked += StartGame;
        restartButton.clicked += ReloadScene;
        continueButton.clicked += ResumeGame;
        exitButton.clicked += ExitGame;
        saveButton.clicked += SaveGame;
        loadButton.clicked += LoadGame;

        // Show only start menu initially
        startButton.style.display = DisplayStyle.Flex;
        loadButton.style.display = DisplayStyle.Flex;
        exitButton.style.display = DisplayStyle.Flex;

        restartButton.style.display = DisplayStyle.None;
        continueButton.style.display = DisplayStyle.None;
        saveButton.style.display = DisplayStyle.None;

        if (cargoBar != null)
        {
            cargoBar.lowValue = 0;
            cargoBar.highValue = maxCargo;
            cargoBar.value = 0;
            cargoBar.title = "Cargo";
        }
    }

    void Update()
    {
        if (!gameStarted) return;

        fireTimer += Time.deltaTime;
        shieldTimer += Time.deltaTime;

        HandleInput();
    }

    void FixedUpdate()
    {
        if (!gameStarted || isPaused) return;
        float speedMultiplier = 1f - ((float)currentCargo / maxCargo) * 0.5f;
        rb.velocity = moveInput * moveSpeed * speedMultiplier;
    }

    void HandleInput()
    {
        var kb = Keyboard.current;
        moveInput = Vector2.zero;

        if (kb.wKey.isPressed) moveInput.y += 1;
        if (kb.sKey.isPressed) moveInput.y -= 1;
        if (kb.aKey.isPressed) moveInput.x -= 1;
        if (kb.dKey.isPressed) moveInput.x += 1;
        moveInput.Normalize();

        if (kb.spaceKey.isPressed && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }

        if (kb.jKey.isPressed && shieldTimer >= shieldRate)
        {
            ActivateShield();
            shieldTimer = 0f;
        }

        if (kb.escapeKey.wasPressedThisFrame)
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || gunPoint == null) return;
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        if (rbBullet != null)
            rbBullet.velocity = gunPoint.up * bulletSpeed;
    }

    void ActivateShield()
    {
        if (shieldPrefab == null || shieldPoint == null) return;
        GameObject shield = Instantiate(shieldPrefab, shieldPoint.position, shieldPoint.rotation);
        shield.transform.SetParent(shieldPoint);
    }

    void StartGame()
    {
        gameStarted = true;
        isPaused = false;
        Time.timeScale = 1f;

        startButton.style.display = DisplayStyle.None;
        exitButton.style.display = DisplayStyle.None;
        loadButton.style.display = DisplayStyle.None;
        saveButton.style.display = DisplayStyle.None;
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        continueButton.style.display = DisplayStyle.Flex;
        restartButton.style.display = DisplayStyle.Flex;
        exitButton.style.display = DisplayStyle.Flex;
        saveButton.style.display = DisplayStyle.Flex;
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        continueButton.style.display = DisplayStyle.None;
        restartButton.style.display = DisplayStyle.None;
        exitButton.style.display = DisplayStyle.None;
        saveButton.style.display = DisplayStyle.None;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);

        restartButton.style.display = DisplayStyle.Flex;
        exitButton.style.display = DisplayStyle.Flex;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Component"))
        {
            if (currentCargo < maxCargo)
            {
                currentCargo++;
                UpdateCargoBar();
                other.gameObject.SetActive(false);

                if (currentCargo >= maxCargo)
                    SetComponentCollidersActive(false);
            }
        }
        else if (other.CompareTag("Portal"))
        {
            ExitGame();
        }
    }

    void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

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

    void UpdateCargoBar()
    {
        if (cargoBar != null)
        {
            cargoBar.value = currentCargo;
            cargoBar.title = $"Cargo: {currentCargo}/{maxCargo}";
        }
    }

    void SetComponentCollidersActive(bool state)
    {
        foreach (GameObject comp in GameObject.FindGameObjectsWithTag("Component"))
        {
            Collider2D col = comp.GetComponent<Collider2D>();
            if (col != null) col.enabled = state;
        }
    }
}