using UnityEngine;

public class Attack : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public Transform gunHand;
    public Transform firePoint;
    private int bulletDamage;
    private GameObject bulletPrefab;
    private string additionalEffect;
    private float fireRate;
    private float fireTimer = 0f;
    private int additionalDamage = 0; // Additional damage from power-ups
    public bool applyExplosiveBullet = false;
    public bool applyPiercingBullet = false;

    public bool enableOppositeGunpoint = false;
    public bool enableMultipleGunpoints = false;
    public int numberOfScatterBullets = 3; // Number of bullets in the scatter shot
    public float scatterAngleRange = 30f; // The angle range for scatter shot

    private Transform oppositeFirePoint;
    private Rigidbody2D playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
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

            // Create opposite gunpoint if enabled and not yet created
            if (enableOppositeGunpoint && oppositeFirePoint == null)
            {
                CreateOppositeGunpoint();
            }
        }

        // Increment the timer by the time passed since the last frame
        fireTimer += Time.deltaTime;

        // If the timer exceeds the fire interval and there is an enemy, fire bullets
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
        if (firePoint == null || bulletPrefab == null)
        {
            return;
        }

        if (enableMultipleGunpoints)
        {
            // Fire scatter bullets in a cone direction
            FireScatterShot();
        }
        else
        {
            // Shoot from the original fire point
            FireBullet(firePoint);
        }

        // Shoot from the opposite fire point if enabled
        if (enableOppositeGunpoint && oppositeFirePoint != null)
        {
            FireBullet(oppositeFirePoint);
        }
    }

    void FireBullet(Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullets bulletComponent = bullet.GetComponent<Bullets>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDamage(bulletDamage + additionalDamage);
            ApplyAdditionalEffect(bulletComponent);
        }
    }

    void FireScatterShot()
    {
        float baseAngle = firePoint.rotation.eulerAngles.z;
        float startAngle = baseAngle - scatterAngleRange / 2f;
        float angleStep = scatterAngleRange / (numberOfScatterBullets - 1);

        for (int i = 0; i < numberOfScatterBullets; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Quaternion bulletRotation = Quaternion.Euler(0, 0, currentAngle);
            FireBulletWithRotation(firePoint.position, bulletRotation);
        }
    }

    void FireBulletWithRotation(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        Bullets bulletComponent = bullet.GetComponent<Bullets>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDamage(bulletDamage + additionalDamage);
            ApplyAdditionalEffect(bulletComponent);
        }
    }

    public void SetGunProperties(GameObject newBulletPrefab, float newFireRate, string newAdditionalEffect, int newBulletDamage)
    {
        bulletPrefab = newBulletPrefab;
        fireRate = newFireRate;
        additionalEffect = newAdditionalEffect;
        bulletDamage = newBulletDamage;
    }

    public void ApplyAdditionalEffect(Bullets bulletComponent)
    {
        if (applyExplosiveBullet)
        {
            bulletComponent.MakeExplosive();
        }
        if (applyPiercingBullet)
        {
            bulletComponent.MakePiercing();
        }

        switch (additionalEffect)
        {
            case "explosive":
                bulletComponent.MakeExplosive();
                break;
            case "piercing":
                bulletComponent.MakePiercing();
                break;
        }
    }

    public void ApplyDamagePowerUp(int damage)
    {
        additionalDamage += damage;
    }

    void CreateOppositeGunpoint()
    {
        oppositeFirePoint = new GameObject("OppositeFirePoint").transform;
        oppositeFirePoint.parent = gunHand;
        oppositeFirePoint.localPosition = new Vector3(-firePoint.localPosition.x, firePoint.localPosition.y, firePoint.localPosition.z) - new Vector3(0.2f, 0, 0);
        oppositeFirePoint.localRotation = firePoint.localRotation * Quaternion.Euler(0, 0, 180);
    }

    void UpdateOppositeGunpoint()
    {
        if (oppositeFirePoint != null)
        {
            oppositeFirePoint.localPosition = new Vector3(-firePoint.localPosition.x, firePoint.localPosition.y, firePoint.localPosition.z) - new Vector3(0.2f, 0, 0);
            oppositeFirePoint.localRotation = firePoint.localRotation * Quaternion.Euler(0, 0, 180);
        }
    }
}
