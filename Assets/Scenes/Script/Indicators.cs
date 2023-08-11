using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicators : MonoBehaviour
{
    public bool isPlayer;
    [SerializeField] private int Life;
    public int Fuel;
    [HideInInspector] public int Buster;
    private int Startlife;
    private int StartFuel;
    private bool isActiveBuster;
    private Instructions instructions;
    [SerializeField] private Canvas canvasLabel;
    [SerializeField] private GameObject ObjectLabel;
    [HideInInspector] public Label label;
    
    private void Start()
    {
        Startlife = Life;
        StartFuel = Fuel;
        if (canvasLabel != null)
        {
            CreateCanvasLabel();          
        }
        if (ObjectLabel!=null)
        {
            CreateObjectLabel();
        }
        instructions = GetComponentInChildren<Instructions>();
    }

    public void Hit(Vector3 Diraction)
    {       
        Life--;
        label.hit();        
        RefreshLabelLife();
        if (Life<=0) 
        {
            instructions.Death();
            label.DestroyLabel();
            
        }       
        if (isPlayer)
        {
            MMVibrationManager.Haptic(HapticTypes.Warning, false, true, this);
        }
        else
        {          
            GetComponent<BotControl>().Force(Diraction);
        }      
    }

    public void AddFuel(int litr)
    {
        Fuel += litr;
        if (Fuel> StartFuel)
        {
            Buster = Fuel - StartFuel;
            Fuel = StartFuel;
        }
        RefreshLabelFuel();
    }
    public bool AnaliseBppster()
    {
        if (Fuel>0)
        {
            if (Buster>0)
            {              
                isActiveBuster = true;
            }
            else
            {
                isActiveBuster = false;
              
            }          
        }        
        return isActiveBuster;
    }

    public void FuelConsumption()
    {
        if (Fuel > 0)
        {
            if (Buster > 0)
            {
                Buster--;               
            }
            else
            {                
                Fuel--;
            }
        }
        RefreshLabelFuel();
    }

    private void CreateCanvasLabel()
    {
        Canvas myCanvas = Instantiate(canvasLabel);
        myCanvas.worldCamera = Camera.main;
        label = myCanvas.GetComponent<Label>();
        myCanvas.transform.SetParent(transform, false);
    }
    private void CreateObjectLabel()
    {
        GameObject myLabel = Instantiate(ObjectLabel);       
        label = myLabel.GetComponent<Label>();
        myLabel.transform.SetParent(transform, false);
    }
  
    private void RefreshLabelLife()
    {
        label.transformHeals.localScale = new Vector3((float)Life/(float)Startlife, 1.0f, 1.0f);      
    }
    private void RefreshLabelFuel()
    {
        label.transformFuel.localScale = new Vector3((float)Fuel / (float)StartFuel, 1.0f, 1.0f);
        label.transformFuelBuster.localScale = new Vector3((float)Buster / (float)StartFuel, 1.0f, 1.0f);
    }
    public void HideIndicator()
    {
        label.gameObject.SetActive(false);
    }
}
