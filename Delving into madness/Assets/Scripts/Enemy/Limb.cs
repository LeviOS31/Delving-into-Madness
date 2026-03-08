using System;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    public enum LimbType
    {
        Head,
        Arm,
        Leg,
        Torso
    }

    public LimbType limbType; // Type of the limb (head, arm, or leg)
    public int Speed; // Speed to calculate the monsters movement speed
    public bool CanAttack; // Flag to determine if the limb can attack (e.g., for cooldowns)
    public int Health; // Health of the limb, if it reaches 0 the limb is destroyed and removed from the monster
    public Animator animator; // Reference to the Animator component for handling animations
    public Attack attack; // List of attacks that the limb can perform
    public int cost; // Cost to use the limb in the monster creation phase, higher cost means a stronger limb 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
