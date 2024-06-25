using UnityEngine;
using System.Collections;

public class MeleeEnemy : Enemy
{
    public int meleeDamage = 15;

    protected override void Start()
    {
        base.Start();
        speed = 7f; // Faster speed for melee
    }

    protected IEnumerator ChargeAttack()
    {
        yield return StartCoroutine(PlayPreChargeAnimationAndCharge());
        yield return new WaitForSeconds(0.5f); // Adjust duration according to your animation length
        anim.SetBool("run", true);
    }

    private IEnumerator PlayPreChargeAnimationAndCharge()
    {
        anim.SetBool("charge_start", true);
        yield return new WaitForSeconds(0.5f); // Adjust duration according to your animation length

        Charging();
    }

    private void Charging()
    {
        anim.SetBool("charge_start", false);
        StartCoroutine(ChargingAttack(2f)); // 2 seconds duration
    }

    private IEnumerator ChargingAttack(float duration)
    {
        yield return new WaitForSeconds(duration);
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("attack", false); // Resume running animation

    }

    public override void HandleDetection(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ChargeAttack());
            anim.SetBool("run", false);
            Debug.Log("Player entered detection range of MeleeEnemy");
        }
    }

    public override void HandleDetectionExit(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
   
            StartCoroutine(DisableAttackAnimationAfterDelay());
            anim.SetBool("attack", false);
            Debug.Log("Player left detection range of MeleeEnemy");
        }
    }

    private IEnumerator DisableAttackAnimationAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);

        anim.SetBool("attack", false);
    }
}
