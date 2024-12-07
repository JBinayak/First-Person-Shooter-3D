using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 lastKnownPos;

    public NavMeshAgent Agent { get => agent; }

    public GameObject Player { get => player; }

    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }

    [Header("Weapon Values")]
    public bool isFrozen;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip shootSound;
    public AudioClip hitSound;

    [Header("Health Values")]
    public float maxHealth = 50;
    private float health;

    public EnemyPath path;
    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("Weapon Values")]
    public Transform gunBarrel;
    public Transform gunBarrel1;
    [Range(0.1f, 10f)]
    public float fireRate;
    public float attackRange;
    public bool isMelee;
    public bool isDualWeilder;
    // do melee person tomorrow
    [SerializeField]
    private string currentState;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        if (!isFrozen)
        {
            stateMachine = GetComponent<StateMachine>();
            agent = GetComponent<NavMeshAgent>();
            stateMachine.Initialise();
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen)
        {
            CanSeePlayer();
            currentState = stateMachine.activeState.ToString();
        }
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void TakeDamage(float damage)
    {
        source.PlayOneShot(hitSound);
        // set boolean and make enemy stare longer so they can target player using delta time with loop
        if (!isFrozen)
        {
            Invoke("lookAtPlayer", 0.25f);
        }
        health -= damage;
        GetComponent<EnemyHealth>().updateHealth(health);

        if (health < 0)
        {
            Destroy(gameObject);
        }
    }
    private void lookAtPlayer()
    {
        gameObject.transform.LookAt(player.transform);
    }
    public void gunSound()
    {
        source.PlayOneShot(shootSound);
    }
}
