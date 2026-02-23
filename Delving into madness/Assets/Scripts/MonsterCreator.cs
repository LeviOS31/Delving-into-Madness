using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterCreator : MonoBehaviour
{
    public GameObject monsterStart; // Reference to the monster prefab
    public GameObject prefab; // Reference to the prefab to instantiate
    private int headcount = 0; // Counter for the number of heads created
    private int armcount = 0; // Counter for the number of arms created
    private int legcount = 0; // Counter for the number of legs created
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        headcount = Random.Range(1, 4); // Randomly determine the number of heads (1 to 3)
        armcount = Random.Range(2, 7); // Randomly determine the number of arms (2 to 6)
        legcount = Random.Range(2, 7); // Randomly determine the number of legs (2 to 6)

        if (headcount == 1)
        {
            int headIndex = Random.Range(1, 4); // Randomly select a head type (1 to 3)
            GameObject obj = Instantiate(
                prefab,
                monsterStart.transform.Find("Head" + headIndex).position,
                new quaternion(0,0,0,0),
                monsterStart.transform.Find("Head" + headIndex));

            obj.transform.localScale = new Vector3(
                0.75f / 100.0f,
                0.75f / 100.0f,
                0.75f / 100.0f
            );

            obj.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
        else
        {
            GameObject obj1 = Instantiate(
                prefab,
                monsterStart.transform.Find("Head2").position,
                new quaternion(0,0,0,0),
                monsterStart.transform.Find("Head2"));

            obj1.transform.localScale = new Vector3(
                0.75f / 100.0f,
                0.75f / 100.0f,
                0.75f / 100.0f
            );
            GameObject obj2 = Instantiate(
                prefab,
                monsterStart.transform.Find("Head3").position,
                new quaternion(0,0,0,0),
                monsterStart.transform.Find("Head3"));

            obj2.transform.localScale = new Vector3(
                0.75f / 100.0f,
                0.75f / 100.0f,
                0.75f / 100.0f
            );

            obj1.transform.localRotation = Quaternion.Euler(90, 0, 0);
            obj2.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }

        for (int i = 0; i < armcount; i++)
        {
            int armIndex = Random.Range(1, 7); // Randomly select an arm type (1 to 6)
            if (monsterStart.transform.Find("Arm" + armIndex).transform.childCount == 0) // Check if the arm slot is empty
            {
                GameObject objarm = Instantiate(
                    prefab,
                    monsterStart.transform.Find("Arm" + armIndex).position,
                    new quaternion(0,0,0,0),
                    monsterStart.transform.Find("Arm" + armIndex));
                    
                objarm.transform.localScale = new Vector3(
                    0.75f / 100.0f,
                    0.75f / 100.0f,
                    0.75f / 100.0f
                );

                objarm.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
        }

        for (int i = 0; i < legcount; i++)
        {
            int legIndex = Random.Range(1, 7); // Randomly select a leg type (1 to 6)
            if (monsterStart.transform.Find("Leg" + legIndex).transform.childCount == 0) // Check if the leg slot is empty
            {
                GameObject objleg = Instantiate(
                    prefab,
                    monsterStart.transform.Find("Leg" + legIndex).position,
                    new quaternion(0,0,0,0),
                    monsterStart.transform.Find("Leg" + legIndex));

                objleg.transform.localScale = new Vector3(
                    0.75f / 100.0f,
                    0.75f / 100.0f,
                    0.75f / 100.0f
                );
                
                objleg.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }
}
