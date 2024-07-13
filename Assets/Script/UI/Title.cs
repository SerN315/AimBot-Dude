using UnityEngine;
using UnityEngine.UI;

public class TitleScreenButtonHandler : MonoBehaviour
{
    [SerializeField] Button PlayBt;
    // Assign the button click function in the Inspector
    void Start(){
        PlayBt.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {
        ScenesManager.Instance.LoadHomeScreen(); // Example: LoadHomeScreen method in GameManager
    }
}
