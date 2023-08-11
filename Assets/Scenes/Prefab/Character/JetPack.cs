using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{    
    public ParticleSystem[] particleSystemsSmocke;  
    private Indicators indicators;
    private bool isDelay;
    private bool isActiveJet;
    private bool isActiveBuster;
    private bool isCheckBuster;

    [System.Obsolete]
    private void Start()
    {
        isActiveJet = true;
        indicators = GetComponentInParent<Indicators>();
        particleSystemsSmocke[0].gameObject.SetActive(true);
        particleSystemsSmocke[1].gameObject.SetActive(true);
        particleSystemsSmocke[2].gameObject.SetActive(true);
        particleSystemsSmocke[3].gameObject.SetActive(true);
        // DeactiveJetPack();
    }

    
    public void ActoveJetPack()
    {
            MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);            
            if (!isActiveJet) 
            {
                isActiveJet = true;              
                         
                particleSystemsSmocke[0].Play(true);                 
                particleSystemsSmocke[1].Play(true);                 

        }
            if (isActiveBuster)
            {
                if (!isCheckBuster)
                {
                    isCheckBuster = true;               
                    particleSystemsSmocke[2].Play(true);
                    particleSystemsSmocke[3].Play(true);
            }
            }
            else
            {
                if (isCheckBuster)
                {
                    isCheckBuster = false;
                    particleSystemsSmocke[2].Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    particleSystemsSmocke[3].Stop(true, ParticleSystemStopBehavior.StopEmitting);                              
                }
            }

        if (!isDelay)
            {
                StartCoroutine(FuelConsumption());
            } 
    }
   
    public void DeactiveJetPack()
    {       
            
        if (isActiveJet)
        {
            isActiveBuster = false;
            isActiveJet = false;
            isCheckBuster = false;           
            particleSystemsSmocke[0].Stop(true, ParticleSystemStopBehavior.StopEmitting);
            particleSystemsSmocke[1].Stop(true, ParticleSystemStopBehavior.StopEmitting);
            particleSystemsSmocke[2].Stop(true, ParticleSystemStopBehavior.StopEmitting);
            particleSystemsSmocke[3].Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private IEnumerator FuelConsumption()
    {
        isDelay = true;
        isActiveBuster = indicators.AnaliseBppster();
        yield return new WaitForSeconds(1);
        if (isActiveJet)
        {          
           indicators.FuelConsumption();
        }
        isDelay = false;
    }
}
