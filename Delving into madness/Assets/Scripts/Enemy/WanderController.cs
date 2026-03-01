using UnityEngine;
using UnityEngine.AI;

public class WanderController : MonoBehaviour
{

    public Vector3 FindNextWanderPoint(Vector3 currentPosition, float wanderRadius = 5f)
    {
        for(int i = 0; i < 10; i++) // Try to find a valid wander point up to 10 times
        {
            Vector2 randomPoint = Random.insideUnitCircle * wanderRadius; // Generate a random point within the wander radius
            Vector3 randomPoint3D = new Vector3(randomPoint.x, -2.5f, randomPoint.y) + currentPosition; // Convert the 2D random point to 3D and offset it by the current position

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint3D, out hit, wanderRadius, NavMesh.AllAreas)) // Check if the random point is on the NavMesh
            {
                Vector3 point = hit.position;
                point.y = 0; // Set the y-coordinate to a fixed value to keep the enemy on the ground
                return point; // Return the valid wander point
            }
        }

        return currentPosition; // If no valid point is found, return the current position

    }
}
