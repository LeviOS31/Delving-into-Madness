using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAndShield : MonoBehaviour, IWeapon, DamageModification
{
    [SerializeField] BoxCollider hitbox;

    [Header("Weapon Stats")]
    [SerializeField] float ShieldProtectionAngle = 45f;
    [SerializeField] float ShieldDamageReduction = 50;
    [SerializeField] int MaxComboCount = 3;
    [SerializeField] float ComboCooldown = 0.5f;

    private InputAction heavyAttack;
    private Animator animator;
    private bool isShielded;
    private bool canAttack = true;
    private int currentComboHit = 0;

    private enum Attacks
    {
        light,
        heavy
    }

    private List<Attacks> AttackQueue = new List<Attacks>();

    private void Start()
    {
        heavyAttack = InputSystem.actions.FindAction("HeavyAttack");
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        isShielded = heavyAttack.ReadValue<float>() > 0;
    }

    public void LightAttack()
    {
        if (AttackQueue.Count >= MaxComboCount || !canAttack) return;

        AttackQueue.Add(Attacks.light);
        animator.SetInteger("ComboCount", AttackQueue.Count);
    }
    public void HeavyAttack()
    {
        // Heavy Attack Replaced with shield raise
        return;
    }

    public void DashAttack()
    {
        // Light Attack
        throw new System.NotImplementedException();
    }
 
    public void SpecialAttack()
    {
        throw new System.NotImplementedException();
    }

    public float CalculateDamage(float baseValue, Vector3 hitOrigin)
    {
        Vector3 hitDirection = hitOrigin - gameObject.transform.position;

        if (Vector3.Angle(Vector3.forward, hitDirection) > ShieldProtectionAngle) 
        {
            float damageMultiplier = (100 - ShieldDamageReduction) / 100;

            baseValue = baseValue * damageMultiplier;
            return baseValue;
        }

        return baseValue;
    }

    public void AnimationFinish(int animationNumber)
    {
        if (animationNumber == AttackQueue.Count) StartCoroutine(CanAttack(animationNumber));
    }

    public void ToggleHitbox(int isHitboxActive)
    {
        if (isHitboxActive > 0)
        {
            hitbox.enabled = true;
        }
        else
        {
            hitbox.enabled = false;
        }
    }

    private IEnumerator CanAttack(int trigger)
    {
        Debug.Log("test " + trigger);
        AttackQueue.Clear();
        animator.SetInteger("ComboCount", 0);
        canAttack = false;
        yield return new WaitForSeconds(ComboCooldown);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Attack Enemy
    }
}
