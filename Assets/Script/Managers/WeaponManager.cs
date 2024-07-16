using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public int SelectedGunIndex { get; private set; }

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }

        // Load selected gun index from PlayerPrefs on startup
        SelectedGunIndex = PlayerPrefs.GetInt("SelectedGunIndex", 0);
    }

    public void SetSelectedGunIndex(int index)
    {
        SelectedGunIndex = index;
        PlayerPrefs.SetInt("SelectedGunIndex", index);
        PlayerPrefs.Save();
    }
}
