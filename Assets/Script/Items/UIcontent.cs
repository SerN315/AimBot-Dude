using UnityEngine;
using UnityEngine.UI;

public class DisplayTotalMoney : MonoBehaviour
{
    public Text totalMoneyText;

    void Start()
    {
        UpdateTotalMoneyDisplay();
    }

    void UpdateTotalMoneyDisplay()
    {
        int totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
        totalMoneyText.text = "Total Money: " + totalMoney.ToString();
        Debug.Log("Total Money Displayed: " + totalMoney); // Debug statement
    }
}