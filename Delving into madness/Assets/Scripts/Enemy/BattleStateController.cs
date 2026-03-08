using UnityEngine;

public class BattleStateController : MonoBehaviour
{
    public EnemyController enemyController; // Reference to the EnemyController script for accessing enemy properties and methods

    // Update is called once per frame
    void Update()
    {
        if (enemyController.state != State.Battle) return;


    }

    public void MoveToRange()
    {
        
    }
}
