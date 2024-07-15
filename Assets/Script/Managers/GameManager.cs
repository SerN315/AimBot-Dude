using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject movementUI; // Reference to the move UI panel
    public GameObject gameOverUI; // Reference to the merged UI panel
    private bool isGameOver = false; // Flag to track game over state
    private int totalEnemies; // Total number of enemies in the scene
    public Button retryButton;
    public Button continueButton;
    public TMP_Text gameOverText; // Reference to the text component for game over message
    public TMP_Text gameOverDetailsText; // Reference to the text component for additional details
    public PowerUpManager powerUpManager;
    private CurrencyManager currencyManager;

    void Start()
    {
        // Ensure game over UI starts inactive
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        gameOverUI.SetActive(false);
        powerUpManager = FindObjectOfType<PowerUpManager>();
        retryButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        currencyManager = CurrencyManager.Instance;
    }

    // Call this method when the player dies
    public void PlayerDied()
    {
        // Set game over flag
        isGameOver = true;
        retryButton.gameObject.SetActive(true);
        // Display game over UI
        gameOverUI.SetActive(true);
        gameOverText.text = "Floor cleared";
        gameOverDetailsText.text = "0";
        // Pause game actions or time scale
        Time.timeScale = 0f; // Pause time scale
        movementUI.SetActive(false);

        // Reset collected currency
        CurrencyManager.Instance.ResetCurrency();
    }

    public void EnemyDestroyed()
    {
        totalEnemies--;
        Debug.Log("Number of enemies remaining: " + totalEnemies);

        // Check win condition
        if (totalEnemies <= 0)
        {
            CheckAndDestroyBarriers();
            powerUpManager.ShowPowerUpUI();
            currencyManager.SaveCurrencyOnWin();

            // Reset collected currency for the new level
            CurrencyManager.Instance.ResetCurrency();
        }
    }

    private void CheckAndDestroyBarriers()
    {
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Finish");
        Debug.Log("Number of barriers found: " + barriers.Length); // Log number of barriers found

        foreach (GameObject barrier in barriers)
        {
            Destroy(barrier);
        }
    }

    // Call this method when the player wins
    public void PlayerWon()
    {
        // Set game over flag
        isGameOver = true;
        gameOverText.text = "Completed";
        gameOverDetailsText.text = "";
        // Display game over UI
        gameOverUI.SetActive(true);
        continueButton.gameObject.SetActive(true);
        // Pause game actions or time scale
        Time.timeScale = 0f; // Pause time scale
        movementUI.SetActive(false);
        powerUpManager.ResetPowerUps();

        // Save currency on win
        currencyManager.SaveCurrencyOnWin();
    }

    // Retry the level
    public void RetryLevel()
    {
        // Reset time scale
        Time.timeScale = 1f;

        // Reset power-ups
        powerUpManager.ResetPowerUps();
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        movementUI.SetActive(true);
        currencyManager.ResetCurrency();
    }

    // Proceed to the next level (or restart the current level for simplicity)
    public void NextLevel()
    {
        // Reset time scale
        Time.timeScale = 1f;

        // Save power-ups before reloading the scene
        powerUpManager.SaveSelectedPowerUps();
        // currencyManager.ResetCurrency();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            movementUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No more scenes available after the current one.");
            PlayerWon();
        }
    }
}
