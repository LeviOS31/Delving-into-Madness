using UnityEngine;

public interface DamageModification
{
    public float CalculateDamage(float baseValue, Vector3 hitOrigin);
}
