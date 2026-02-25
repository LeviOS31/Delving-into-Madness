using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Enemy/New Attack")]
public class Attack : ScriptableObject
{
    public string attackName; // Name of the attack/name of animation
    public int damage; // Damage dealt by the attack
    public float minRange; // Range of the attack
    public float maxRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
