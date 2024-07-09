using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpManager : MonoBehaviour
{
    public JoystickMove joystickMove;
    public PlayerStats playerStats;
    public Bullets bullets;
   
    public List<PowerUpsEffect> powerUps;
    public GameObject powerUpUIPanel;
    public Button[] powerUpButtons;
    public GameObject movementUI; // Reference to the move UI panel
    public Image[] powerUpIcons;
    public TMP_Text[] powerUpDescriptions;
    private GameManager gameManager;
    private Dictionary<PowerUpsEffect, int> selectedPowerUps = new Dictionary<PowerUpsEffect, int>();

    private void Start()
    {
        powerUpUIPanel.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();

        // Load selected power-ups
        LoadSelectedPowerUps();
    }

    public void ShowPowerUpUI()
    {
        Time.timeScale = 0f;
        movementUI.SetActive(false);
        List<PowerUpsEffect> availablePowerUps = new List<PowerUpsEffect>(powerUps);
        while (availablePowerUps.Count > 3)
        {
            availablePowerUps.RemoveAt(Random.Range(0, availablePowerUps.Count));
        }

        for (int i = 0; i < powerUpButtons.Length; i++)
        {
            int index = i; // Local copy for lambda
            powerUpIcons[i].sprite = availablePowerUps[i].icon;
            powerUpDescriptions[i].text = availablePowerUps[i].description;
            powerUpButtons[i].onClick.RemoveAllListeners();
            powerUpButtons[i].onClick.AddListener(() => SelectPowerUp(availablePowerUps[index]));
        }

        powerUpUIPanel.SetActive(true);
    }

    private void SelectPowerUp(PowerUpsEffect powerUp)
    {
        if (selectedPowerUps.ContainsKey(powerUp))
        {
            selectedPowerUps[powerUp]++;
        }
        else
        {
            selectedPowerUps[powerUp] = 1;
        }

        // Apply the power-up effect as many times as it is stacked
        for (int i = 0; i < selectedPowerUps[powerUp]; i++)
        {
            powerUp.ApplyEffect(GameObject.FindWithTag("Player"));
        }

        // Hide the UI panel
        powerUpUIPanel.SetActive(false);
        Time.timeScale = 1f;
        movementUI.SetActive(true);
    }

    public void SaveSelectedPowerUps()
    {
        // Clear previous saved data
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("SelectedPowerUpCount", selectedPowerUps.Count);
        int i = 0;
        foreach (var kvp in selectedPowerUps)
        {
            PlayerPrefs.SetInt($"SelectedPowerUp_{i}_Index", powerUps.IndexOf(kvp.Key));
            PlayerPrefs.SetInt($"SelectedPowerUp_{i}_Count", kvp.Value);
            i++;
        }
    }

    public void LoadSelectedPowerUps()
    {
        selectedPowerUps.Clear();
        int count = PlayerPrefs.GetInt("SelectedPowerUpCount", 0);

        for (int i = 0; i < count; i++)
        {
            int index = PlayerPrefs.GetInt($"SelectedPowerUp_{i}_Index", -1);
            int powerUpCount = PlayerPrefs.GetInt($"SelectedPowerUp_{i}_Count", 0);

            if (index != -1 && index < powerUps.Count)
            {
                PowerUpsEffect powerUp = powerUps[index];
                selectedPowerUps[powerUp] = powerUpCount;
                for (int j = 0; j < powerUpCount; j++)
                {
                    powerUp.ApplyEffect(GameObject.FindWithTag("Player"));
                }
            }
        }
    }

    public void ResetPowerUps()
    {
        // Reset player-specific attributes
        
        if (joystickMove != null)
        {
            joystickMove.ResetPowerUps();
        }
        
        if (playerStats != null)
        {
            playerStats.ResetHealth();
        }
        
        if (bullets != null)
        {
            bullets.ResetDamage();
        }

        // Clear the selected power-ups
        selectedPowerUps.Clear();

        // Remove the stored power-ups from PlayerPrefs
        int count = PlayerPrefs.GetInt("SelectedPowerUpCount", 0);
        for (int i = 0; i < count; i++)
        {
            PlayerPrefs.DeleteKey($"SelectedPowerUp_{i}_Index");
            PlayerPrefs.DeleteKey($"SelectedPowerUp_{i}_Count");
        }
        PlayerPrefs.DeleteKey("SelectedPowerUpCount");
    }
}
