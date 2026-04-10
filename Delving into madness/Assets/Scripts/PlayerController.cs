using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.Cinemachine;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerModel;
    [SerializeField] CapsuleCollider playerHitbox;
    [SerializeField] UIManager uiManager;
    [SerializeField] GameObject BloodParticles;
    [SerializeField] CinemachineCamera cinemachineCamera;

    [Header("Player Stats")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private float dashPower = 25;
    [SerializeField] float health = 100f;
    private bool canDash = true;
    private float currentHealth;
    private bool isDead = false;


    private InputAction movementAction;
    private Rigidbody rigidBody;

    private bool disableMovement;

    private IWeapon weaponController;

    void Start()
    {
        movementAction = InputSystem.actions.FindAction("Move");
        rigidBody = GetComponent<Rigidbody>();
        weaponController = gameObject.GetComponentInChildren<IWeapon>();
        currentHealth = health;
    }

    void Update()
    {
        if (disableMovement) return;

        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 movementDirectionVector = movementAction.ReadValue<Vector2>();

        if (movementDirectionVector.magnitude > 0)
        {
            Vector3 normalizedMovement = new Vector3(movementDirectionVector.normalized.x, 0, movementDirectionVector.normalized.y);

            rigidBody.linearVelocity = normalizedMovement * movementSpeed;

            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, Quaternion.LookRotation(normalizedMovement), Time.deltaTime * turnSpeed);
        }
    }

    private void OnDash()
    {
        if (isDead) return;
        if (canDash) StartCoroutine(Dash());
    }

    private void OnAttack()
    {
        if (isDead) return;
        weaponController.LightAttack();
    }

    private void OnRestart()
    {
        SceneManager.LoadScene("Player_Movement_Scene");
    }

    private IEnumerator Dash()
    {
        canDash = false;
        disableMovement = true;
        rigidBody.linearVelocity = playerModel.transform.forward * dashPower;
        playerHitbox.enabled = false;
        yield return new WaitForSeconds(dashDuration);
        disableMovement = false;
        playerHitbox.enabled = true;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public async void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        uiManager.UpdateHealthBar(health, currentHealth);


        if (currentHealth <= 0)
        {
            isDead = true;
            disableMovement = true;

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            cinemachineCamera.enabled = false;

            while (transform.position.y > -10)
            {
                transform.Translate(Vector3.down * 3 * Time.deltaTime);

                await Task.Yield();
            }

            SceneManager.LoadScene("Main_Menu");
        }

        if (BloodParticles != null)
        {
            Quaternion rot = transform.rotation * Quaternion.Euler(0, 180, 0);
            GameObject instance = Instantiate(BloodParticles, transform.position, rot);
        }
    }
}
