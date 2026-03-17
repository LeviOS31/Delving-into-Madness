using UnityEngine;

public class FakePlayerController : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        Debug.Log("Player takes " + damage + " damage.");
    }
}
