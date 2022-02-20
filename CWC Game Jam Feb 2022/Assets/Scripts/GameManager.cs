using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]

public class GameManager : MonoBehaviour
{
    public float playerSpeed;
    public static GameManager Manager { get; private set; }
    [SerializeField] TextMeshProUGUI speedometerText;
    public TextMeshProUGUI gameOverText;
    public float timeRemaining = 180.0f;
    public TextMeshProUGUI timerText;
    private Rigidbody playerRb;
    public bool isGameActive;
    public Button restartButton;
    public TextMeshProUGUI titleText;
    public Button startButton;

    private void Awake()
    {
        isGameActive = false;

        if (Manager != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Manager = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        
        AddToTimer(0);
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = Mathf.Round(playerRb.velocity.magnitude);
        //* 2.237f
        speedometerText.SetText("Speed: " + playerSpeed + "mph");

        if (isGameActive == true)
        {
            Timer();
        }
        if (timeRemaining < 0)
        {
            GameOver();
        }
    }

    public void Timer()
    {
        timeRemaining -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Round(timeRemaining);
    }

    public void AddToTimer(int timeToAdd)
    {
        timeRemaining += timeToAdd;
    }

    public void StartGame()
    {
        isGameActive = true;
        SphereController.Controller.score = 0;
        titleText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

        public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
