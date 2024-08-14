using UnityEngine;

[CreateAssetMenu(fileName = "Piercing Bullets", menuName = "ScriptableObjects/PowerUp/Piercing", order = 6)]
public class Piercing : PowerUpsEffect
{

    public override void ApplyEffect(GameObject player)
    {
        Attack playerAttack = player.GetComponent<Attack>();
        if (playerAttack != null)
        {
            playerAttack.applyPiercingBullet = true;
        }
    }
}
