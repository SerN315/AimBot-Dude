using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Material normalMaterial;
    public Material damagedMaterial;

    private Renderer objectRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material = normalMaterial;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //Debug.Log("BreakableObject took damage: " + damage + ", current health: " + currentHealth);

        if (currentHealth <= 50)
        {
            objectRenderer.material = damagedMaterial;
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Method to demonstrate taking damage (e.g., from player or enemy attack)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(10); // Example damage value
        }
    }
}
