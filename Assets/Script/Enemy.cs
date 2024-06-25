using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    // Fields common to all enemies
    public int health = 100;
    public float speed = 5f;
    public Transform[] patrolPoints;
    public float decelerationRate = 2f;
    public string runParameterName = "run";

    // Protected fields for internal use
    protected Rigidbody2D rb;
    protected Animator anim;
    protected Transform currentPatrolPoint;
    protected bool isRunning = true;
    protected bool isShieldActive = false;
    protected bool isCooldownActive = false;
    protected float cooldownDuration = 2f; // Cooldown duration in seconds

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Start patrolling between the defined points
        if (patrolPoints.Length >= 2)
        {
            currentPatrolPoint = patrolPoints[0];
        }

        anim.SetBool(runParameterName, true);
    }

    protected virtual void FixedUpdate()
    {
  if (isRunning && !isShieldActive && !anim.GetBool("attack") && !anim.GetBool("charge_start"))
    {
        MoveTowardsCurrentPatrolPoint();
    }
    else if (!anim.GetBool("attack") && !anim.GetBool("charge_start"))
    {
        DecelerateMovement();
    }

    // Synchronize movement during charging state for melee enemies
    else if (anim.GetBool("charge_start") && !anim.GetBool("attack"))
    {
        DecelerateMovement();
    }

    // Add additional synchronization logic for other enemy types here
    }

    protected void MoveTowardsCurrentPatrolPoint()
    {
        Vector2 direction = (currentPatrolPoint.position - transform.position).normalized;
        rb.velocity = direction * speed;
  // Update the rotation to face the direction of movement
    if (direction != Vector2.zero)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle + 180, 0));
    }
        float distance = Vector2.Distance(transform.position, currentPatrolPoint.position);

    if (distance < 0.5f)
        {
            // Switch to the next patrol point when close enough
            CyclePatrolPoints();
        }
    }

    protected void CyclePatrolPoints()
    {
        // Cycle to the next patrol point in the array
        int currentIndex = System.Array.IndexOf(patrolPoints, currentPatrolPoint);
        int nextIndex = (currentIndex + 1) % patrolPoints.Length;
        currentPatrolPoint = patrolPoints[nextIndex];
    }

    protected void DecelerateMovement()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime * decelerationRate);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            HandleProjectileCollision(collision);
        }
        else if (collision.collider.CompareTag("Player"))
        {
            HandlePlayerCollision(collision);
        }
    }

    protected virtual void HandleProjectileCollision(Collision2D collision)
    {
        Debug.Log("Projectile touched the enemy");
        Destroy(collision.gameObject); // Destroy the projectile

        if (!isShieldActive)
        {
            TakeDamage(10); // Example: Enemy takes 10 damage on collision with projectile
        }
    }

    protected virtual void HandlePlayerCollision(Collision2D collision)
    {
        Debug.Log("Player touched the enemy");
        PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(0); // Example: Player takes 0 damage (modify as needed)
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (isShieldActive)
        {
            // If shield is active, take no damage
            Debug.Log("Shield is active. No damage taken.");
            return;
        }

        health -= damage;
        Debug.Log("Enemy took damage: " + damage + ", current health: " + health);

        if (health < 50 && health > 0 && !isShieldActive && !isCooldownActive)
        {
            StartCoroutine(ActivateShieldIfNeeded());
        }

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual IEnumerator ActivateShieldIfNeeded()
    {
        yield return null; // Placeholder return
    }
        protected virtual IEnumerator ChargeAttack()
    {
        yield return null; // Placeholder return
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
        // Add abstract method for handling detection
    public abstract void HandleDetection(Collider2D other);

    // Add abstract method for handling detection exit
    public abstract void HandleDetectionExit(Collider2D other);
}
