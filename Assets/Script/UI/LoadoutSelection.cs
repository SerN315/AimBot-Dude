using UnityEngine;
using UnityEngine.UI;

public class GunSelectionMenu : MonoBehaviour
{
    public Gun[] availableGuns; // Array of available guns
    public Button[] gunButtons; // Array of buttons for selecting guns
    public Text gunNameText; // UI Text for displaying gun name
    public Text fireRateText; // UI Text for displaying fire rate
    public Text damageText; // UI Text for displaying damage
    public Image gunImage; // Image component to display the gun sprite
    public Button equipButton; // Button for equipping the selected gun

    private int selectedGunIndex = -1; // Index of the currently selected gun

    void Start()
    {
        // Assign button click listeners
        for (int i = 0; i < gunButtons.Length; i++)
        {
            int index = i;
            Image buttonImage = gunButtons[i].GetComponentInChildren<Image>();
            if (buttonImage != null && availableGuns[i].sprite != null)
            {
                buttonImage.sprite = availableGuns[i].sprite;
            }
            gunButtons[i].onClick.AddListener(() => DisplayGunDetails(index));
        }

        // Assign equip button click listener
        equipButton.onClick.AddListener(EquipSelectedGun);

        // Load selected gun index from game state manager
        selectedGunIndex = GameStateManager.Instance.SelectedGunIndex;

        DisplayGunDetails(selectedGunIndex); // Ensure selected gun details are displayed
    }

    void DisplayGunDetails(int index)
    {
        if (index >= 0 && index < availableGuns.Length)
        {
            Gun gun = availableGuns[index];

            // Debug logs to check values
            Debug.Log($"Displaying details for gun at index {index}");
            Debug.Log($"Name: {gun.name}");
            Debug.Log($"Fire Rate: {gun.fireRate}");
            Debug.Log($"Damage: {gun.bulletDamage}");

            gunNameText.text = "Name: " + gun.name;
            fireRateText.text = "Fire Rate: " + gun.fireRate.ToString();
            damageText.text = "Damage: " + gun.bulletDamage;

            // Set gun image
            if (gunImage != null && gun.sprite != null)
            {
                gunImage.sprite = gun.sprite;
            }

            // Update selected gun index
            selectedGunIndex = index;
        }
        else
        {
            Debug.LogWarning("Invalid gun selection index.");
        }
    }

    void EquipSelectedGun()
    {
        if (selectedGunIndex >= 0 && selectedGunIndex < availableGuns.Length)
        {
            // Save the selected gun index
            GameStateManager.Instance.SetSelectedGunIndex(selectedGunIndex);

            // Example: Equip the selected gun immediately
            EquipGun(selectedGunIndex);
        }
    }

    void EquipGun(int index)
    {
        // Example logic to equip the selected gun in your game
        // Replace with your actual gun equipping method
        Debug.Log("Equipping gun: " + availableGuns[index].name);
    }
}
