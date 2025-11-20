using UnityEngine;
using UnityEngine.UI;

public class ErrorManager : MonoBehaviour
{
    public static ErrorManager Instance;
    public GameObject BG;
    public Text ErrorMsg;


    private bool ShowErr;
    private float Timer;
    float TimerMax = 2;
    void Awake()
    {
        Instance = this;
    }

    public void ShowErrorMsg(string msg)
    {
        ErrorMsg.text = msg;
        ShowErr = true;
    }

    void Update()
    {
        if (ShowErr)
        {
            BG.SetActive(true);
            Timer += Time.deltaTime;
            if (Timer > TimerMax)
            {
                ShowErr = false;
                Timer = 0;
            }
        }
        else
        {
            BG.SetActive(false);
        }
    }
}
