using UnityEngine;
using System.Collections;

public class SniperAttack : EnemyAttack
{
    public LineRenderer lineRenderer;
    public float sniperAimTime = 2f;
    public float sniperCooldown = 3f;
    private bool isSniping = false;

    void Update()
    {
        if (!isSniping)
        {
            GameObject nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                StartCoroutine(SniperRoutine(nearestEnemy));
            }
        }
    }

    public override void Attack(GameObject target)
    {
        // Sniper logic handled in SniperRoutine coroutine
    }

    private IEnumerator SniperRoutine(GameObject target)
    {
        isSniping = true;

        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.transform.position);

        yield return new WaitForSeconds(sniperAimTime);

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        yield return new WaitForSeconds(0.5f);

        // Apply damage logic here
        // Example: target.GetComponent<Health>().TakeDamage(damageAmount);

        lineRenderer.enabled = false;
        yield return new WaitForSeconds(sniperCooldown);
        isSniping = false;
    }
}
