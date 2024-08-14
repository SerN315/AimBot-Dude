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

    public Gun[] availableGuns; // Array of available guns
    public Transform gunHand; // Reference to the gun hand transform
    private Gun currentGun; // Reference to the currently equipped gun
    private Attack attackScript; // Reference to the Attack script on the player

void Start()
{
    // Load selected gun index from PlayerPrefs or save data
    int selectedGunIndex = GameStateManager.Instance.SelectedGunIndex;

    // Ensure game over UI starts inactive
    totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
    gameOverUI.SetActive(false);
    powerUpManager = FindObjectOfType<PowerUpManager>();
    retryButton.gameObject.SetActive(false);
    continueButton.gameObject.SetActive(false);
    currencyManager = CurrencyManager.Instance;

    // Get the Attack script attached to the player GameObject
    attackScript = FindObjectOfType<Attack>(); // Find the Attack script in the scene

    if (attackScript == null)
    {
        //Debug.LogWarning("Attack script not found on Player GameObject.");
        return;
    }

    // Equip the selected gun based on index
    EquipGun(selectedGunIndex);
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
        //Debug.Log("Number of enemies remaining: " + totalEnemies);

        // Check win condition
        if (totalEnemies <= 0)
        {
            CheckAndDestroyBarriers();
            //powerUpManager.ShowPowerUpUI();
            currencyManager.SaveCurrencyOnWin();

            // Reset collected currency for the new level
            CurrencyManager.Instance.ResetCurrency();
        }
    }

    private void CheckAndDestroyBarriers()
    {
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Finish");
        //Debug.Log("Number of barriers found: " + barriers.Length); // Log number of barriers found

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
        SceneManager.LoadScene(3); // Assuming Home is at build index 1
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            GameData.instance.SaveData(player.currentlevel, player.currentexp, player.maxExp);
            movementUI.SetActive(true);
        }
        else
        {
            
            //Debug.LogWarning("No more scenes available after the current one.");
            PlayerWon();
        }
    }

    
public void EquipGun(int index)
{
    // Find existing gun GameObject named "Gun" under gunHand
    Transform existingGun = gunHand.Find("Gun");

        // Remove existing gun if one is already equipped
        if (currentGun != null)
        {
            Destroy(currentGun.gameObject);
        }

        // Instantiate new gun prefab from availableGuns array
        currentGun = Instantiate(availableGuns[index], gunHand.position, gunHand.rotation, gunHand);
        //Debug.Log("Equipping gun: " + availableGuns[index].name);

    // Set position relative to gunHand with specified offset
    Vector3 offset = new Vector3(0.135f, 0.033f, 0f); // Adjust as needed
    currentGun.transform.localPosition = offset;
    currentGun.transform.localRotation = Quaternion.identity;

    // Optionally, you can rename the instantiated gun to match "Gun"
    currentGun.gameObject.name = "Gun";

    // Automatically assign firePoint to the child named "gunPoint" if available
    Transform gunPoint = currentGun.transform.Find("gunpoint");
    if (gunPoint != null)
    {
        // Assign gunPoint as firePoint in Attack script
        attackScript.firePoint = gunPoint;
        attackScript.SetGunProperties(currentGun.bulletPrefab, currentGun.fireRate, currentGun.additionalEffect, currentGun.bulletDamage); // Set gun properties
        //Debug.Log("Assigned firePoint: " + gunPoint.position);
    }
    else
    {
        //Debug.LogWarning("gunPoint not found on the instantiated gun prefab.");
    }
}
}
 

