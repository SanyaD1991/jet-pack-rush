using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public AudioMixerGroup Mixer;
    public Image[] CheckBox;
    public Color Enable;
    public Color Dicable;
    private bool isPauseMove;
    private bool isStatusVibration;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            EnabledSound();
        }
        else
        {
            DisabledSound();
        }
        if (PlayerPrefs.GetInt("Vibration") == 0)
        {
            EnabledVibration();
            isStatusVibration = true;
        }
        else
        {
            DisabledVibration();
            isStatusVibration = false;
        }
        MMVibrationManager.SetHapticsActive(false);
        Time.timeScale = 0;      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            close();
        }
    }
    public void close()
    {
        if (isPauseMove)
        {
            ManagerGame.isBlockMove = false;
        }
        if (isStatusVibration)
        {
            PlayerPrefs.SetInt("Vibration", 0);
            MMVibrationManager.SetHapticsActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("Vibration", 1);
            MMVibrationManager.SetHapticsActive(false);
        }
        Destroy(gameObject, 0.1f);
        Time.timeScale = 1;
    }

    public void OffOnnSource()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            DisabledSound();
        }
        else
        {
            EnabledSound();
        }
    }
    public void OffOnVibration()
    {
        if (isStatusVibration)
        {
            DisabledVibration();
        }
        else
        {
            EnabledVibration();
        }
    }

    private void EnabledSound()
    {
        AudioListener.volume = 1;
        CheckBox[0].color = Enable;
        CheckBox[1].enabled = true;
        CheckBox[2].enabled = false;
        PlayerPrefs.SetInt("Sound", 0);
    }
    private void DisabledSound()
    {
        CheckBox[0].color = Dicable;
        CheckBox[1].enabled = false;
        CheckBox[2].enabled = true;
        AudioListener.volume = 0;
        PlayerPrefs.SetInt("Sound", 1);
    }

    private void EnabledVibration()
    {
        // PlayerPrefs.SetInt("Vibration", 0);
        CheckBox[3].color = Enable;
        CheckBox[4].enabled = true;
        CheckBox[5].enabled = false;
        isStatusVibration = true;
    }

    private void DisabledVibration()
    {
        //PlayerPrefs.SetInt("Vibration", 1);
        CheckBox[3].color = Dicable;
        CheckBox[4].enabled = false;
        CheckBox[5].enabled = true;
        isStatusVibration = false;
    }
    void OnApplicationPause(bool pauseStatus)
    {
        close();
    }
}
