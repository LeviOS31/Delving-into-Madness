using UnityEngine;

public class WanderController : MonoBehaviour
{
    public float wanderRadius = 5f; // Radius within which the enemy will wander

    public Vector3 FindNextWanderPoint(Vector3 currentPosition)
    {
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius; // Generate a random point within the wander radius
        return new Vector3(currentPosition.x + randomPoint.x, currentPosition.y, currentPosition.z + randomPoint.y); // Return the new wander point based on the current position
    }
}
