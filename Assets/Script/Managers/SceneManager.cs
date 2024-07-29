using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadTitleScreen()
    {
        SceneManager.LoadScene(0); // Assuming GameTitle is at build index 0
    }

    public void LoadHomeScreen()
    {
        SceneManager.LoadScene(1); // Assuming Home is at build index 1
    }
        public void LoadLoadOut()
    {
        SceneManager.LoadScene(2); // Assuming Home is at build index 1
    }
    public void LoadMapScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 2;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene available in build hierarchy.");
        }
    }
}
