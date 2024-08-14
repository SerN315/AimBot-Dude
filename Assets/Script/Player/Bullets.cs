using UnityEngine;

public class Bullets : MonoBehaviour
{
    private bool hit;
    public float speed = 0;
    public int damage = 20; // Base damage from the bullet
    public int explosionDamage = 30; // Additional damage caused by the explosion
    private float timer;
    public Rigidbody2D rb;
    public bool isExplosive = false;
    public bool isPiercing = false;
    public float explosionRadius = 2f; // Radius for explosive effect
    public GameObject explosionPrefab; // Reference to the explosion prefab
    private int TotalExplosionDamage;

    void Awake()
    {
        rb.velocity = transform.right * speed;
        timer = 0;
        TotalExplosionDamage = damage + explosionDamage;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("OneWayPlatform"))
        {
            Physics2D.IgnoreCollision(hitInfo, GetComponent<Collider2D>());
            return;
        }

        if (hitInfo.CompareTag("Enemy"))
        {
            if (!isExplosive)
            {
                ApplyInitialDamage(hitInfo);
            }

            if (isExplosive)
            {
                Explode(); // Apply explosion damage to all enemies in radius
            }

            if (!isPiercing)
            {
                Destroy(gameObject);
            }
        }
        else if (!isPiercing || !hitInfo.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    private void ApplyInitialDamage(Collider2D hitInfo)
    {
        RangeEnemy rangeenemy = hitInfo.GetComponent<RangeEnemy>();
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Apply initial damage
        }
        if (rangeenemy != null)
        {
            rangeenemy.TakeDamage(damage); // Apply initial damage
        }
    }

    private void Explode()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                RangeEnemy rangeenemy = enemy.GetComponent<RangeEnemy>();
                Enemy baseEnemy = enemy.GetComponent<Enemy>();
                if (baseEnemy != null)
                {
                    baseEnemy.TakeDamage(TotalExplosionDamage); // Apply explosion damage
                }
                if (rangeenemy != null)
                {
                    rangeenemy.TakeDamage(TotalExplosionDamage); // Apply explosion damage
                }
            }
        }

        // Instantiate the explosion effect and scale it
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Calculate the scale factor
            float scaleFactor = explosionRadius / explosion.GetComponent<Renderer>().bounds.size.x;
            explosion.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void SetDamage(int additionalDamage)
    {
        damage += additionalDamage;
    }

    public void DamageUp(int DamageAmount)
    {
        damage += DamageAmount;
    }

    public void ResetDamage()
    {
        damage = 20;
    }

    public void MakeExplosive()
    {
        isExplosive = true;
    }

    public void MakePiercing()
    {
        isPiercing = true;
    }
}
