using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour
{
    public Vector3 FindNextWanderPoint(float wanderRadius = 5f)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
            Vector3 randomPoint3D = new Vector3(randomPoint.x, this.transform.position.y, randomPoint.y) + this.transform.position;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint3D, out hit, wanderRadius, NavMesh.AllAreas))
            {
                Vector3 point = hit.position;
                return point; 
            }
        }

        return this.transform.position; // If no valid point is found, return the current position
    }

    public Vector3 FindAttackPosition(Vector3 playerPos, float attackDistance)
    {
    // Start with current position as fallback
    Vector3 bestPoint = transform.position;
    
    float bestScore = -float.MaxValue;

    float distanceToPlayer = Vector3.Distance(transform.position, playerPos);

    float searchRadius = Mathf.Clamp(Mathf.Abs(distanceToPlayer - attackDistance), 0.5f, 10f); 

    for (int i = 0; i < 8; i++)
    {
        float angle = i * 45f * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        
        Vector3 testPosition = transform.position + dir * searchRadius;

        if (NavMesh.SamplePosition(testPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            float distFromSpotToPlayer = Vector3.Distance(hit.position, playerPos);
            

            float distanceError = Mathf.Abs(distFromSpotToPlayer - attackDistance);

            float score = -distanceError - (Vector3.Distance(transform.position, hit.position) * 0.2f);

            if (score > bestScore)
            {
                bestScore = score;
                bestPoint = hit.position;
            }
        }
    }

    return bestPoint;
    }
}
