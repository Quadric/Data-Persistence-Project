using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text BestScoreText;
    public Text ScoreText;
    public GameObject GameOverText;
    public GameObject HighScorePanel;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        UpdateBestScoreInfo();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        UpdateBestScore();
        m_GameOver = true;
        // GameOverText.SetActive(true);
        ShowHighScorePanel();
       
    }

    private void ShowHighScorePanel()
    {
        List<PlayerScore> highScore = GameManager.Instance.GetHighScore();
        HighScorePanel.SetActive(true);

        for (int i=0; i < Mathf.Min(5, highScore.Count); i++)
        {
            GameObject nameField = GameObject.Find("Name " + (i + 1).ToString());
            nameField.GetComponent<TextMeshProUGUI>().text = highScore[i].playerName;

            GameObject scoreField = GameObject.Find("Score " + (i + 1).ToString());
            scoreField.GetComponent<TextMeshProUGUI>().text = highScore[i].score.ToString();
        }

    }

    public void UpdateBestScoreInfo()
    {
        string currentPlayerName = GameManager.Instance.GetLastPlayerName();
        string bestPlayerName = GameManager.Instance.GetBestPlayerName();
        int bestPlayerScore = GameManager.Instance.GetBestPlayerScore();

        BestScoreText.text = "Current player: " + currentPlayerName + ". Best Score : " + bestPlayerName + ": " + bestPlayerScore.ToString();
    }

    public void UpdateBestScore()
    {
        string playerName = GameManager.Instance.lastPlayerName;
        GameManager.Instance.SetPlayerScore(playerName, m_Points);
        UpdateBestScoreInfo();
        GameManager.Instance.SaveGame();
    }
}
