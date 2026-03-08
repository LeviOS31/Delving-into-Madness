using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public enum State
{
    Idle,
    Wander,
    Battle,
}

public class EnemyController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform for chasing and attacking
    public State state;

    Navigator navigator;
    LimbController limbcontroller;

    int maxHealth = 0; // Maximum health of the enemy
    int currentHealth = 0; // Current health of the enemy
    int Speed = 0; // Speed of the enemy, calculated from the limbs
    bool IsDead = false; // Flag to determine if the enemy is dead
    float WanderTimer;
    Vector3 wandertarget = Vector3.zero; // Target position for wandering
    float detectionRange = 50f; // Range within which the enemy can detect the player
    NavMeshAgent agent; // Reference to the NavMeshAgent component for pathfinding

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
        navigator = GetComponent<Navigator>(); 
        limbcontroller = GetComponent<LimbController>();

        tag = "Enemy";
        state = State.Idle;
        WanderTimer = Random.Range(2f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        {
            return; // If the enemy is dead, exit the update loop
        }

        if (player != null)
        {
            float distance = Vector3.Distance(player.position, this.transform.position);
            if (distance <= detectionRange && state != State.Battle)
            {
                state = State.Battle;
            }
        }

        if (state == State.Wander || state == State.Idle)
        {
            WanderTimer -= Time.deltaTime;
            if (WanderTimer <= 0)
            {
                switch (state)
                {
                    case State.Idle:
                        state = State.Wander;
                        WanderTimer = Random.Range(2f, 8f);
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
            else if (agent.remainingDistance < 0.1f)
            {
                WanderTimer = 0;
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

        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by the damage taken
        if (currentHealth <= 0)
        {
            //Die(); // If health drops to 0 or below, the enemy dies
        }
    }


}
