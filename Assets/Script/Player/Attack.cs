using UnityEngine;

public class Attack : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public Transform gunHand;
    public float bulletsPerSecond = 5f; // Number of bullets per second
    public Transform firePoint;
    public GameObject BulletPrefab;

    private float fireInterval;
    public float secondsdelay;
    private float fireTimer = 0f;

    void Start()
    {
        // Calculate the interval between each bullet based on the fire rate
        fireInterval = secondsdelay / bulletsPerSecond;
    }

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

        // Increment the timer by the time passed since last frame
        fireTimer += Time.deltaTime;

        // If the timer exceeds the fire interval and there is an enemy, fire a bullet
        if (fireTimer >= fireInterval && nearestEnemy != null)
        {
            Shoot();
            fireTimer -= fireInterval; // Reset the timer, taking into account the excess time
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
        Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
    }
}
