using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPositionEmulator : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) 
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    this.transform.position = hit.point;
                    Debug.Log("Player position set to: " + hit.point);

                    EnemyController[] enemies = Object.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.player = this.transform;
                    }
                }
            }
        }
    }
}