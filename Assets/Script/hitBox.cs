
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Enemy enemy;

    private Animator anim;

   private CapsuleCollider2D hitboxCollider;
    void Start()
    {
        // anim = GetComponentInParent<Animator>();
        hitboxCollider = GetComponent<CapsuleCollider2D>(); 

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in hitbox range");
            // anim.SetBool("attack", true);
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(5); // Example: Player takes 5 damage per second in hitbox range
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left hitbox range");
            // anim.SetBool("attack", false);
        }
    }
}