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
        headcount = Random.Range(1, 3); // Randomly determine the number of heads (1 to 2)
        armcount = Random.Range(2, 7); // Randomly determine the number of arms (2 to 6)
        legcount = Random.Range(2, 7); // Randomly determine the number of legs (2 to 6)

        CreateMonster(headcount, armcount, legcount); // Call the method to create the monster
    }

    public void Regenerate()
    {
        headcount = Random.Range(1, 3); // Randomly determine the number of heads (1 to 2)
        armcount = Random.Range(2, 7); // Randomly determine the number of arms (2 to 6)
        legcount = Random.Range(2, 7); // Randomly determine the number of legs (2 to 6)

        DeleteMonster();

        CreateMonster(headcount, armcount, legcount);
    }

    private void DeleteMonster()
    {
        // Destroy existing monster parts
        foreach(Transform child in monsterStart.transform)
        {
            foreach(Transform grandChild in child)
            {
                Destroy(grandChild.gameObject); // Destroy each part of the monster
            }
        }
    }

    private void CreateMonster(int HC, int AC, int LC)
    {
        if (HC == 1)
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

        InstanceObjects(AC, monsterStart, "Arm");
        InstanceObjects(LC, monsterStart, "Leg"); 
        
    }

    // Method to add limbs to the monster based on the specified count, parent object and limbtype, currently using just standard cylinders as placeholders for limbs
    // also checks if the limb slot is empty before adding
    // expand to also include limb group as paramerter to choose a specific arm or leg when they are added
    // Also for now its not used for the head
    private void InstanceObjects(int count, GameObject parent, string limbType)
    {
        for (int i = 0; i < count; i++)
        {
            int Index = Random.Range(1, 7); // Randomly select a leg type (1 to 6)

            if (monsterStart.transform.Find(limbType + Index).transform.childCount == 0) // Check if the limb slot is empty
            {
                GameObject objleg = Instantiate(
                    prefab,
                    monsterStart.transform.Find(limbType + Index).position,
                    new quaternion(0,0,0,0),
                    monsterStart.transform.Find(limbType + Index));

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
