using UnityEngine;
using UnityEngine.UI;

public class HomeScreenButtonHandler : MonoBehaviour
{
    [SerializeField] Button PlayBt;
    [SerializeField] Button TitleBt;

    // Assign the button click function in the Inspector
    void Start(){
        PlayBt.onClick.AddListener(OnPlayClick);
        TitleBt.onClick.AddListener(OnTitleClick);
    }
    public void OnPlayClick()
    {
        ScenesManager.Instance.LoadMapScene(); // Example: LoadHomeScreen method in GameManager
        Time.timeScale = 1f;

    }
        public void OnTitleClick()
    {
        ScenesManager.Instance.LoadTitleScreen(); // Example: LoadHomeScreen method in GameManager
    }
}
