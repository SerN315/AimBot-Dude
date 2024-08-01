using UnityEngine;
using System.Collections;

public abstract class RangeEnemy : MonoBehaviour
{
    public int health = 100;
    public float speed = 5f;
    public Transform[] patrolPoints;
    public float decelerationRate = 2f;
    public string runParameterName = "run";
    public bool facesLeftByDefault = true;
    private GameManager gameManager;
    private SimpleHit flashEffect;
    public float patrolPointSpacing = 8f;
    public int numberOfPatrolPoints = 3;
    public LayerMask groundLayer;
    public float ledgeCheckDistance = 1f;
    public float lineOfSight = 5f;
    public float range;
    private Transform player;
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


    private bool isWaitingAtLedge = false;

    // Enemy states
    private enum EnemyState { Patrolling, Chasing }
    private EnemyState currentState = EnemyState.Patrolling;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

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
        anim.SetBool("run", true);
        gameManager = FindObjectOfType<GameManager>();
    }

    protected virtual void Update()
    {
        if (isWaitingAtLedge)
        {
            return; // Skip other logic while waiting at a ledge
        }

        // Check if the Rigidbody2D velocity is close to zero
        if (rb.velocity.magnitude <= 0)
        {
            anim.SetBool("run", false);
        }
        else
        {
            anim.SetBool("run", true);
        }

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSight)
        {
            currentState = EnemyState.Chasing;
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0; // Ignore elevation differences

            Vector2 nextPosition = (Vector2)transform.position + directionToPlayer * speed * Time.deltaTime;

            if (CheckForGroundAhead(nextPosition))
            {
                rb.velocity = new Vector2(directionToPlayer.x * speed, rb.velocity.y);
                FaceDirection(directionToPlayer.x);

                if (distanceFromPlayer <= range)
                {
                    HandleDetection();
                }
            }
            else
            {
                StartCoroutine(WaitAtLedge());
            }
        }
        else
        {
            HandleDetectionExit();
            currentState = EnemyState.Patrolling;
            MoveTowardsCurrentPatrolPoint();
        }
    }

    protected virtual void MoveTowardsCurrentPatrolPoint()
    {
        if (currentPatrolPoint == null) return;

        Vector2 direction = (currentPatrolPoint.position - transform.position).normalized;
        Vector2 nextPosition = (Vector2)transform.position + direction * speed * Time.deltaTime;

        if (CheckForGroundAhead(nextPosition))
        {
            rb.velocity = direction * speed;
            FaceDirection(direction.x);

            float distance = Vector2.Distance(transform.position, currentPatrolPoint.position);

            if (distance < 0.5f)
            {
                CyclePatrolPoints();
            }
        }
    }

    private void FaceDirection(float directionX)
    {
        if (directionX > 0)
        {
            transform.localScale = new Vector3(5, 5, 1); // Face right
        }
        else if (directionX < 0)
        {
            transform.localScale = new Vector3(-5, 5, 1); // Face left
        }
    }

    private IEnumerator WaitAtLedge()
    {
        isWaitingAtLedge = true;

        rb.velocity = Vector2.zero;
        anim.SetBool("run", false);

        yield return new WaitForSeconds(1.0f); // Adjust wait time as needed

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;

        if (distanceFromPlayer < lineOfSight && CheckForGroundAhead((Vector2)transform.position + directionToPlayer * speed * Time.deltaTime))
        {
            rb.velocity = new Vector2(directionToPlayer.x * speed, rb.velocity.y);
            anim.SetBool("run", true);
            FaceDirection(directionToPlayer.x);
        }
        else
        {
            currentState = EnemyState.Patrolling;
            MoveTowardsCurrentPatrolPoint();
        }

        isWaitingAtLedge = false;
    }


    // Abstract method for handling attack
    public abstract void HandleDetection();
    public abstract void HandleDetectionExit();

  


    protected bool CheckForGroundAhead(Vector2 nextPosition)
    {
        // Cast a ray downwards from the next position to check for ground
        RaycastHit2D hit = Physics2D.Raycast(nextPosition, Vector2.down, 1.0f, groundLayer);
        return hit.collider != null;
    }

    protected Vector2 FindLedgePosition(Vector2 direction)
    {
        // Start checking slightly ahead of the current position
        float checkDistance = 0.5f;
        Vector2 startPosition = (Vector2)transform.position + new Vector2(checkDistance * direction.x, 0);

        // Cast a ray downwards from the start position to find the ledge
        RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.down, 1.0f, groundLayer);
        if (hit.collider != null)
        {
            return hit.point;
        }

        return Vector2.zero; // No ledge found
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
    }
}

