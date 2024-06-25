using UnityEngine;
using System.Collections;

public class TankEnemy : Enemy
{
     public int tankDamage = 20;
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
            anim.SetBool("run", false);
            anim.SetBool("attack", true);
            Debug.Log("Player entered detection range of MeleeEnemy");
        }
    }

    public override void HandleDetectionExit(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DisableAttackAnimationAfterDelay());
            Debug.Log("Player left detection range of MeleeEnemy");
        }
    }

    private IEnumerator DisableAttackAnimationAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);

        anim.SetBool("attack", false);
        anim.SetBool("run", true);
    }

}
