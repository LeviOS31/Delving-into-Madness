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
        Vector3 bestpoint = this.transform.position;
        float score = -1000f;

        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f * Mathf.Deg2Rad;
            Vector3 dir =  new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Vector3 testposition = playerPos + dir * attackDistance;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(testposition, out hit, 2, NavMesh.AllAreas))
            {
                float distBetweenTargetAndPlayer = Vector3.Distance(playerPos, hit.position);
                float distBetweenTargetAndEnemy = Vector3.Distance(this.transform.position, hit.position);

                float newscore = distBetweenTargetAndPlayer - distBetweenTargetAndEnemy;
                if (newscore > score)
                {
                    score = newscore;
                    bestpoint = hit.position;
                }
            }
        }

        return bestpoint;
    }
}
