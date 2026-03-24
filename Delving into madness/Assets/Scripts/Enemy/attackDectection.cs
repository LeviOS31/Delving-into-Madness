using UnityEngine;

public class attackDectection : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage); //To be replaced by real playercontroller
        }
    }
}
