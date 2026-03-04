using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Enemy/New Attack")]
public class Attack : ScriptableObject
{
    public string attackName; // Name of the attack/name of animation
    public int damage; // Damage dealt by the attack
    public float minRange; //Min range of the attack
    public float maxRange; //Max range of the attack
    public float cooldown = 3; // Cooldown time for the attack in seconds
}
