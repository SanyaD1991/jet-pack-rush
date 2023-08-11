using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGUI : MonoBehaviour
{
    public TextMeshProUGUI NumeLevel;  
    public GameObject GUILevel;
    public Slider sliderBar;

    private void Start()
    {
        NumeLevel.text = "Level: " + PlayerPrefs.GetInt("NumLevelMetrica").ToString();
    }
}
