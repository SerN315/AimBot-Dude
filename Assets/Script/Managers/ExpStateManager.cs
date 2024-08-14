using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public int currentLevel = 1;
    public int currentExp = 0;
    public int maxExp = 200;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetData()
    {
        currentLevel = 1;
        currentExp = 0;
        maxExp = 200;
    }

    public void SaveData(int level, int exp, int maxExp)
    {
        this.currentLevel = level;
        this.currentExp = exp;
        this.maxExp = maxExp;
    }
}
    