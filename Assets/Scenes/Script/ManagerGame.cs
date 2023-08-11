using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.RemoteConfig; //Удаленное получени данных
using GameAnalyticsSDK;
using AppsFlyerSDK;
using MoreMountains.NiceVibrations;

public class ManagerGame : MonoBehaviour
{   
    public GameObject[] Level;
    public GameObject UISettings;
    public GameObject LoadingUI;    
    private static  GameObject _LoadingUI;
    private static GameObject _UISettings;
    private static GameObject[] StaticLevel;
    private static  GameObject ActiveLevel;
    public static  bool isStopTimer;
    public static bool isPauseTimer;
    public static  float time;
    public static  float timeMetrica;
    private static bool isLoad;
    public static bool isGameOver;
    public static bool isStartGame;
    public static bool isBlockMove;                            //Блокировать действие игрока


    /// RemoteConfig
    // public struct userAttributes { }                         //Удаленное получени данных(время между рекламой)
    // public struct appAtributes { }                           //Удаленное получени данных(время между рекламой)

    private void Awake()
    {       
        if (PlayerPrefs.GetInt("Level") == 0)
        {
            PlayerPrefs.SetInt("Level", 1);
        }

        if (PlayerPrefs.GetInt("NumLevelMetrica") == 0)
        {
            PlayerPrefs.SetInt("NumLevelMetrica", 1);
            AFEvents("session_first", "");
        }
        //--------------------------------------------
        if (PlayerPrefs.GetInt("Sound") == 0)                                                    //Проверка настроек звука
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
        //----------------------------------------
        if (PlayerPrefs.GetInt("Vibration") == 0)                                                //Проверка настроек вибрации
        {

            PlayerPrefs.SetInt("Vibration", 0);      //Включин
            MMVibrationManager.SetHapticsActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("Vibration", 1);     //Выключин
            MMVibrationManager.SetHapticsActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //TenjinConnect();
        GameAnalytics.Initialize();      
        _LoadingUI = LoadingUI;
        _UISettings = UISettings;       
        StaticLevel = new GameObject[Level.Length];
        StaticLevel = Level;
        AFEvents("session_start", "");
    }

    private void FixedUpdate()
    {
        Timer();
        if (isLoad)
        {
            isLoad = false;
            StartCoroutine(Loading());
        }
    }
    public static void CreateLevel()
    {
        SendLevelStart(PlayerPrefs.GetInt("NumLevelMetrica"));   
        AFEvents("level_started", PlayerPrefs.GetInt("NumLevelMetrica").ToString());
        isGameOver = false;
        isBlockMove = false;
        isStopTimer = false;
        isPauseTimer = true;
        time = 0;
        timeMetrica = 0;
        ActiveLevel = Instantiate(StaticLevel[PlayerPrefs.GetInt("Level")]);
    }
    public static void DestroyLevel()
    {
        Destroy(ActiveLevel);
    }

    public static void CreateLoading()
    {
       Instantiate(_LoadingUI);
    }

    public static void LoadLevel(bool isRestart)
    {
        isLoad = true;
        if (isRestart)
        {
            AFEvents("level_restart", PlayerPrefs.GetInt("NumLevelMetrica").ToString());
        }
    }

    public static void CompliteLevel()
    {
        if (StaticLevel.Length - 1 <= PlayerPrefs.GetInt("Level"))
        {
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("NumLevelMetrica", PlayerPrefs.GetInt("NumLevelMetrica") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            PlayerPrefs.SetInt("NumLevelMetrica", PlayerPrefs.GetInt("NumLevelMetrica") + 1);
        }
    }

    private void Timer()
    {
        if (!isStopTimer)
        {
            if (!isPauseTimer)
            {
                time += Time.deltaTime;
            }
            timeMetrica += Time.deltaTime;
        }
    }   

    /*
    //Удаленное получени данных
    void SetInterva(ConfigResponse response)
    {
        Adv_Interval = ConfigManager.appConfig.GetInt("Adv_Interval");
    }

    private void StartIntervalADS()
    {
        ConfigManager.FetchCompleted += SetInterva;          //Удаленное получени данных(время между рекламой)         
        ConfigManager.FetchConfigs<userAttributes, appAtributes>(new userAttributes(), new appAtributes()); //Удаленное получени данных(время между рекламой)
    }
    */

    public IEnumerator Loading()
    {
        CreateLoading();
        yield return new WaitForSeconds(1f);
        DestroyLevel();
        CreateLevel();
    }
    public static int GetTimer()
    {
        int timer = (int)System.Math.Round((double)timeMetrica, 0);
        return timer;
    }

    public static void OpenSettings()
    {
        GameObject setting = Instantiate(_UISettings);       
        Time.timeScale = 0;
    }

    public static void OpenLevel(int level)
    {
        DestroyLevel();
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("NumLevelMetrica", level);
        isStopTimer = false;
        isPauseTimer = true;
        time = 0;
        timeMetrica = 0;
        ActiveLevel = Instantiate(StaticLevel[level]);
        
    }

    //--------------------------AppMetrica-----------------------------//
    
    public static void SendLevelStart(int Level)                                      //Старт уровня 0
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["Start"] = Level;
        AppMetrica.Instance.ReportEvent("Progression", data);
        AppMetrica.Instance.SendEventsBuffer();
        print(Level);
    }
    public static void SendLevelFinish(int Level)                                      //Финисш уровня 0
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["Complete"] = Level;
        data["Level_time"] = Level + ":" + GetTimer();
        AppMetrica.Instance.ReportEvent("Progression", data);
        AppMetrica.Instance.SendEventsBuffer();
        print(Level);
        print(Level + ":" + GetTimer());
    }
    public static void SendLevelFail(int Level)                                      //Финисш уровня 0
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["Fail"] = Level;
        data["Level_time"] = Level + ":" + GetTimer();
        AppMetrica.Instance.ReportEvent("Progression", data);
        AppMetrica.Instance.SendEventsBuffer();
        print(Level);
        print(Level + ":" + GetTimer());
    }



    //----------------------------Tenjin---------------------------------//
    /*
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            TenjinConnect();
        }
    }
    public void TenjinConnect()
    {
        BaseTenjin instance = Tenjin.getInstance("KEY");

        //Sends install/open event to Tenjin
        instance.Connect();
    }

     */
    //------------------------AppsFlyerSDK-----------------------------//
    public static void AFEvents(string NameEvent, string ValueEvent)
    {
       // Dictionary<string, string> value = new Dictionary<string, string>();
       // value.Add(NameEvent, ValueEvent);
      //  AppsFlyer.sendEvent(NameEvent, value);
    }
   
}
