using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    int maxHealth = 0; // Maximum health of the enemy
    int currentHealth = 0; // Current health of the enemy
    List<Limb> limbs; // Array of limbs that the enemy has
    int Speed = 0; // Speed of the enemy, calculated from the limbs
    bool IsDead = false; // Flag to determine if the enemy is dead

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillStats()
    {
        foreach (GameObject limbattachpoint in transform)
        {
            if (limbattachpoint.transform.childCount == 1 && limbattachpoint.transform.GetChild(0).GetComponent<Limb>() != null)
            {
                Limb limb = limbattachpoint.transform.GetChild(0).GetComponent<Limb>();
                limbs.Add(limb); // Add the new limb to the list
            }
        }
        maxHealth = 0; // Reset max health before calculating
        Speed = 0; // Reset speed before calculating

        foreach (Limb limb in limbs)
        {
            maxHealth += limb.Health; // Add the health of each limb to the max health
            Speed += limb.Speed / limbs.Count; // Add the speed of each limb to the total speed and divide by the number of limbs to get the average speed
        }

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
