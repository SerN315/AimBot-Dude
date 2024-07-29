using UnityEngine;
using System.Collections;

public class Traps : MonoBehaviour
{
    [SerializeField] private int damageAmount = 30; // Amount of damage the trap deals
    [SerializeField] private float damageInterval = 1f; // Time interval between damage applications

    private Coroutine damageCoroutine;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            HandlePlayerCollision(collision);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StopDamageCoroutine();
        }
    }

    void HandlePlayerCollision(Collision2D collision)
    {
        PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            // Deal immediate damage
            playerStats.TakeDamage(damageAmount);

            // Start coroutine to deal damage repeatedly if the player stays in contact with the trap
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(playerStats));
            }
        }
    }

    void StopDamageCoroutine()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    IEnumerator DealDamageOverTime(PlayerStats playerStats)
    {
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
            }
        }
    }
}
