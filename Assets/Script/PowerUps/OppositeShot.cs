using UnityEngine;

[CreateAssetMenu(fileName = "OppositeShot", menuName = "ScriptableObjects/PowerUp/OppositeShot", order = 8)]
public class OppositeShot : PowerUpsEffect
{

    public override void ApplyEffect(GameObject player)
    {
        Attack playerAttack = player.GetComponent<Attack>();
        if (playerAttack != null)
        {
            playerAttack.enableOppositeGunpoint = true;
        }
    }
}
