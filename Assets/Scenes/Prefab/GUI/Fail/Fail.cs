using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fail : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ManagerGame.isStopTimer = true;
        ManagerGame.SendLevelFail(PlayerPrefs.GetInt("NumLevelMetrica"));
        ManagerGame.AFEvents("level_failed", PlayerPrefs.GetInt("NumLevelMetrica").ToString());
    }
}
