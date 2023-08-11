using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour
{
    public TextMeshProUGUI TextNumLevel;
    public Image BoxSprite;
    public TextMeshProUGUI TextProcent;
    public Transform LineProcent;
    private ManagerLevel _managerLevel;
    public starFxController stars;
    // Start is called before the first frame update
    void Start()
    {
        ManagerGame.isStopTimer = true;
        ManagerGame.SendLevelFinish(PlayerPrefs.GetInt("NumLevelMetrica"));
        ManagerGame.AFEvents("level_completed", PlayerPrefs.GetInt("NumLevelMetrica").ToString());
        _managerLevel = GetComponentInParent<ManagerLevel>();
        stars.ea = _managerLevel.NumeStars;
        TextNumLevel.text = "Level "+ PlayerPrefs.GetInt("NumLevelMetrica") +" complete!";
        ManagerGame.CompliteLevel();      
    }
    private void SetProcent(float Procent)
    {
        float procent = Procent;
        TextProcent.text = procent.ToString() + "%";
        float y = procent / 100.0f;
        LineProcent.localScale = new Vector3(1, y, 1);
    }
}
