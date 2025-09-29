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
    public float maxSpeed = 5f;
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

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

    void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
}
