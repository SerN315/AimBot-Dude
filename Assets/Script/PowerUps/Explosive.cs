using UnityEngine;

[CreateAssetMenu(fileName = "Explosive Bullets", menuName = "ScriptableObjects/PowerUp/Explosive", order = 5)]
public class Explosive : PowerUpsEffect
{
    public int DamageAmount;

    public override void ApplyEffect(GameObject player)
    {
        Attack playerAttack = player.GetComponent<Attack>();
        if (playerAttack != null)
        {
            playerAttack.applyExplosiveBullet = true;
        }
    }
}