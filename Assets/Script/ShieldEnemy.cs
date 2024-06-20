using System.Collections;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D shieldhitboxCollider;
    private Vector2 originalVelocity; // To store the original velocity of the enemy


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        shieldhitboxCollider = GetComponentInChildren<CircleCollider2D>();

        // Ensure the shield hitbox is initially disabled
        if (shieldhitboxCollider != null)
        {
            shieldhitboxCollider.enabled = false;
        }

        Debug.Log("ShieldEnemy initialized.");
    }

    public void CreateShield()
    {
        if (!anim.GetBool("shield")) // Check if the shield is not already active
        {
            Debug.Log("Creating shield...");
            anim.SetBool("run", false);
            anim.SetBool("shield", true);
            StartCoroutine(ActivateShieldForDuration(1.5f)); // 1.5 seconds duration
        }
        else
        {
            Debug.Log("Shield is already active.");
        }
    }

    private IEnumerator ActivateShieldForDuration(float duration)
    {
        // Store the original velocity and stop the enemy
        originalVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // Disable physics interactions
        Debug.Log("Shield activated. Enemy stopped.");

        // Enable the shield hitbox
        if (shieldhitboxCollider != null)
        {
            shieldhitboxCollider.enabled = true;
        }

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Set the shield animation parameter to false
        anim.SetBool("shield", false);
        Debug.Log("Shield deactivated.");

        // Disable the shield hitbox
        if (shieldhitboxCollider != null)
        {
            shieldhitboxCollider.enabled = false;
        }

        // Restore the original velocity and resume physics interactions
        rb.isKinematic = false;
        rb.velocity = originalVelocity;
        Debug.Log("Enemy resumed movement.");

        anim.SetBool("run", true);
    }
}
