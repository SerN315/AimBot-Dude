
// using UnityEngine;

// public class Hitbox : MonoBehaviour
// {
//     private Enemy enemy;
//     private bool hasHitPlayer = false;
//     private Animator anim;

//    private CapsuleCollider2D hitboxCollider;
//     void Start()
//     {
//         // anim = GetComponentInParent<Animator>();
//         hitboxCollider = GetComponent<CapsuleCollider2D>(); 

//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") && !hasHitPlayer)
//         {
//             Debug.Log("Player in hitbox range");
//             // anim.SetBool("attack", true);
//             PlayerStats playerStats = other.GetComponent<PlayerStats>();
//             if (playerStats != null)
//             {
//                 playerStats.TakeDamage(5); // Example: Player takes 5 damage per second in hitbox range
//                 hasHitPlayer = true; // Set flag to true to prevent further hits
//             }
//         }
//     }

//     void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             Debug.Log("Player left hitbox range");
//             // anim.SetBool("attack", false);
//             hasHitPlayer = false;
//         }
//     }
// }

using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Enemy enemy; // Assuming you have a reference to the enemy script here if needed
    private bool hasHitPlayer = false;
    private Animator anim;
    private CapsuleCollider2D hitboxCollider;

    // Damage value specific to each enemy type
    private int damageAmount = 5; // Default damage amount

    void Start()
    {
        hitboxCollider = GetComponent<CapsuleCollider2D>(); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasHitPlayer)
        {
            Debug.Log("Player in hitbox range");

            // Get the specific enemy type from the parent GameObject
            enemy = GetComponentInParent<Enemy>(); // Adjust as per your structure

            // Adjust damage amount based on enemy type
            if (enemy is ShieldEnemy)
            {
                damageAmount = 10; // Example: Shield enemies do more damage
            }
            else if (enemy is TankEnemy)
            {
                damageAmount = 20; // Example: Shield enemies do more damage
            }
            else if (enemy is MeleeEnemy)
            {
                damageAmount = 15; // Example: Shield enemies do more damage
            }



            // Add more conditions for other enemy types as needed

            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
                hasHitPlayer = true; // Set flag to true to prevent further hits
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left hitbox range");
            hasHitPlayer = false;
        }
    }
}
