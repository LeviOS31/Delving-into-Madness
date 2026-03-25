using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using TMPro;
using System.Collections;
using static UnityEngine.EventSystems.EventTrigger;
using System.Threading.Tasks;

public enum State
{
    Idle,
    Wander,
    Battle,
    Attacking,
    Staggered
}

public class EnemyController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform for chasing and attacking
    public State state;
    public GameObject BloodParticles;

    Navigator navigator;
    LimbController limbcontroller;
    EnemyUI UI;

    int maxHealth = 0; // Maximum health of the enemy
    int currentHealth = 0; // Current health of the enemy
    int Speed = 0; // Speed of the enemy, calculated from the limbs
    bool IsDead = false; // Flag to determine if the enemy is dead
    float WanderTimer;
    float StaggerTimer;
    Vector3 wandertarget = Vector3.zero; // Target position for wandering
    float detectionRange = 50f; // Range within which the enemy can detect the player
    NavMeshAgent agent; // Reference to the NavMeshAgent component for pathfinding

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); 
        navigator = GetComponent<Navigator>(); 
        limbcontroller = GetComponent<LimbController>();
        UI = GetComponentInChildren<EnemyUI>();

        tag = "Enemy";
        state = State.Idle;
        WanderTimer = Random.Range(2f, 5f);
    }

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // Automatically find the player by tag if not assigned
        }
    }

    void Update()
    {
        if (IsDead || state == State.Staggered)
        {
            return;
        }

        if (player != null && state != State.Attacking)
        {
            float distance = Vector3.Distance(player.position, this.transform.position);
            if (distance <= detectionRange && state != State.Battle)
            {
                state = State.Battle;
                agent.SetDestination(this.transform.position);
                agent.speed = Speed;
            }
        }

        if (state == State.Attacking)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        if (state == State.Wander || state == State.Idle)
        {
            WanderTimer -= Time.deltaTime;
            if (WanderTimer <= 0 || (agent.remainingDistance < 0.5f && state == State.Wander))
            {
                switch (state)
                {
                    case State.Idle:
                        state = State.Wander;
                        WanderTimer = Random.Range(2f, 5f);
                        agent.speed = Speed / 2;
                        agent.SetDestination(navigator.FindNextWanderPoint(10f)); 
                        break;
                    case State.Wander:
                        state = State.Idle;
                        agent.SetDestination(this.transform.position);
                        WanderTimer = Random.Range(1f, 3f);
                        break;
                }
            }
        }
        else if (state == State.Battle)
        {
            //TODO: check if enemy can attack
            float currentDistToPlayer = Vector3.Distance(transform.position, player.position);

            Limb bestAttackLimb = limbcontroller.getBestAttack(currentDistToPlayer);

            //Debug.Log("Current distance to player: " + currentDistToPlayer);

            float perfectdistance = (bestAttackLimb.attack.minRange + bestAttackLimb.attack.maxRange) / 2f;

            if (currentDistToPlayer >= bestAttackLimb.attack.minRange && currentDistToPlayer <= bestAttackLimb.attack.maxRange && bestAttackLimb.CanAttack)
            {
                agent.SetDestination(this.transform.position);
                agent.isStopped = true;

                Vector3 lookatpos = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.LookAt(lookatpos);

                bestAttackLimb.Attack(player.gameObject, currentDistToPlayer, this);
                state = State.Attacking;
            }
            if (currentDistToPlayer > perfectdistance + 2 || currentDistToPlayer < perfectdistance - 2)
            {
                agent.SetDestination(navigator.FindAttackPosition(player.position, perfectdistance));
            }
        }
    }

    public void FillStats()
    {
        limbcontroller.limbs.Add(this.GetComponent<Limb>());

        foreach (Transform limbattachpoint in transform)
        {
            if (limbattachpoint.childCount == 1 && limbattachpoint.GetChild(0).GetComponent<Limb>() != null)
            {
                Limb limb = limbattachpoint.GetChild(0).GetComponent<Limb>();
                limbcontroller.limbs.Add(limb);
            }
        }
        maxHealth = 0;
        Speed = 0; 

        foreach (Limb limb in limbcontroller.limbs)
        {
            maxHealth += limb.Health;
            Speed += limb.Speed;

            if (limb.attack != null)
            {
                if (limb.attack.maxRange > detectionRange)
                {
                    detectionRange = limb.attack.maxRange;
                }
            }
        }

        Speed /= limbcontroller.limbs.Count;
        Speed /= 5;

        maxHealth = maxHealth / 4;

        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= (int)Math.Round(damage); // Reduce current health by the damage taken

        UI.SetHealth(currentHealth, maxHealth);

        StartCoroutine(StaggerAndKnockback(new Vector3(transform.position.x - player.position.x, 0, transform.position.z - player.position.z), damage, 2f));

        if (BloodParticles != null)
        {
            Quaternion rot = transform.rotation * Quaternion.Euler(0, 180, 0);
            GameObject instance = Instantiate(BloodParticles, transform.position, rot);
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }
    }

    private async void Die()
    {
        IsDead = true;
        Debug.Log("Enemy died!");
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Collider>().enabled = false;

        float bottomY = transform.position.y - 10;

        while (transform.position.y > bottomY)
        {
            transform.Translate(Vector3.down * 3 * Time.deltaTime);

            await Task.Yield();
        }

        Destroy(gameObject);
    }

    private IEnumerator StaggerAndKnockback(Vector3 knockbackDirection, float knockbackForce, float staggerDuration)
    {
        state = State.Staggered;
        StaggerTimer = staggerDuration;

        // Apply knockback force
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (rb != null)
        {
            rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(staggerDuration);

        state = State.Battle; // Return to idle state after staggering
    }
}
