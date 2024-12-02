using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerNameText;

    public void GameOver(string winnerName = "No One")
    {
        gameObject.SetActive(true);
        winnerNameText.text = winnerName + " Win";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
