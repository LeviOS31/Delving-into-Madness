using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
    public bool CanAttack = true; // Flag to determine if the limb can attack (e.g., for cooldowns)
    public int Health; // Health of the limb, if it reaches 0 the limb is destroyed and removed from the monster
    //public AnimationClip attackAnim; // Reference to the animation
    public Attack attack; // List of attacks that the limb can perform
    public int cost; // Cost to use the limb in the monster creation phase, higher cost means a stronger limb 
    public int attachmentPointsArms;
    public int attachmentPointsLegs;
    public int attachmentPointsHeads;

    public float GetAttackScore(float playerDistance, bool LineOfSight)
    {
        float score = 10f;

        if (playerDistance >= attack.minRange && playerDistance <= attack.maxRange)
        {
            score += 10f; // Base score for being in range
        }
        else
        {
            float distanceError = Mathf.Abs(playerDistance - (attack.minRange + attack.maxRange) / 2f);
            score -= distanceError; // Penalize based on how far from the ideal range the player is
        }

        if (!attack.isMelee && !LineOfSight)
        {
            score -= 5f;
        }

        return score;
    }

    public void Attack(GameObject target, float distanceToTarget, EnemyController attacker)
    {
        attack.perform(target, distanceToTarget, attacker);
        CanAttack = false;
        float length = attacker.GetComponent<Animation>()[attack.ANIMNAME].length * 1000 + attack.cooldown * 1000;

        Task.Run(async () =>
        {
            await Task.Delay((int)length);
            CanAttack = true;
        });
        Debug.Log("waiting?");
    }
     
}
