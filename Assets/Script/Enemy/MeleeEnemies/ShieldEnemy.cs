// using System.Collections;
// using UnityEngine;

// public class ShieldEnemys : MonoBehaviour
// {
//     private Rigidbody2D rb;
//     private Animator anim;
//     private CapsuleCollider2D shieldhitboxCollider;
//     public bool isShieldActive = false;
//     private bool hasPlayedPreShieldAnimation = false;
//     private bool isCooldownActive = false;
//     private float cooldownDuration = 2f; // Cooldown duration in seconds

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         anim = GetComponent<Animator>();
//         shieldhitboxCollider = GetComponentInChildren<CapsuleCollider2D>();

//         // Ensure the shield hitbox is initially disabled
//         if (shieldhitboxCollider != null)
//         {
//             shieldhitboxCollider.enabled = false;
//         }

//         Debug.Log("ShieldEnemy initialized.");
//     }

//     public void CreateShield()
//     {
//         if (!isShieldActive && !anim.GetBool("shield") && !isCooldownActive) // Check if shield is not active and cooldown is not active
//         {
//             Debug.Log("Creating shield...");

//             // Play pre-shield animation if it hasn't been played yet
//                 StartCoroutine(PlayPreShieldAnimationAndCreateShield());
//         }
//         else
//         {
//             shieldhitboxCollider.enabled = false;
//         }
//     }

//     private IEnumerator PlayPreShieldAnimationAndCreateShield()
//     {
//         // Play your pre-shield animation here
//         anim.SetBool("shield_start", true);
//         hasPlayedPreShieldAnimation = true;

//         // Wait for the pre-shield animation to finish (adjust duration as needed)
//         yield return new WaitForSeconds(0.5f); // Adjust duration according to your animation length

//         // After animation, create shield
//         CreateShieldAfterAnimation();
//     }

//     private void CreateShieldAfterAnimation()
//     {
//         // Set pre-shield animation to false
//         anim.SetBool("shield_start", false);

//         // Activate shield
//         anim.SetBool("run", false); // Assuming run animation should be false during shield activation
//         anim.SetBool("shield", true);
//         shieldhitboxCollider.enabled = true;
//         isShieldActive = true;

//         // Start coroutine to deactivate shield after duration
//         StartCoroutine(DeactivateShieldAfterDuration(10f)); // 10 seconds duration
//     }

//     private IEnumerator DeactivateShieldAfterDuration(float duration)
//     {
//         yield return new WaitForSeconds(duration); // Wait for the specified duration

//         // Deactivate shield
//         anim.SetBool("shield", false);
//         shieldhitboxCollider.enabled = false;
//         Debug.Log("Shield deactivated.");
//         isShieldActive = false;

//         // Start cooldown
//         StartCoroutine(StartCooldown(cooldownDuration));
        
//         // Wait for 3 seconds before setting run animation to true
//         yield return new WaitForSeconds(0.3f);
//         anim.SetBool("run", true);
//     }

//     private IEnumerator StartCooldown(float cooldown)
//     {
//         isCooldownActive = true;
//         Debug.Log("Shield cooldown started. Duration: " + cooldown + " seconds.");

//         yield return new WaitForSeconds(cooldown);

//         Debug.Log("Shield cooldown ended.");
//         isCooldownActive = false;
//     }
// }


using UnityEngine;
using System.Collections;

public class ShieldEnemy : Enemy
{
    private CapsuleCollider2D shieldhitboxCollider;
    private bool hasPlayedPreShieldAnimation = false;

    protected override void Start()
    {
        base.Start();

        shieldhitboxCollider = GetComponentInChildren<CapsuleCollider2D>();

        if (shieldhitboxCollider != null)
        {
            shieldhitboxCollider.enabled = false;
        }

        //Debug.Log("ShieldEnemy initialized.");
    }

    protected override IEnumerator ActivateShieldIfNeeded()
    {
        if (!isShieldActive && !anim.GetBool("shield") && !isCooldownActive)
        {
            //Debug.Log("Creating shield...");

            yield return StartCoroutine(PlayPreShieldAnimationAndCreateShield());
        }
        else
        {
            shieldhitboxCollider.enabled = false;
        }
    }

    private IEnumerator PlayPreShieldAnimationAndCreateShield()
    {
        anim.SetBool("attack",false);
        anim.SetBool("shield_start", true);
        hasPlayedPreShieldAnimation = true;

        yield return new WaitForSeconds(0.5f); // Adjust duration according to your animation length

        CreateShieldAfterAnimation();
    }

    private void CreateShieldAfterAnimation()
    {
        anim.SetBool("shield_start", false);

        anim.SetBool(runParameterName, false); // Assuming run animation should be false during shield activation
        anim.SetBool("shield", true);
        shieldhitboxCollider.enabled = true;
        isShieldActive = true;

        StartCoroutine(DeactivateShieldAfterDuration(10f)); // 10 seconds duration
    }

    private IEnumerator DeactivateShieldAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        anim.SetBool("shield", false);
        shieldhitboxCollider.enabled = false;
        //Debug.Log("Shield deactivated.");
        isShieldActive = false;

        StartCoroutine(StartCooldown(cooldownDuration));
        
        yield return new WaitForSeconds(0.3f);
        anim.SetBool(runParameterName, true); // Resume running animation
    }

    private IEnumerator StartCooldown(float cooldown)
    {
        isCooldownActive = true;
        //Debug.Log("Shield cooldown started. Duration: " + cooldown + " seconds.");

        yield return new WaitForSeconds(cooldown);

        //Debug.Log("Shield cooldown ended.");
        isCooldownActive = false;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        // Additional shield enemy specific collision logic
    }


     public override void HandleDetection(Collider2D other)
    {
        if (other.CompareTag("Player") && !anim.GetBool("shield_start") && !anim.GetBool("shield"))
        {
            anim.SetBool("run", false);
            anim.SetBool("attack", true);
            //Debug.Log("Player entered detection range of ShieldEnemy");
        }
    }

    public override void HandleDetectionExit(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DisableAttackAnimationAfterDelay());
            //Debug.Log("Player left detection range of ShieldEnemy");
        }
    }

    private IEnumerator DisableAttackAnimationAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);

        anim.SetBool("attack", false);
        anim.SetBool("run", true);
    }
}
