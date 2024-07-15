using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    public int health = 100;
    public float speed = 5f;
    public Transform[] patrolPoints;
    public float decelerationRate = 2f;
    public string runParameterName = "run";
    public bool facesLeftByDefault = true;
    private GameManager gameManager;
    private SimpleHit flashEffect;
    public float patrolPointSpacing = 2f;
    public int numberOfPatrolPoints = 3;
    public LayerMask groundLayer;
    public float ledgeCheckDistance = 1f;

    protected bool facingRight = true;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected Transform currentPatrolPoint;
    protected bool isRunning = true;
    protected bool isShieldActive = false;
    protected bool isCooldownActive = false;
    protected float cooldownDuration = 2f;
    private float deathDelay = 0.2f;
    private bool isDead = false;
    public GameObject coinPrefab;
    public GameObject silverCoinPrefab;
    public GameObject bronzeCoinPrefab;
    public GameObject coinBagPrefab;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
           // Ensure the flashEffect is assigned
        if (flashEffect == null)
        {
            flashEffect = GetComponent<SimpleHit>();
            if (flashEffect == null)
            {
                Debug.LogError("FlashEffect is not assigned and SimpleHit component is not found on the player.");
            }
        }

        // Generate patrol points automatically
        GeneratePatrolPoints();

        anim.SetBool(runParameterName, true);
         gameManager = FindObjectOfType<GameManager>();
    }

    protected virtual void FixedUpdate()
    {
        if (anim.GetBool("run") && !isShieldActive && !anim.GetBool("attack") && !anim.GetBool("charge") && !anim.GetBool("charge_start"))
        {
            MoveTowardsCurrentPatrolPoint();
        }
        else if (!anim.GetBool("run") && !isShieldActive && !anim.GetBool("attack") && !anim.GetBool("charge") && !anim.GetBool("charge_start"))
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            DecelerateMovement();
        }
    }

    protected virtual void MoveTowardsCurrentPatrolPoint()
    {
        if (currentPatrolPoint == null) return;

        Vector2 direction = (currentPatrolPoint.position - transform.position).normalized;
        rb.velocity = direction * speed;

        // Update the rotation to face the direction of movement
        if (direction != Vector2.zero)
        {
            if (facesLeftByDefault)
            {
                if (direction.x < 0)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0); // Flip to face left
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0); // Face right
                }
            }
            else if (!facesLeftByDefault)
            {
                if (direction.x < 0)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0); // Flip to face left
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0); // Face right
                }
            }
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
        // Debug.Log("Projectile touched the enemy");
        Destroy(collision.gameObject);

        if (!isShieldActive)
        {
            TakeDamage(10); 
        }
    }

    protected virtual void HandlePlayerCollision(Collision2D collision)
    {
        // Debug.Log("Player touched the enemy");
        PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(10); 
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (isShieldActive)
        {
            // Debug.Log("Shield is active. No damage taken.");
            return;
        }
        if (flashEffect != null)
        {
            flashEffect.Flash();
        }


        health -= damage;
        Debug.Log("Enemy took damage: " + damage + ", current health: " + health);

        if (health < 50 && health > 0 && !isShieldActive && !isCooldownActive)
        {
            StartCoroutine(ActivateShieldIfNeeded());
        }

        if (health <= 0 && !isDead)
        {
            Die();
            gameManager.EnemyDestroyed();
        }
    }  
     protected virtual void Die()
    {
            isDead = true;
            anim.SetBool("dead", true);
            StartCoroutine(HandleDeath());
            
        
    }
    private IEnumerator HandleDeath()
    {
        // Debug.Log("Handling Death");
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
        int coinValue = Random.Range(5, 101); // Random value between 5 and 100
        SpawnCoins(coinValue);

        
    }

private void SpawnCoins(int value)
{
    while (value > 0)
    {
        if (value >= 60)
        {
            Instantiate(coinBagPrefab, transform.position, Quaternion.identity);
            CurrencyManager.Instance.AddCurrency(60);
            value -= 60;
        }
        else if (value >= 30)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
            CurrencyManager.Instance.AddCurrency(30);
            value -= 30;
        }
        else if (value >= 20)
        {
            Instantiate(silverCoinPrefab, transform.position, Quaternion.identity);
            CurrencyManager.Instance.AddCurrency(20);
            value -= 20;
        }
        else if (value >= 10)
        {
            Instantiate(bronzeCoinPrefab, transform.position, Quaternion.identity);
            CurrencyManager.Instance.AddCurrency(10);
            value -= 10;
        }
        else
        {
            break;
        }
    }
}
    protected virtual IEnumerator ActivateShieldIfNeeded()
    {
        yield return null; 
    }

    protected virtual IEnumerator ChargeAttack()
    {
        yield return null; 
    }

 


    public abstract void HandleDetection(Collider2D other);

    public abstract void HandleDetectionExit(Collider2D other);
private void GeneratePatrolPoints()
{
    patrolPoints = new Transform[numberOfPatrolPoints];
    Vector3 startPoint = transform.position;

    // Get the bottom position of the enemy's collider
    Collider2D collider = GetComponent<Collider2D>();
    Vector3 bottomCenter = new Vector3(startPoint.x, startPoint.y - collider.bounds.extents.y, startPoint.z);

    // Determine the spacing between patrol points
    float effectiveSpacing = patrolPointSpacing;

    // Check if the enemy is standing on ground or a platform
    RaycastHit2D hit = Physics2D.Raycast(bottomCenter, Vector2.down, ledgeCheckDistance, groundLayer);
    if (hit.collider != null)
    {
        // Calculate the length of the platform within the ground layer
        float platformLength = CalculatePlatformLength(hit.point, hit.collider);

        // Adjust spacing if it's longer than the platform length
        if (platformLength < patrolPointSpacing)
        {
            effectiveSpacing = platformLength;
        }
    }

    // Place patrol points at the calculated positions
    Vector3 leftPatrolPoint = startPoint + new Vector3(-effectiveSpacing / 2f, 0, 0);
    Vector3 rightPatrolPoint = startPoint + new Vector3(effectiveSpacing / 2f, 0, 0);

    // Ensure the Y position matches the center of the enemy
    leftPatrolPoint.y = transform.position.y;
    rightPatrolPoint.y = transform.position.y;

    // Create and assign positions to patrol points
    patrolPoints[0] = new GameObject("PatrolPoint0").transform;
    patrolPoints[0].position = leftPatrolPoint;

    patrolPoints[1] = new GameObject("PatrolPoint1").transform;
    patrolPoints[1].position = rightPatrolPoint;

    if (patrolPoints.Length >= 2)
    {
        currentPatrolPoint = patrolPoints[0];
    }
}

private float CalculatePlatformLength(Vector2 hitPoint, Collider2D hitCollider)
{
    float leftEdge = hitCollider.bounds.min.x;
    float rightEdge = hitCollider.bounds.max.x;
    return rightEdge - leftEdge;
}


private float CalculatePlatformLength(Vector3 start)
{
    Vector2 start2D = new Vector2(start.x, start.y);
    RaycastHit2D hitLeft = Physics2D.Raycast(start2D, Vector2.left, Mathf.Infinity, groundLayer);
    RaycastHit2D hitRight = Physics2D.Raycast(start2D, Vector2.right, Mathf.Infinity, groundLayer);

    float length = 0f;
    if (hitLeft.collider != null)
    {
        length += Mathf.Abs(hitLeft.point.x - start2D.x);
    }
    if (hitRight.collider != null)
    {
        length += Mathf.Abs(hitRight.point.x - start2D.x);
    }

    return length;
}

private bool IsAboveGround(Vector3 position)
{
    RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, ledgeCheckDistance, groundLayer);
    return hit.collider != null;
}

}