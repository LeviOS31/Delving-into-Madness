using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterCreator : MonoBehaviour
{
    public Transform spawnPoint;
    public int PointsToSpend;

    private int armCount = 0;
    private int legCount = 0;
    private int totalArms;
    private int totalLegs;
    private int totalHeads;
    private int totalTorsos;
    private float timer;
    void Start()
    {
        Application.targetFrameRate = 60;

        totalArms = Resources.LoadAll<GameObject>("Enemy/Limbs/Arm/").Length;
        totalLegs = Resources.LoadAll<GameObject>("Enemy/Limbs/Leg/").Length;
        totalHeads = Resources.LoadAll<GameObject>("Enemy/Limbs/Head/").Length;
        totalTorsos = Resources.LoadAll<GameObject>("Enemy/Limbs/Torso/").Length;

        armCount = Random.Range(2, 7);
        legCount = Random.Range(2, 7);

        CreateMonster(armCount, legCount);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 2)
        {
            GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");
            if (list.Length < 1)
            {
                Regenerate();
            }

            timer = 0;
        }
    }

    public void Regenerate()
    {
        armCount = Random.Range(2, 7);
        legCount = Random.Range(2, 7);

        CreateMonster(armCount, legCount);
    }


    private void CreateMonster(int AC, int LC)
    {
        int i = Random.Range(1, totalTorsos + 1);
        GameObject prefab = Resources.Load<GameObject>("Enemy/Limbs/Torso/Torso" + i);
        GameObject Monster = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        armCount = armCount > Monster.GetComponent<Limb>().attachmentPointsArms ? Monster.GetComponent<Limb>().attachmentPointsArms : armCount;
        legCount = legCount > Monster.GetComponent<Limb>().attachmentPointsLegs ? Monster.GetComponent<Limb>().attachmentPointsLegs : legCount;

        InstanceObjects(1, Monster, "Head"); // Add the head to the monster
        InstanceObjects(AC, Monster, "Arm");
        InstanceObjects(LC, Monster, "Leg"); 
        
        Monster.GetComponent<EnemyController>().FillStats(); // Update the enemy's stats after adding the limb
    }

    // Method to add limbs to the monster based on the specified count, parent object and limbtype, currently using just standard cylinders as placeholders for limbs
    // also checks if the limb slot is empty before adding
    // expand to also include limb group as paramerter to choose a specific arm or leg when they are added
    // Also for now its not used for the head
    private void InstanceObjects(int count, GameObject parent, string limbType)
    {
        for (int i = 0; i < count; i++)
        {
            int Index = 0;// Randomly select a leg type (1 to 6)

            int limbcount = 0;

            switch (limbType)
            {
                case "Head":
                    limbcount = totalHeads;
                    Index = Random.Range(1, 4);
                    break;
                case "Arm":
                    limbcount = totalArms;
                    Index = Random.Range(1, 7);
                    break;
                case "Leg":
                    limbcount = totalLegs;
                    Index = Random.Range(1, 7);
                    break;
            }

            int limbIndex = Random.Range(1, limbcount + 1); // Randomly select a limb type based on the number of available prefabs

            if (parent.transform.Find(limbType + Index).transform.childCount == 0) // Check if the limb slot is empty
            {
                GameObject obj = Instantiate(
                    Resources.Load<GameObject>("Enemy/Limbs/" + limbType + "/" + limbType + limbIndex), // Load the limb prefab from the Resources folder
                    parent.transform.Find(limbType + Index).position,
                    new quaternion(0,0,0,0),
                    parent.transform.Find(limbType + Index));

                obj.transform.localScale = new Vector3(
                    0.75f / 100.0f,
                    0.75f / 100.0f,
                    0.75f / 100.0f
                );

                obj.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }
}
