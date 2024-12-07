using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private bool isDead;
    public GameManagerScript gameManager;
    [Header("Player Health")]
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("Damage Overlay")]
    public Image damageOverlay;
    public float damageDuration;
    public float damageFadeSpeed;

    [Header("Heal Overlay")]
    public Image healOverlay;
    public float healDuration;
    public float healFadeSpeed;
    public Button healUI;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip healSound;
    public AudioClip hitSound;

    private float damageDurationTimer;
    private float healDurationTimer;

    private float healCooldown = 10f;
    private float healTimer;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        health = maxHealth;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
        healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, 0);
        healTimer = healCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (health != 100)
            {
                healUI.GetComponent<SpellCooldown>().UseSpell();
            }
            restoreHealth(20);
        }
        health = Mathf.Clamp(health, 0, maxHealth);
        updateHealthUI();
        if (damageOverlay.color.a > 0)
        {
            if (!(health < 30))
            {
                damageDurationTimer += Time.deltaTime;
                if (damageDurationTimer > damageDuration)
                {
                    float tempDamageAlpha = damageOverlay.color.a;
                    tempDamageAlpha -= Time.deltaTime * damageFadeSpeed;
                    damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, tempDamageAlpha);
                }
            }
        }
        if (healOverlay.color.a > 0)
        {
            healDurationTimer += Time.deltaTime;
            if (healDurationTimer > healDuration)
            {
                float tempHealAlpha = damageOverlay.color.a;
                tempHealAlpha -= Time.deltaTime * healFadeSpeed;
                healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, tempHealAlpha);
            }
        }
        healTimer += Time.deltaTime;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Time.timeScale = 0;
            gameManager.gameOver();
        }
    }

    void updateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void takeDamage(float damage)
    {
        source.PlayOneShot(hitSound);
        damageDurationTimer = 0;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0.4f);
        health -= damage;
        lerpTimer = 0f;
    }

    public void restoreHealth(float healAmount)
    {
        if (healTimer > healCooldown)
        {
            healTimer = 0;
            healDurationTimer = 0;
            if (health < 100)
            {
                healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, 1);
            }
            if (health != 100)
            {
                source.PlayOneShot(healSound);
            }
            health += healAmount;
            lerpTimer = 0f;
        }
    }
}
