using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float critChance;
    [SerializeField] private float critDamage;
    [SerializeField] Animator animator;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Get enemy controller
        }
    }
}
