using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}