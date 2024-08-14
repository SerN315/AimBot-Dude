using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private SimpleHit flashEffect;
    public int health = 100;

    private Rigidbody2D rb;
    private Animator anim;
    private GameManager gameManager;
    public GameObject gunHand;
    private SimpleHit gunHandFlashEffect;
    [SerializeField] private Text exptext;
    [SerializeField] private Sprite Csprite;
    [SerializeField] private Image expprogress;
    [SerializeField] private Image Cavatar;
    [SerializeField] private float deathDelay = 1f;
    private bool isDead = false;
    private int currentHealth;
    public int currentexp;
    public int currentlevel;
    public int maxExp;
    public PowerUpManager powerUpManager;

    void Start()
    {
        Csprite = GetComponent<SpriteRenderer>().sprite;
        exptext = GameObject.Find("exptext")?.GetComponent<Text>();
        expprogress = GameObject.Find("Exp")?.GetComponent<Image>();
        Cavatar = GameObject.Find("CharacterAva")?.GetComponent<Image>();
        currentHealth = health;
        currentlevel = GameData.instance.currentLevel;
        currentexp = GameData.instance.currentExp;
        maxExp = GameData.instance.maxExp;
        powerUpManager = FindObjectOfType<PowerUpManager>();
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (flashEffect == null)
        {
            flashEffect = GetComponent<SimpleHit>();
        }

        if (gunHand != null)
        {
            gunHandFlashEffect = gunHand.GetComponent<SimpleHit>();
        }

        UpdateExpUI(); 
    }

    void Update()
    {
        // Update logic 
    }

    public void TakeDamage(int damage)
    {
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
        currentHealth = Mathf.Min(currentHealth + amount, health);
    }

    public void IncreaseMaxHealth(int amount)
    {
        health += amount;
        currentHealth = Mathf.Min(currentHealth + amount, health);
    }

    public void ResetHealth()
    {
        currentHealth = health;
    }

    private void OnEnable()
    {
        if (ExpManager.instance != null)
        {
            ExpManager.instance.ExpOnChange += HandleExpChange;
        }
    }

    private void OnDisable()
    {
        if (ExpManager.instance != null)
        {
            ExpManager.instance.ExpOnChange -= HandleExpChange;
        }
    }

    private void HandleExpChange(int newExp)
    {
        Debug.Log("Exp changed by: " + newExp);
        currentexp += newExp;
        if (currentexp >= maxExp)
        {
            LevelUp();
        }
        else
        {
            UpdateExpUI(); // Update UI after exp change
        }
    }

    private void LevelUp()
    {
        powerUpManager.ShowPowerUpUI();
        currentlevel++;
        currentexp = 0;
        maxExp += 100;
        GameData.instance.SaveData(currentlevel, currentexp, maxExp);
        UpdateExpUI(); // Update UI after level up
    }

    private void UpdateExpUI()
    {
        // Update the level text
        exptext.text = "Lv: " + currentlevel;

        // Update the experience progress bar
        StartCoroutine(UpdateExpProgressBar());

        Cavatar.sprite = Csprite; 
    }
    private IEnumerator UpdateExpProgressBar()
    {
        float elapsedTime = 0f;
        float duration = 0.5f; // Duration of the fill effect
        float startFill = expprogress.fillAmount;
        float targetFill = (float)currentexp / maxExp;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            expprogress.fillAmount = Mathf.Lerp(startFill, targetFill, elapsedTime / duration);
            yield return null;
        }

        // Ensure the fill amount reaches the target value
        expprogress.fillAmount = targetFill;
    }
}
