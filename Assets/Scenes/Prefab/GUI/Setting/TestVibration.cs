using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVibration : MonoBehaviour
{
    private bool isVobro;
    private float timeVib;
    private bool isPredel;

    void Update()
    {
        if (isPredel)
        {
            timeVib += Time.deltaTime;
            if (timeVib>=0.2f)
            {
                isPredel = false;
                timeVib = 0;
            }
        }
        if (Input.GetMouseButton(0))
        {
            OnVibrator();
        }
    }
    public void OnVibrator()
    {
        if (!isPredel)
        {
            MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
            isPredel = true;
        }
    CameraShake.Shake(2, 0.2f);
    print("ZzzzzzZ");
    isVobro = true;
    }
    public void OffVibrator()
    {
        if (isVobro)
        {
            isVobro = false;
        
            CameraShake.Shake(0, 0);
        }
       
    }
}
