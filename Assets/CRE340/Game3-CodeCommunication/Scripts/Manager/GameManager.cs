using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton Implementation

    // Singleton instance
    private static GameManager instance;

    public static GameManager Instance{
        get{
            if (instance == null){
                instance = FindObjectOfType<GameManager>();
                if (instance == null){
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                }
            }

            return instance;
        }
    }

    #endregion
    
    #region Properties and Fields
    //reference to SaveLoadManager
    private SaveLoadManager saveLoadManager;
    
    // Player reference
    public GameObject playerPrefab;
    private Player playerInstance;

    // Inspector-visible default player state
    [SerializeField] private string playerName = "Player1"; // Default player name
    [SerializeField] private int playerHealth = 100;        // Default health
    [SerializeField] private int score = 0;                 // Default score
    [SerializeField] private int experience = 0;            // Default experience
    [SerializeField] private int coins = 0;                 // Default coins
    
    // Public properties to access these fields but prevent external modification
    public string PlayerName
    {
        get { return playerName; }
        private set
        {
            playerName = value;
            //notify the UI when the player name changes
            UI_EventHandler.PlayerNameChanged(playerName);
        }
    }

    public int PlayerHealth
    {
        get { return playerHealth; }
        private set
        {
            playerHealth = value;
            UI_EventHandler.PlayerHealthChanged(playerHealth);
        }
    }

    public int Score
    {
        get { return score; }
        private set
        {
            score = value;
            UI_EventHandler.ScoreChanged(score);
        }
    }

    public int Experience
    {
        get { return experience; }
        private set
        {
            experience = value;
            UI_EventHandler.ExperienceChanged(experience);
            saveLoadManager.GainExperience(experience);
        }
    }

    public int Coins
    {
        get { return coins; }
        private set
        {
            coins = value;
            UI_EventHandler.CoinsChanged(coins);
            saveLoadManager.AddCoins(coins);
        }
    }

    #endregion

    #region Unity Methods
    private void Start()
    {
        // Initialize with default values
        Debug.Log("GameManager initialized with default player state.");
        
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        if (saveLoadManager == null)
        {
            Debug.LogError("SaveLoadManager not found in the scene.");
        }
        else
        {
            if (saveLoadManager.autoLoad)
            {
                LoadData();
            }
        }
    }
    #endregion

    #region Custom Public Methods
    
    // Method to instantiate the player and keep track of its instance
    public void SpawnPlayer(Vector3 spawnPosition)
    {
        if (playerInstance == null) // Ensure we don't spawn multiple players
        {
            GameObject playerObject = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            playerInstance = playerObject.GetComponent<Player>();
            SetPlayerName(playerInstance.name);
            SetPlayerHealth(playerInstance.health);
        }
    }
    
    // Method to set the player name
    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    // Method to update player health
    public void SetPlayerHealth(int health)
    {
        PlayerHealth = Mathf.Clamp(health, 0, 100); // Ensure health stays between 0 and 100
        if (PlayerHealth <= 0)
        {
            // Handle player death, such as restarting level or showing game over
            Invoke("RestartLevel", 5F);
        }
    }

    // Method to increase the score
    public void AddScore(int points)
    {
        Score += points;
    }

    // Method to restart the current level
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void AddExperience(int points)  
    {  
        Experience += points;  
        UI_EventHandler.ExperienceChanged(experience);  
        saveLoadManager.playerProperties.experience = experience;  
        if (saveLoadManager.autoSave){  
            saveLoadManager.SaveData();  
        }
    }  
	  
    public void AddCoins(int amount)  
    {  
        coins += amount;  
        UI_EventHandler.CoinsChanged(coins);  
        saveLoadManager.playerProperties.coins = coins;  
        if(saveLoadManager.autoSave){  
            saveLoadManager.SaveData();  
        }
    }
    
    //// *** SAVE AND LOAD FUNCTIONS *** ////
    
    public void SaveData()
    {
        saveLoadManager.SaveData();
        GetSaveData();
        UpdateUI();
    }

    public void LoadData()
    {
        saveLoadManager.LoadData();
        GetSaveData();
        UpdateUI();
    }

    public void ClearData()
    {
        ResetData();
        saveLoadManager.ClearData();
        UpdateUI();
    }
    
    
    /// HELPER METHODS ///
    private void ResetData()
    {
        playerName = "Player1";
        playerHealth = 100;
        score = 0;
        experience = 0;
        coins = 0;
    }

    private void GetSaveData()
    {
        playerName = saveLoadManager.playerProperties.name;
        playerHealth = 100; // Not saved, kept default
        score = 0;          // Not saved, kept default
        experience = saveLoadManager.playerProperties.experience;
        coins = saveLoadManager.playerProperties.coins;
    }

    private void UpdateUI()
    {
        UI_EventHandler.PlayerNameChanged(playerName);
        UI_EventHandler.PlayerHealthChanged(playerHealth);
        UI_EventHandler.ScoreChanged(score);
        UI_EventHandler.ExperienceChanged(experience);
        UI_EventHandler.CoinsChanged(coins);
    }
    
    #endregion
}