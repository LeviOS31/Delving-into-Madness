using System.Threading.Tasks;
using UnityEngine;

public class MonsterStart : MonoBehaviour
{
    public Animator monster1;
    public Animator monster2;
    public Animator monster3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Task Start()
    {
        await Task.Delay(2000);
        monster1.SetTrigger("Disolve");
        monster2.SetTrigger("Disolve");
        monster3.SetTrigger("Disolve");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
