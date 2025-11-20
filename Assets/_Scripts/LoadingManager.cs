using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;
    
    
    [SerializeField] GameObject LoadingScreen;
    void Awake()
    {
        Instance = this;
    }

    public void Loading()
    {
        LoadingScreen.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        LoadingScreen.SetActive(false);
    }
}
