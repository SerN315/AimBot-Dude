using UnityEngine;

public class ExpManager : MonoBehaviour
{
    public static ExpManager instance;
    public delegate void ExpChangeHandler(int amount);
    public event ExpChangeHandler ExpOnChange;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    public void AddExp(int amount)
    {
        ExpOnChange?.Invoke(amount);
    }
}