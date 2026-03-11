using System.Collections.Generic;
using UnityEngine;

public class LimbController : MonoBehaviour
{
    public List<Limb> limbs = new List<Limb>(); // List of limbs that the enemy has
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Limb getBestAttack(float playerDistance, bool LineOfSight = true)
    {
        Limb bestLimb = null;
        float bestScore = -float.MaxValue;

        foreach (Limb limb in limbs)
        {
            if (limb.attack == null)
            {
                continue; // Skip limbs that cannot attack
            }
            float score = limb.GetAttackScore(playerDistance, LineOfSight);
            if (score > bestScore)
            {
                bestScore = score;
                bestLimb = limb;
            }
        }

        return bestLimb;
    }
}
