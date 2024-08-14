using UnityEngine;
using System.Collections;

public class SniperRange : RangeEnemy
{
    private SniperAttack sniperAttack;

    protected override void Start()
    {
        base.Start();
        health = 200; // Higher health for tank
        speed = 3f; // Slower speed for tank
        sniperAttack = GetComponent<SniperAttack>();
    }

    protected override void Update()
    {
        if (isWaitingAtLedge)
        {
            return; // Skip other logic while waiting at a ledge
        }

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSight)
        {
            currentState = EnemyState.Chasing;
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0; // Ignore elevation differences

            if (distanceFromPlayer <= range)
            {
                rb.velocity = Vector2.zero; // Stop movement
                FaceDirection(directionToPlayer.x);
                HandleDetection();
            }
            else
            {
                Vector2 nextPosition = (Vector2)transform.position + directionToPlayer * speed * Time.deltaTime;
                if (CheckForGroundAhead(nextPosition))
                {
                    rb.velocity = new Vector2(directionToPlayer.x * speed, rb.velocity.y);
                    FaceDirection(directionToPlayer.x);
                }
                else
                {
                    StartCoroutine(WaitAtLedge());
                }
            }
        }
        else
        {
            HandleDetectionExit();
            currentState = EnemyState.Patrolling;
            MoveTowardsCurrentPatrolPoint();
        }
    }

    public override void HandleDetection()
    {
        if (!sniperAttack.isSniping)
        {
            sniperAttack.StartSniping(player.gameObject);
        }
    }

    public override void HandleDetectionExit()
    {
        // Optionally disable the sniper attack when out of range
        sniperAttack.StopSniping();
    }

    public override void FaceDirection(float directionX)
    {
        if (directionX > 0)
        {
            transform.localScale = new Vector3(10, 10, 1); // Face right
        }
        else if (directionX < 0)
        {
            transform.localScale = new Vector3(-10, 10, 1); // Face left
        }
    }
}


