using UnityEngine;
using System.Collections;

public class Detection : MonoBehaviour
{
    private Animator anim;
    private Enemy enemy;
    public float delayTime = 0.3f;
    private PlayerStats playerStats;
    private CapsuleCollider2D hitboxCollider;
    private BoxCollider2D detectionCollider;
    private bool playerInRange = false;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        playerStats = FindObjectOfType<PlayerStats>(); // Reference to the PlayerStats script
        enemy = GetComponentInParent<Enemy>(); // Reference to the Enemy script in the parent

        hitboxCollider = transform.Find("hitBox").GetComponent<CapsuleCollider2D>();
        detectionCollider = GetComponent<BoxCollider2D>();
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemy != null && !IsPlayerDead())
        {
            enemy.HandleDetection(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemy != null && !IsPlayerDead())
        {
            enemy.HandleDetectionExit(other);
        }
    }

    private bool IsPlayerDead()
    {
        // Check if playerStats is assigned and if the player is dead
        return playerStats == null || playerStats.IsDead();
    }
}
