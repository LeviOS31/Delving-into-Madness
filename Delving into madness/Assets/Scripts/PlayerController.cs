using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerModel;
    [SerializeField] CapsuleCollider playerHitbox;

    [Header("Player Stats")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private float dashPower = 25;
    private bool canDash = true;


    private InputAction movementAction;
    private Rigidbody rigidBody;

    private bool disableMovement;

    void Start()
    {
        movementAction = InputSystem.actions.FindAction("Move");
        rigidBody = GetComponent<Rigidbody>();
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
        if (canDash) StartCoroutine(Dash());
    }

    private void OnAttack()
    {
        weaponController.Attack();
        TakeDamage(5);
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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        uiManager.UpdateHealthBar(health, currentHealth);
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("Main_Menu");
        }
    }
}
