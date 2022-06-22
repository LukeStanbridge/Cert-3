using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public HighScores m_BestScores;
    public Text m_MessageText; // Main message text
    public Text m_TimerText; // Timer text 

    public GameObject m_HighScorePanel;
    public GameObject m_PlayerTank;
    public Text m_HighScoresText;
    
    public Button m_NewGameButton;
    public Button m_HighScoresButton;

    public Transform spawnPoint; // transform variable for the enemy tank respawn point
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform bossSpawn;
    public Transform playerRespawn; // transform variable for the player tank to respawn
    public GameObject[] m_Tanks; // tanks array to store each tank object

    private float m_gameTime = 0; // count the time the player has survived(score)

    public float GameTime { get { return m_gameTime; } } // public accessor function(property), so we can access this priavte variable from another script
    public GameObject[] m_BestTime;

    public enum GameState
    {
        Start,
        Playing,
        GameOver
    }

    private GameState m_GameState;
    public GameState State { get { return m_GameState; } }

    private void Awake()
    {
        m_GameState = GameState.Start;
        // sets spawnpoint to the enemy spawn game object with the "Respawn1,2 & 3" tag
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        spawnPoint2 = GameObject.FindGameObjectWithTag("Respawn2").transform;
        spawnPoint3 = GameObject.FindGameObjectWithTag("Respawn3").transform;
        // sets playerRespawn to the player spawn game object with the "PlayerRespawn" tag
        playerRespawn = GameObject.FindGameObjectWithTag("PlayerRespawn").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(false);
        }

        m_TimerText.gameObject.SetActive(false);
        m_MessageText.text = "Press Enter to start";

        // Sets high score panel and buttons to false so they don't display when the game starts
        m_HighScorePanel.gameObject.SetActive(false);
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_GameState)
        {
            case GameState.Start:

                if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    m_TimerText.gameObject.SetActive(true);
                    m_MessageText.text = "";
                    m_GameState = GameState.Playing;

                    for (int i = 0; i < m_Tanks.Length; i++)
                    {
                        m_Tanks[i].SetActive(true);                   
                    }
                    m_Tanks[4].SetActive(false);
                }
                break;
            case GameState.Playing:
                bool isGameOver = false;

                m_gameTime += Time.deltaTime;
                int seconds = Mathf.RoundToInt(m_gameTime);
                m_TimerText.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60));

                // These if statements have to be ordered this way or game breaks/causes issues
                if (IsPlayerDead() == true)
                {
                    isGameOver = true;
                }
                else if (m_Tanks[4].GetComponent<EnemyTankHealth>().m_Dead == true)
                {                
                    isGameOver = true;
                }
                else if (BossFight() == true)
                {
                    m_Tanks[4].SetActive(true);
                }                          

                if (isGameOver == true)
                {
                    m_GameState = GameState.GameOver;

                    // Sets timer text to false and activates button options for new game and high scores
                    m_TimerText.gameObject.SetActive(false);             
                    m_NewGameButton.gameObject.SetActive(true);
                    m_HighScoresButton.gameObject.SetActive(true);

                    if (IsPlayerDead() == true)
                    {
                        m_MessageText.text = "TRY AGAIN";                      
                    }
                    else
                    {
                        m_MessageText.text = "WINNER";
                        m_Tanks[0].gameObject.SetActive(false);
                        // Save the score
                        m_BestScores.AddScore(Mathf.RoundToInt(m_gameTime));
                        m_BestScores.SaveScoresToFile();
                    }
                }
                break;
        }
        //Exit game at any time
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private bool BossFight()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == true)
            {
                numTanksLeft++;
            }
        }
        return numTanksLeft <= 1;
    }

    private bool IsPlayerDead()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == false)
            {
                if (m_Tanks[i].tag == "Player")
                    return true;               
            }
        }
        return false;
    }

    public void OnNewGame()
    {
        m_HighScorePanel.gameObject.SetActive(false);
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);

        m_gameTime = 0;
        m_GameState = GameState.Playing;
        m_TimerText.gameObject.SetActive(true);
        m_MessageText.text = "";
        m_PlayerTank.GetComponent<TankHealth>().SetHealth(1); // passes value of 1(100%) to set health function on new game click
        
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(true);
            m_Tanks[1].transform.position = spawnPoint.position; // respawns enemy tanks at spawn point position
            m_Tanks[2].transform.position = spawnPoint2.position;
            m_Tanks[3].transform.position = spawnPoint3.position;
            m_Tanks[4].transform.position = bossSpawn.position;
            m_Tanks[0].transform.position = playerRespawn.position; // respawns player tank at player spawn position
            m_Tanks[0].transform.rotation = playerRespawn.rotation; // respawns player facing forward instead of whatever its rotation was on NewGame click
            m_Tanks[4].SetActive(false);
        }
    }

    public void OnHighScores()
    {
        m_MessageText.text = "";

        m_HighScoresButton.gameObject.SetActive(false);
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScorePanel.SetActive(true);
        
        string text = "";
        for (int i = 0; i < m_BestScores.scores.Length; i++)
        {
            int seconds = m_BestScores.scores[i];
            text += string.Format("{0:D2}:{1:D2}\n", (seconds / 60), (seconds % 60));
        }
        m_HighScoresText.text = text;
    }
}