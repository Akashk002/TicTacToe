using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayLocal()
    {
        PlayerPrefs.SetInt("GameMode", (int)GameMode.Local);
        SceneManager.LoadScene("Game");
    }
    public void PlayWithAI()
    {
        PlayerPrefs.SetInt("GameMode", (int)GameMode.AI);
        SceneManager.LoadScene("Game");
    }
    public void Exit()
    {
        Application.Quit();
    }

}
