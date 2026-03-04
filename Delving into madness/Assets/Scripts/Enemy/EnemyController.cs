using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

enum State
{
    Idle,
    Wander,
    Chase,
    Attack
}

public class EnemyController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform for chasing and attacking
    int maxHealth = 0; // Maximum health of the enemy
    int currentHealth = 0; // Current health of the enemy
    List<Limb> limbs = new List<Limb>(); // Array of limbs that the enemy has
    int Speed = 0; // Speed of the enemy, calculated from the limbs
    bool IsDead = false; // Flag to determine if the enemy is dead
    public WanderController wandercontroller; // Reference to the Wandercontroller script for handling enemy movement
    State state;
    float WanderTimer;
    Vector3 wandertarget = Vector3.zero; // Target position for wandering

    void Start()
    {
        tag = "Enemy";
        state = State.Idle; // Set the initial state to Idle
        WanderTimer = Random.Range(2f, 5f); // Randomly set the wander timer between 2 and 5 seconds
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {    
            if (Vector3.Distance(player.position, this.transform.position) < 50f && (state != State.Chase || state != State.Attack))
            {
                state = State.Chase;
            }
        }

        WanderTimer -= Time.deltaTime; // Decrease the wander timer by the time elapsed since the last frame
        if (WanderTimer <= 0 && (state != State.Attack || state != State.Chase))
        {
            switch (state)
            {
                case State.Idle:
                    state = State.Wander; // Transition to the Wander state
                    WanderTimer = Random.Range(2f, 8f); // Reset the wander timer for the next transition
                    wandertarget = wandercontroller.FindNextWanderPoint(this.transform.position, 10f);
                    break;
                case State.Wander:
                    state = State.Idle; // Transition back to the Idle state
                    WanderTimer = Random.Range(1f, 3f); // Reset the wander timer for the next transition
                    break;
            }
        }
        else if(state == State.Wander)
        {
            if (Vector3.Distance(this.transform.position, wandertarget) < 0.1f)
            {
                WanderTimer = 0; // Reset the wander timer for the next transition
            }
            else
            {
                this.transform.LookAt(wandertarget); // Rotate the enemy to face the wander target
                this.transform.position = Vector3.MoveTowards(this.transform.position, wandertarget, Speed / 2 * Time.deltaTime); // Move the enemy forward based on its speed
            }
        }
        else if(state == State.Chase)
        {
            Vector3 Playerpos = new Vector3(player.position.x, 0, player.position.z); // Calculate the direction vector from the enemy to the player
            this.transform.LookAt(Playerpos); // Rotate the enemy to face the player
            this.transform.position = Vector3.MoveTowards(this.transform.position, Playerpos, Speed * Time.deltaTime);
            Debug.Log("Chasing player at position: " + Playerpos); // Move the enemy towards the player based on its speed
        }
    }

    public void FillStats()
    {
        limbs.Add(this.GetComponent<Limb>()); // Add the torso limb (the parent object) to the list of limbs

        foreach (Transform limbattachpoint in transform)
        {
            if (limbattachpoint.childCount == 1 && limbattachpoint.GetChild(0).GetComponent<Limb>() != null)
            {
                Limb limb = limbattachpoint.GetChild(0).GetComponent<Limb>();
                limbs.Add(limb); // Add the new limb to the list
            }
        }
        maxHealth = 0; // Reset max health before calculating
        Speed = 0; // Reset speed before calculating

        foreach (Limb limb in limbs)
        {
            maxHealth += limb.Health; // Add the health of each limb to the max health
            Speed += limb.Speed; // Add the speed of each limb to the total speed
        }

        Speed /= limbs.Count; // devide speed by the number of limbs to get the actual speed
        Speed /= 5;

        currentHealth = maxHealth; // Set current health to max health at the start
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
