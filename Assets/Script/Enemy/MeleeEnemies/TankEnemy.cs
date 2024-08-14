using UnityEngine;
using System.Collections;

public class TankEnemy : Enemy
{
    public int tankDamage = 20;
    private bool hasAttack = false;
    public float delayBeforeNextAttack = 2f; // Delay before the tank can attack again

    protected override void Start()
    {
        base.Start();
        health = 200; // Higher health for tank
        speed = 3f; // Slower speed for tank
    }

    public override void HandleDetection(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasAttack)
            {
                anim.SetBool("run", false);
                anim.SetBool("attack", true);
                hasAttack = true;
                //Debug.Log("Player entered detection range of TankEnemy");
            }
        }
    }

    public override void HandleDetectionExit(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasAttack)
            {
                StartCoroutine(DisableAttackAnimationAfterDelay(delayBeforeNextAttack));
                hasAttack = false;
                //Debug.Log("Player left detection range of TankEnemy");
            }
        }
    }

    private IEnumerator DisableAttackAnimationAfterDelay(float delay)
    {
        anim.SetBool("attack", false);
        anim.SetBool("run", false); // Set to idle
        yield return new WaitForSeconds(delay);
        anim.SetBool("run", true); // Resume running animation
        hasAttack = false; // Allow the tank to attack again after the delay
        //Debug.Log("TankEnemy can attack again");
    }

}
