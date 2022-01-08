using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class MenuHandler : MonoBehaviour
{
    public void Start()
    {
        string lastPlayerName = GameManager.Instance.GetLastPlayerName();

        GameObject.Find("Player Name").GetComponent<TMP_InputField>().text = lastPlayerName;
    }

    public void StartGame()
    {
        TMP_InputField playerName = GameObject.Find("Player Name").GetComponent<TMP_InputField>();

        GameManager.Instance.lastPlayerName = playerName.text != "" ? playerName.text : "Guest";
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        //GameManager.Instance.SaveColor();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
