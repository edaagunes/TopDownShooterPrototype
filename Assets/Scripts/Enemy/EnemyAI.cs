using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{    
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;
    public float maxHealth=100;
    public GameObject newProjectile;

    [SerializeField]  HealthBarManager healthBar;
    [SerializeField] KillCounter killCounter;
    
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    [SerializeField]  Transform spawnPoint;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<HealthBarManager>();
    }

    private void Start()
    {
        SearchWalkPoint();
        healthBar.UpdateEnemyHealthBar(health,maxHealth);
        killCounter = GameObject.Find("KCO").GetComponent<KillCounter>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        else if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        else if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        else if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            SearchWalkPoint();
        }
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        //     walkPointSet = true;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(walkPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            walkPoint = hit.position; // Yürüme noktasını zemine sabitle
            walkPointSet = true; // Yürüme noktası belirlendiğini işaretle
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
             newProjectile = Instantiate(projectile, spawnPoint.transform.position, Quaternion.identity);
             Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 6f, ForceMode.Impulse);
            ///End of attack code
            
            Destroy(newProjectile,2f);
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
            
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);

        
        healthBar.UpdateEnemyHealthBar(health,maxHealth);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
        killCounter.AddKill();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

   

}


