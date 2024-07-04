using UnityEngine;

[CreateAssetMenu(fileName = "DamagePowerUp", menuName = "ScriptableObjects/PowerUp/DamageUp", order = 3)]
public class ShieldPowerUp : PowerUpsEffect
{
    public int DamageAmount;
    public override void ApplyEffect(GameObject player)
    {
        Bullets playerController = player.GetComponent<Bullets>();
        if (playerController != null)
        {
            playerController.DamageUp(DamageAmount);
        }
    }
}
