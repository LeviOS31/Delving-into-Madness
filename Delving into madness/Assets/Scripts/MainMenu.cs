using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Player_Movement_Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
