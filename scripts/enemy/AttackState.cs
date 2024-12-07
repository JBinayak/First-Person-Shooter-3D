using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;

    private float shotTimer;
    public override void Enter()
    {
        moveTimer = 5;
        shotTimer = enemy.fireRate * 0.75f;
    }

    public override void Exit()
    {
        
    }


    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);
            if (moveTimer > Random.Range(2,4))
            { 
                if (enemy.isMelee)
                {
                    enemy.Agent.SetDestination(enemy.Player.transform.position - (Random.insideUnitSphere * 1f));
                }
                else
                {
                    enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                }
                moveTimer = 0;
            }
            enemy.LastKnownPos = enemy.Player.transform.position;
            if (shotTimer > enemy.fireRate)
            {
                if (enemy.isMelee)
                {
                    Slash();
                }
                else
                {
                    Shoot();
                }
            }
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public void Slash()
    {
        if (enemy.attackRange > Vector3.Distance(enemy.Player.transform.position, enemy.transform.position))
        {
            enemy.gunSound();
            Debug.Log("Slash");
            enemy.Player.GetComponent<PlayerHealth>().takeDamage(20);
            shotTimer = 0;
        }
    }

    public void Shoot()
    {
        enemy.gunSound();
        if (enemy.isDualWeilder)
        {
            Transform gunBarrel1 = enemy.gunBarrel1;
            GameObject bullet1 = GameObject.Instantiate(Resources.Load("prefabs/Revolver_Bullet") as GameObject, gunBarrel1.position, enemy.transform.rotation);
            Vector3 shootDirection1= (enemy.Player.transform.position - gunBarrel1.transform.position).normalized;
            bullet1.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection1 * 40;
        }
        Transform gunBarrel = enemy.gunBarrel;
        GameObject bullet = GameObject.Instantiate(Resources.Load("prefabs/Revolver_Bullet") as GameObject, gunBarrel.position, enemy.transform.rotation);
        Vector3 shootDirection = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f,3f), Vector3.up) * shootDirection * 40;
        Debug.Log("Shoot");
        shotTimer = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
