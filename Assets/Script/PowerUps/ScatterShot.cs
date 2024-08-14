using UnityEngine;

[CreateAssetMenu(fileName = "ScatterShots", menuName = "ScriptableObjects/PowerUp/ScatterShots", order = 7)]
public class ScatterShots : PowerUpsEffect
{

    public override void ApplyEffect(GameObject player)
    {
        Attack playerAttack = player.GetComponent<Attack>();
        if (playerAttack != null)
        {
            playerAttack.enableMultipleGunpoints = true;
        }
    }
}

