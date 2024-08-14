using UnityEngine;
using System.Collections;

public class SniperAttack : EnemyAttack
{
    public LineRenderer lineRenderer;
    public float sniperAimTime = 2f;
    public float preDamageDuration = 0.5f; // Time between stopping tracking and causing damage
    public float sniperCooldown = 1f;
    public float hitboxDuration = 0.5f;
    public int damageAmount = 20;
    public bool isSniping = false;

    private Coroutine sniperRoutine;
    private Vector3 lastTrackedPosition;
    private bool hasDamagedPlayer = false;

    void Start()
    {
        lineRenderer.enabled = false;
    }

    public void StartSniping(GameObject target)
    {
        if (!isSniping)
        {
            sniperRoutine = StartCoroutine(SniperRoutine(target));
        }
    }

    public void StopSniping()
    {
        if (sniperRoutine != null)
        {
            StopCoroutine(sniperRoutine);
        }
        isSniping = false;
        lineRenderer.enabled = false;
    }

    private IEnumerator SniperRoutine(GameObject target)
    {
        isSniping = true;
        hasDamagedPlayer = false;

        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        float elapsedTime = 0f;

        while (elapsedTime < sniperAimTime)
        {
            lastTrackedPosition = target.transform.position;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, lastTrackedPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Lock the line at the last tracked position
        lineRenderer.SetPosition(1, lastTrackedPosition);

        // Delay before causing damage
        yield return new WaitForSeconds(preDamageDuration);

        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;

        // Enable the hitbox and apply damage
        float damageEndTime = Time.time + hitboxDuration;
        while (Time.time < damageEndTime)
        {
            ExtendLine();
            if (!hasDamagedPlayer && CheckCollisionWithPlayer(target))
            {
                ApplyDamage(target);
                hasDamagedPlayer = true; // Ensure damage is applied only once
            }
            yield return null;
        }

        lineRenderer.enabled = false;

        yield return new WaitForSeconds(sniperCooldown);

        isSniping = false;
    }

    private void ExtendLine()
    {
        RaycastHit2D hit = Physics2D.Raycast(lastTrackedPosition, (lastTrackedPosition - firePoint.position).normalized);
        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
    }

    private bool CheckCollisionWithPlayer(GameObject target)
    {
        // Check if the player is touching the line
        Vector2 playerPosition = target.transform.position;
        Vector2 closestPoint = lineRenderer.GetPosition(0);
        float minDistance = Vector2.Distance(playerPosition, closestPoint);

        for (int i = 1; i < lineRenderer.positionCount; i++)
        {
            Vector2 linePoint = lineRenderer.GetPosition(i);
            float distance = Vector2.Distance(playerPosition, linePoint);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = linePoint;
            }
        }

        // Check if the distance from the player to the closest point on the line is less than or equal to the line width
        return minDistance <= lineRenderer.endWidth / 2;
    }

    private void ApplyDamage(GameObject target)
    {
        // Apply damage logic here
        PlayerStats targetHealth = target.GetComponent<PlayerStats>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
        }
    }

    public override void Attack(GameObject target)
    {
        // Sniper logic handled in SniperRoutine coroutine
    }
}
