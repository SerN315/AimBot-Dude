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
    private List<PowerUpsEffect> selectedPowerUps = new List<PowerUpsEffect>();

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
        selectedPowerUps.Clear();
        while (selectedPowerUps.Count < 3)
        {
            PowerUpsEffect randomPowerUp = powerUps[Random.Range(0, powerUps.Count)];
            if (!selectedPowerUps.Contains(randomPowerUp))
            {
                selectedPowerUps.Add(randomPowerUp);
            }
        }

        for (int i = 0; i < powerUpButtons.Length; i++)
        {
            int index = i; // Local copy for lambda
            powerUpIcons[i].sprite = selectedPowerUps[i].icon;
            powerUpDescriptions[i].text = selectedPowerUps[i].description;
            powerUpButtons[i].onClick.RemoveAllListeners();
            powerUpButtons[i].onClick.AddListener(() => SelectPowerUp(selectedPowerUps[index]));
        }

        powerUpUIPanel.SetActive(true);
    }

    private void SelectPowerUp(PowerUpsEffect powerUp)
    {
        // Apply the power-up effect
        powerUp.ApplyEffect(GameObject.FindWithTag("Player"));

        // Hide the UI panel
        powerUpUIPanel.SetActive(false);
        Time.timeScale = 1f;
        movementUI.SetActive(true);
    }

    public void SaveSelectedPowerUps()
    {
        // Save selected power-ups
        PlayerPrefs.SetInt("SelectedPowerUpCount", selectedPowerUps.Count);
        for (int i = 0; i < selectedPowerUps.Count; i++)
        {
            PlayerPrefs.SetInt($"SelectedPowerUp_{i}", powerUps.IndexOf(selectedPowerUps[i]));
        }
    }

    public void LoadSelectedPowerUps()
    {
        // Load selected power-ups
        selectedPowerUps.Clear();
        int count = PlayerPrefs.GetInt("SelectedPowerUpCount", 0);
        for (int i = 0; i < count; i++)
        {
            int index = PlayerPrefs.GetInt($"SelectedPowerUp_{i}", -1);
            if (index != -1 && index < powerUps.Count)
            {
                selectedPowerUps.Add(powerUps[index]);
            }
        }

        // Apply loaded power-ups
        foreach (var powerUp in selectedPowerUps)
        {
            powerUp.ApplyEffect(GameObject.FindWithTag("Player"));
        }
    }

    public void ResetPowerUps()
    {
        // Reset player-specific attributes
        joystickMove.ResetPowerUps();
        playerStats.ResetHealth();
        bullets.ResetDamage();

        // Clear the selected power-ups
        selectedPowerUps.Clear();

        // Remove the stored power-ups from PlayerPrefs
        int count = PlayerPrefs.GetInt("SelectedPowerUpCount", 0);
        for (int i = 0; i < count; i++)
        {
            PlayerPrefs.DeleteKey($"SelectedPowerUp_{i}");
        }
        PlayerPrefs.DeleteKey("SelectedPowerUpCount");
    }
}
