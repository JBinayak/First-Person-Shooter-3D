using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private float lerpTimer;
    private float maxHealth;
    private float enemyHealth;

    [Header("EnemyHealth")]
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    void Start()
    {
        maxHealth = GetComponent<Enemy>().maxHealth;
        enemyHealth = maxHealth;
    }

    void Update()
    {

        enemyHealth = Mathf.Clamp(enemyHealth, 0, maxHealth);
        updateHealthUI();
    }

    void updateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float hFraction = enemyHealth / maxHealth;
        if (fillF > hFraction)
        {
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
    }

    public void updateHealth(float health)
    {
        enemyHealth = health;
        lerpTimer = 0f;
    }
}
