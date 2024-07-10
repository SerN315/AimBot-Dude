using UnityEngine;
using TMPro;

public class DisplayTotalMoney : MonoBehaviour
{
    public TMP_Text totalMoneyText;

    private void Start()
    {
        int totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
        totalMoneyText.text = "Total Money: " + totalMoney.ToString();
    }
}
