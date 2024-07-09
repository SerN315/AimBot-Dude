using UnityEngine;

[CreateAssetMenu(fileName = "DamagePowerUp", menuName = "ScriptableObjects/PowerUp/DamageUp", order = 3)]
public class DamagePowerUp : PowerUpsEffect
{
    public int DamageAmount;

    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("Applying damage increase: " + DamageAmount);
        Attack playerAttack = player.GetComponent<Attack>();
        if (playerAttack != null)
        {
            playerAttack.ApplyDamagePowerUp(DamageAmount);
        }
    }
}
