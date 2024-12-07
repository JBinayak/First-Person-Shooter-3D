using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    public Enemy enemy;
    // Start is called before the first frame update
    public void TakeDamage(float damage)
    {
        if (this.CompareTag("EnemyFace"))
        {
            enemy.TakeDamage(damage * 2);
        }
        if (this.CompareTag("EnemyBody") || this.CompareTag("Enemy"))
        {
            enemy.TakeDamage(damage);
        }
        if (this.CompareTag("EnemyLimbs"))
        {
            enemy.TakeDamage(damage * 0.75f);
        }
    }
}
