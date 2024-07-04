using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoostPowerUp", menuName = "ScriptableObjects/PowerUp/SpeedBoost", order = 2)]
public class SpeedBoostPowerUp : PowerUpsEffect
{
    public float speedMultiplier;

    public override void ApplyEffect(GameObject player)
    {
        JoystickMove playerController = player.GetComponent<JoystickMove>();
        if (playerController != null)
        {
            playerController.ApplySpeedBoost(speedMultiplier);
        }
    }
}
