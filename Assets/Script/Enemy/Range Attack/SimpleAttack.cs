using UnityEngine;

public class StandardProjectileAttack : EnemyAttack
{
    public GameObject bulletPrefab;
    public float fireRate;
    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        GameObject nearestEnemy = FindNearestEnemy();
        if (fireTimer >= 1f / fireRate && nearestEnemy != null)
        {
            Attack(nearestEnemy);
            fireTimer = 0f;
        }
    }

    public override void Attack(GameObject target)
    {
        Vector3 direction = target.transform.position - gunHand.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunHand.rotation = Quaternion.Euler(0, 0, angle);

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
