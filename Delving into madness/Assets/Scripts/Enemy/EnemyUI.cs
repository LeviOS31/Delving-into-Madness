using UnityEngine;
using UnityEngine.Rendering;

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
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetHealth(float health, float maxHealth)
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        float healthPercentage = health / maxHealth;
        healthbar.localScale = new Vector3(MaxHPBarScale * healthPercentage, healthbar.localScale.y, healthbar.localScale.z);
        healthbar.localPosition = new Vector3((MaxHPBarScale * healthPercentage - MaxHPBarScale) / 2 * 10, healthbar.localPosition.y, healthbar.localPosition.z);
    }
}
