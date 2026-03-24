using System.Threading;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Enemy/New Attack")]
public class Attack : ScriptableObject
{
    public string attackName; // Name of the attack/name of animation
    public string ANIMNAME;// used for now to play animation later to be handeled by inverse kinematics
    public int damage; // Damage dealt by the attack
    public float minRange; //Min range of the attack
    public float maxRange; //Max range of the attack
    public bool isMelee; // Flag to determine if the attack is melee or ranged
    public float cooldown = 3; // Cooldown time for the attack in seconds

    public async void perform(GameObject target, float distanceToTarget, EnemyController attacker)
    {
        Debug.Log("Attacking " + target.name + " with " + attackName + " from " + distanceToTarget);

        attacker.GetComponentInChildren<attackDectection>(true).damage = damage;

        attacker.GetComponent<Animation>().Play(ANIMNAME);
        float length = attacker.GetComponent<Animation>()[ANIMNAME].length * 1000;

        await Task.Delay((int)length);
        attacker.state = State.Battle;
    }
}
