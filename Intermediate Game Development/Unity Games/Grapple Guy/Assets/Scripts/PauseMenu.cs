using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    private int totalDeaths;
    public GameObject pauseMenuUI;
    public GameObject winUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    PlayerMovement playerMovement;
    MovingPlatform movingPlatform;
    Respawn respawn;
    public Text highScoreText;
    private int highScore;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        movingPlatform = GameObject.FindGameObjectWithTag("endPlat").GetComponent<MovingPlatform>();
        respawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Respawn>();
        highScore = PlayerPrefs.GetInt("highScore", 20);
        highScoreText.text = "BEST SCORE: " + highScore + " DEATH(S)";      
    }

    // Update is called once per frame
    void Update()
    {       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        // set the best score and save it with PlayerPrefs - Edit/Clear all in inspector to reset
        if (movingPlatform.youWon == true)
        {          
            WinScreen();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        Time.timeScale = 1f;
        GameIsPaused = false;
        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void WinScreen()
    {
        // set the best score and save it with PlayerPrefs - Edit/Clear all in inspector to reset
        totalDeaths = respawn.deathCounter;
        if (totalDeaths < highScore)
        {
            PlayerPrefs.SetInt("highScore", totalDeaths);
            Debug.Log("Update Player prefs");
        }

        Time.timeScale = 0f;
        GameIsPaused = true;
        playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;      
        winUI.SetActive(true);      
        scoreText.text = ("TOTAL DEATHS: " + totalDeaths.ToString());       
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
