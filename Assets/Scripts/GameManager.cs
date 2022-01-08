using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public int score;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string lastPlayerName;
    public List<PlayerScore> highScore; 

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadGame();
    }

    [System.Serializable]
    class SaveData
    {
        public string lastPlayerName;
        public List<PlayerScore> highScore;
    }

    private void LoadGame()
    {
        string path = Application.persistentDataPath + "/arka4.cfg";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            lastPlayerName = data.lastPlayerName;
            highScore = data.highScore;
        } 
    }

    public void SaveGame()
    {
        string path = Application.persistentDataPath + "/arka4.cfg";
        SaveData data = new SaveData();
        data.highScore = highScore;
        data.lastPlayerName = lastPlayerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(path, json);
    }

    public string GetBestPlayerName()
    {
        if (highScore.Count > 0)
        {
            return highScore[0].playerName;
        }
        else
        {
            return "Unknown";
        }
    }

    public string GetLastPlayerName()
    {
        if (lastPlayerName != "")
        {
            return lastPlayerName;
        }
        else
        {
            return "Unknown";
        }
    }

    public int GetBestPlayerScore()
    {
        if (highScore.Count > 0)
        {
            return highScore[0].score;
        }
        else
        {
            return 0;
        }
    }

    public void SetPlayerScore(string playerName, int score)
    {
        bool foundInHighScore = false;

        for (int i=0; i < highScore.Count; i++)
        {
            if (highScore[i].playerName == playerName)
            {
                if (highScore[i].score < score)
                {
                    highScore[i].score = score;
                }
                foundInHighScore = true;
                break;
            }
        }

        if (!foundInHighScore)
        {
            PlayerScore ps = new PlayerScore();
            ps.playerName = playerName;
            ps.score = score;

            highScore.Add(ps);
        }

        highScore.Sort((u1, u2) => u2.score.CompareTo(u1.score));
    }

    public List<PlayerScore> GetHighScore()
    {
        return highScore;
    }
}
