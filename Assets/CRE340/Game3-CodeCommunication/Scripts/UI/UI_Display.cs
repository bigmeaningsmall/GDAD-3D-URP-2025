using UnityEngine;
using TMPro;
using DG.Tweening;

public class UI_Display : MonoBehaviour
{
    // References to the UI elements in the scene
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI scoreText;

    // Method to update the player's name in the UI
    public void UpdatePlayerName(string playerName)
    {
        if(playerNameText != null)
        {
            playerNameText.text = "Player: " + playerName;
        }
    }

    // Method to update the player's health in the UI
    public void UpdatePlayerHealth(int playerHealth)
    {
        if(playerHealthText != null)
        {
            playerHealthText.text = "Health: " + playerHealth.ToString();
            //TODO - add a health animation effect  
            playerHealthText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 10, 1f);
        }
    }

    // Method to update the score in the UI
    public void UpdateScore(int score)
    {
        if(scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
            //TODO - add a score animation effect  
            scoreText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 10, 1f);
        }
    }
}