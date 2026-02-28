using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerModel;

    [Header("Player Stats")]
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float turnSpeed = 40f;

    private InputAction movementAction;
    private Rigidbody rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementAction = InputSystem.actions.FindAction("Move");
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
