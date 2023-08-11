using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.Review;
using UnityEngine.UI;

public class Rate : MonoBehaviour
{    
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;

    // Update is called once per frame
    private void Awake()
    {       
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        GetComponentInChildren<Canvas>().planeDistance = 0.5f;
        Time.timeScale = 0; 
    }  
    ///////////////////////////////////////////////////////////////////////
    public void RateNowPopUp() 
    {
       // SendClickFromPopUp(PlayerPrefs.GetInt("Level"));
        // RateNow();
        StartCoroutine(RequestReview());
       // exit();
    }

    public void RateNowMenu()
    {
        //SendClickFromMenu(PlayerPrefs.GetInt("Level"));
        //  RateNow();
        StartCoroutine(RequestReview());
        //exit();
    }

    private void RateNow()
    {  
        PlayerPrefs.SetInt("isRate", 1);
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
#else
       // Application.OpenURL("itms-apps://itunes.apple.com/app/id" + Application.identifier);
       // Device.RequestStoreReview();
#endif
    }

    public void close()
    {
       // SendNotRate(PlayerPrefs.GetInt("Level"));
        Time.timeScale = 1;
        StartCoroutine(CurDestroy());
    }

    public void exit()
    {
        Time.timeScale = 1;
        StartCoroutine(CurDestroy());
    }

    private IEnumerator CurDestroy()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    ///////////////////////////////////////////////////////////////////////
    private IEnumerator RequestReview()
    {      
        _reviewManager = new ReviewManager();
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
          
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();


        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
       
         yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
           
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }


    /*
    public void SendClickFromMenu(int level)                    //Оценили из меню
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["the_click_from_menu"] = level;
        AppMetrica.Instance.ReportEvent("rate", data);
        AppMetrica.Instance.SendEventsBuffer();
    }

    public void SendNotRate(int level)                          //Пропустили оценить
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["do_not_rate"] = level;
        AppMetrica.Instance.ReportEvent("rate", data);
        AppMetrica.Instance.SendEventsBuffer();
    }

    public void SendClickFromPopUp(int level)                   //Оценили из всплывающего окна
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["the_click_from_pop-up "] = level;
        AppMetrica.Instance.ReportEvent("rate", data);
        AppMetrica.Instance.SendEventsBuffer();
    }
    */
}
