using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    private int currentLevelCurrency;
    private int totalMoney;

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
        Debug.Log("Current Level Currency Added: " + value + ", Total: " + currentLevelCurrency);
    }

    public int GetCurrentLevelCurrency()
    {
        return currentLevelCurrency;
    }

    public void ResetCurrency()
    {
        currentLevelCurrency = 0;
        Debug.Log("Current Level Currency Reset");
    }

    public void SaveCurrencyOnWin()
    {
        Debug.Log("CurrencyManager SaveCurrencyOnWin called");
        totalMoney += currentLevelCurrency; // Accumulate total money
        Debug.Log("TotalMoney: " + totalMoney);

        PlayerPrefs.SetInt("TotalMoney", totalMoney);
        PlayerPrefs.Save();
        Debug.Log("TotalMoney in PlayerPrefs: " + PlayerPrefs.GetInt("TotalMoney", 0));

        ResetCurrency();
    }

    public int GetTotalMoney()
    {
        return totalMoney;
    }
}
