using System.Threading;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Enemy/New Attack")]
public class Attack : ScriptableObject
{
    public string attackName; // Name of the attack/name of animation
    public int damage; // Damage dealt by the attack
    public float minRange; //Min range of the attack
    public float maxRange; //Max range of the attack
    public bool isMelee; // Flag to determine if the attack is melee or ranged
    public float cooldown = 3; // Cooldown time for the attack in seconds

    public async void perform(GameObject target, float distanceToTarget, EnemyController attacker)
    {
        // Implement the logic to perform the attack on the target
        // This could involve playing an animation, applying damage, etc.
        Debug.Log("Attacking " + target.name + " with " + attackName + " from " + distanceToTarget);
        await Task.Delay(1000);
        attacker.state = State.Battle;
    }
}
