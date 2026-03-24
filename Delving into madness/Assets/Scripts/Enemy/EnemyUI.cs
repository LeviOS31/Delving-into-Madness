using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    public Transform healthbar;
    private float MaxHPBarScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MaxHPBarScale = healthbar.localScale.x;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);    
    }

    public void SetHealth(float health, float maxHealth)
    {
        float healthPercentage = health / maxHealth;
        healthbar.localScale = new Vector3(MaxHPBarScale * healthPercentage, healthbar.localScale.y, healthbar.localScale.z);
    }
}
