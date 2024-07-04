using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegenPowerUp", menuName = "ScriptableObjects/PowerUp/HealthRegen", order = 4)]
public class HealthRegenPowerUp : PowerUpsEffect
{
    public int healthAmount;

    public override void ApplyEffect(GameObject player)
    {
        PlayerStats playerHealth = player.GetComponent<PlayerStats>();
        if (playerHealth != null)
        {
            playerHealth.RecoverHealth(healthAmount);
        }
    }
}

