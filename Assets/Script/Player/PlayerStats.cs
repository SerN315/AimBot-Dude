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
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Ensure the flashEffect is assigned
        if (flashEffect == null)
        {
            flashEffect = GetComponent<SimpleHit>();
            if (flashEffect == null)
            {
                Debug.LogError("FlashEffect is not assigned and SimpleHit component is not found on the player.");
            }
        }

        // Ensure the gunHand's flashEffect is assigned
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

    // Update is called once per frame
    void Update()
    {
        // Update logic if necessary
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player takes damage: " + damage); // Log damage taken

        // Call the flash effect when taking damage
        if (flashEffect != null)
        {
            flashEffect.Flash();
        }

        // Call the flash effect on gunHand when taking damage
        if (gunHandFlashEffect != null)
        {
            gunHandFlashEffect.Flash();
        }

        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
        if (gunHand != null)
        {
            gunHand.SetActive(false);
        }
            isDead = true;
            anim.SetBool("died", true);
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
        currentHealth = Mathf.Min(currentHealth + amount,health);
        // Update health UI or other logic
    }

    public void ResetHealth()
    {
        currentHealth = health;
        // Reset other health-related states
    }
}
