using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    [HideInInspector] public bool isActiveLevel;
    public bool isAllDeath;
    public float MaxDistantion;
    [SerializeField] private GameObject Key;
    [SerializeField] private BoxCollider colliderExit;  
    [SerializeField] private BoxCollider colliderEntranceNexLocation;
    [SerializeField] private GameObject Coridor;
    private RespawnBot[] respawnBots;
    [SerializeField] private BotControl[] bots;    
    private void Start()
    {
        if (Coridor != null)
        {
            Coridor.SetActive(false);
        } 
        respawnBots = GetComponentsInChildren<RespawnBot>();       
        StartCoroutine(ActivitiBots());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (colliderEntranceNexLocation!=null)
            {
                colliderEntranceNexLocation.enabled = true;
                colliderExit.GetComponentInParent<LocationManager>().DestroyAllBots();
            }
            if (Coridor != null)
            {
                Destroy(Coridor);
            }            
            Destroy(GetComponent<LocationManager>());
        }
    }
    public void isAllDeathBot()
    {
        StartCoroutine(AnaliseDeathAllBots());
    }

    public Transform SearchBot()
    {
        Indicators[] indicators = GetComponentsInChildren<Indicators>();
        for (int i=0; i < indicators.Length; i++)
        {
            if (indicators[i] != null)
            {
                return indicators[i].transform;               
            }                 
        }
        return null;
    }

    private IEnumerator AnaliseDeathAllBots()
    {        
        yield return new WaitForSeconds(0.5f);
        if (GetComponentInChildren<Indicators>() == null)
        {         
                isAllDeath = true;
                GetComponentInParent<ManagerLevel>().ActiveNextLocation();
                if (Coridor != null)
                {
                    Coridor.SetActive(true);
                    if (colliderExit != null)
                    {
                        colliderExit.enabled = false;
                    }
                    if (colliderEntranceNexLocation != null)
                    {
                        colliderEntranceNexLocation.enabled = false;                        
                        GetComponentInParent<ManagerLevel>().GetComponentInChildren<MoveCharacter>().GetComponentInChildren<Instructions>().ActiveNavigation(Coridor.transform, 20);
                    }
                }
                else
                {
                   GameObject key = Instantiate(Key, GetComponentInParent<ManagerLevel>().GetComponentInChildren<MoveCharacter>().GetComponentInChildren<Instructions>().DeathTarget, Quaternion.identity);
                   key.transform.SetParent(transform, false);
                    Transform target = key.transform;
                    //Transform target = GetComponentInChildren<Cell>().animDoor.gameObject.transform;
                    GetComponentInParent<ManagerLevel>().GetComponentInChildren<MoveCharacter>().GetComponentInChildren<Instructions>().ActiveNavigation(target, 2);
                    //GetComponentInChildren<Cell>().ActiveGlowDoor();  
                }
         
        }
        else
        {
            isAllDeath = false;
        }       
    }  

    public void ActiveDoors()
    {
        Transform target = GetComponentInChildren<Cell>().animDoor.gameObject.transform;
        GetComponentInParent<ManagerLevel>().GetComponentInChildren<MoveCharacter>().GetComponentInChildren<Instructions>().ActiveNavigation(target, 5);
        GetComponentInChildren<Cell>().ActiveGlowDoor(); 
    }  

    public IEnumerator ActivitiBots()
    {
        if (isActiveLevel)
        {
            if (respawnBots != null)
            {
                for (int i = 0; i < respawnBots.Length; i++)
                {
                    respawnBots[i].CreateBot();
                    yield return new WaitForSeconds(0.2f);
                }
            }
          
        }
    }

    public void DestroyAllBots()
    {
        bots = GetComponentsInChildren<BotControl>();
        for (int i = 0; i < bots.Length; i++)
        {
           Destroy(bots[i].gameObject);
        }
    }
}
