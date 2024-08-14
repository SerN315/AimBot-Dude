using UnityEngine;
using System.Collections;

public class FlyingRangeEnemy : RangeEnemy
{
    public GameObject projectilePrefab;
    public float shootingInterval = 1f;
    private float lastShootTime;
    public int numberOfGunPoints = 3; // Number of gun points to generate
    public float projectileSpeed = 5f; // Speed of the projectiles
    public float gunPointRadius = 1f; // Radius of the circle around the enemy where gun points are placed
    private Transform[] gunPoints;

    protected override void Start()
    {
        base.Start();
        // Custom behavior for flying enemies
        rb.gravityScale = 0; // Ensure the enemy is not affected by gravity

        // Generate gun points around the enemy
        GenerateGunPoints();
    }

    protected override void Update()
    {
        if (isWaitingAtLedge) return;

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSight)
        {
            currentState = EnemyState.Chasing;
            Vector2 directionToPlayer = (player.position - transform.position).normalized;

            // Adding vertical movement
            Vector2 nextPosition = (Vector2)transform.position + directionToPlayer * speed * Time.deltaTime;
            rb.velocity = directionToPlayer * speed;

            FaceDirection(directionToPlayer.x);

            if (distanceFromPlayer <= range)
            {
                HandleDetection();
            }
        }
        else
        {
            HandleDetectionExit();
            currentState = EnemyState.Patrolling;
            MoveTowardsCurrentPatrolPoint();
        }
    }

    protected override void MoveTowardsCurrentPatrolPoint()
    {
        if (currentPatrolPoint == null) return;

        Vector2 direction = (currentPatrolPoint.position - transform.position).normalized;
        rb.velocity = direction * speed;

        FaceDirection(direction.x);

        float distance = Vector2.Distance(transform.position, currentPatrolPoint.position);
        if (distance < 0.5f)
        {
            CyclePatrolPoints();
        }
    }

    public override void FaceDirection(float directionX)
    {
        if (directionX > 0)
        {
            transform.localScale = new Vector3(-5, 5, 1); // Face right
        }
        else if (directionX < 0)
        {
            transform.localScale = new Vector3(5, 5, 1); // Face left
        }
    }

    public override void HandleDetection()
    {
        if (Time.time > lastShootTime + shootingInterval)
        {
            ShootProjectiles();
            lastShootTime = Time.time;
        }
    }

    public override void HandleDetectionExit()
    {
        // Implement behavior when the player is no longer in range, if necessary
    }

    private void GenerateGunPoints()
    {
        gunPoints = new Transform[numberOfGunPoints];
        float angleStep = 360f / numberOfGunPoints;
        float angle = 0f;

        for (int i = 0; i < numberOfGunPoints; i++)
        {
            float gunPointDirX = Mathf.Sin(angle * Mathf.Deg2Rad) * gunPointRadius;
            float gunPointDirY = Mathf.Cos(angle * Mathf.Deg2Rad) * gunPointRadius;

            Vector3 gunPointPosition = new Vector3(gunPointDirX, gunPointDirY, 0);
            GameObject gunPoint = new GameObject("GunPoint" + i);
            gunPoint.transform.position = transform.position + gunPointPosition;
            gunPoint.transform.SetParent(transform); // Make gun point a child of the enemy

            // Rotate the gun point to face outward
            gunPoint.transform.right = gunPointPosition.normalized; // Face outward

            gunPoints[i] = gunPoint.transform;

            angle += angleStep;
        }
    }

    private void ShootProjectiles()
    {
        foreach (Transform gunPoint in gunPoints)
        {
            // Calculate the direction from the gun point to the target (player or shooting direction)
            Vector2 direction = gunPoint.position - transform.position;

            // Instantiate the projectile at the gun point position
            GameObject projectile = Instantiate(projectilePrefab, gunPoint.position, Quaternion.identity);

            // Set the projectile's rotation to face the direction it will travel
            projectile.transform.right = direction;

            // Get the Rigidbody2D component and set its velocity
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction.normalized * projectileSpeed;
        }
    }

}
