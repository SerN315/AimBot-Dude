using UnityEngine;
[CreateAssetMenu(fileName = "HealthRegenPowerUp", menuName = "ScriptableObjects/PowerUp/MaxHealthIncrease", order = 5)]
public class MaxHealthPowerUp : PowerUpsEffect
{
    public int maxHealthIncrease; // New field for max health increase

    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("Applying max health increase: " + maxHealthIncrease);
        PlayerStats playerHealth = player.GetComponent<PlayerStats>();
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(maxHealthIncrease); // Apply max health increase
        }
    }
}
