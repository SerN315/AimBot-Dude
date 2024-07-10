using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    private int currentLevelCurrency;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCurrency(int value)
    {
        currentLevelCurrency += value;
    }

    public int GetCurrentLevelCurrency()
    {
        return currentLevelCurrency;
    }

    public void ResetCurrency()
    {
        currentLevelCurrency = 0;
    }
}
