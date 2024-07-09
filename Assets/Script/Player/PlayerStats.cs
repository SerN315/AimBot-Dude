using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private SimpleHit flashEffect;
    public int health = 100;

    private Rigidbody2D rb;
    private Animator anim;
    private GameManager gameManager;
    public GameObject gunHand;
    private SimpleHit gunHandFlashEffect;
    [SerializeField] private float deathDelay = 1f;
    private bool isDead = false;
    [SerializeField] private int currentHealth;

    void Start()
    {
        currentHealth = health;
    Debug.Log("Initial health set to: " + currentHealth);
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (flashEffect == null)
        {
            flashEffect = GetComponent<SimpleHit>();
            if (flashEffect == null)
            {
                Debug.LogError("FlashEffect is not assigned and SimpleHit component is not found on the player.");
            }
        }

        if (gunHand != null)
        {
            gunHandFlashEffect = gunHand.GetComponent<SimpleHit>();
            if (gunHandFlashEffect == null)
            {
                Debug.LogError("SimpleHit component is not found on the gunHand.");
            }
        }
        else
        {
            Debug.LogError("GunHand is not assigned.");
        }
    }

    void Update()
    {
        // Update logic if necessary
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player takes damage: " + damage); // Log damage taken

        if (flashEffect != null)
        {
            flashEffect.Flash();
        }

        if (gunHandFlashEffect != null)
        {
            gunHandFlashEffect.Flash();
        }

        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            anim.SetBool("died", true);
            if (gunHand != null)
            {
                gunHand.SetActive(false);
            }
            Debug.Log("Player health reached zero or below.");
            StartCoroutine(HandlePlayerDeath());
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        yield return new WaitForSeconds(deathDelay);
        gameManager.PlayerDied();
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void RecoverHealth(int amount)
    {
    Debug.Log("Current health before recovery: " + currentHealth);
    currentHealth = Mathf.Min(currentHealth + amount, health);
    Debug.Log("Recovered health by " + amount + ". Current health after recovery: " + currentHealth);
    }
    public void IncreaseMaxHealth(int amount)
{
    health += amount;
    currentHealth = Mathf.Min(currentHealth + amount, health);
    Debug.Log("Increased max health by " + amount + ". New max health: " + health + ", Current health: " + currentHealth);
    // Update health UI or other logic
}

    public void ResetHealth()
    {
        currentHealth = health;
        // Reset other health-related states
    }
}
