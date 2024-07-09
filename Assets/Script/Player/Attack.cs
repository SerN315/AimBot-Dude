using UnityEngine;

public class Attack : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public Transform gunHand;
    public float fireRate = 5f; // Bullets per second
    public Transform firePoint;
    public GameObject BulletPrefab;

    private float fireTimer = 0f;
    private int additionalDamage = 0; // Additional damage from power-ups

    void Update()
    {
        GameObject nearestEnemy = FindNearestEnemy();

        // If an enemy is found, point the gunHand towards it
        if (nearestEnemy != null)
        {
            Vector3 direction = nearestEnemy.transform.position - gunHand.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gunHand.rotation = Quaternion.Euler(0, 0, angle);

            // Flip gunHand if necessary
            if (direction.x > 0)
            {
                gunHand.localScale = new Vector3(1, 1, 1);
            }
            else if (direction.x < 0)
            {
                gunHand.localScale = new Vector3(-1, -1, 1);
            }
        }

        // Increment the timer by the time passed since the last frame
        fireTimer += Time.deltaTime;

        // If the timer exceeds the fire interval and there is an enemy, fire a bullet
        if (fireTimer >= 1f / fireRate && nearestEnemy != null)
        {
            Shoot();
            fireTimer = 0f; // Reset the timer
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(gunHand.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
        Bullets bulletComponent = bullet.GetComponent<Bullets>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDamage(additionalDamage);
        }
    }

    public void ApplyDamagePowerUp(int damage)
    {
        additionalDamage += damage;
    }
}
