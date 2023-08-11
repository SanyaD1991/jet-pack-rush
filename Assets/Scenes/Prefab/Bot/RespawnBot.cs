using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RespawnBot : MonoBehaviour
{
    [SerializeField] private TextMeshPro textNumeBots;
    [SerializeField]private int NumberBots;
    [SerializeField] private GameObject Bot;
    [SerializeField] private Animation animationOpenDoor;
    private int StartNumberBots;
    public Transform[] ZonaFreePosition;
    public Transform PointCreate;
    // Start is called before the first frame update
    void Start()
    {
        StartNumberBots = NumberBots;    
    }

    public void CreateBot()
    {
        if (NumberBots>0) 
        {
            NumberBots--;
            if (animationOpenDoor!=null)
            {
                animationOpenDoor.Play("OpenDoor");              
                textNumeBots.text = NumberBots + "/" + StartNumberBots;
            }           
            GameObject bot = Instantiate(Bot, PointCreate.position, PointCreate.rotation);
            bot.transform.SetParent(transform, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bot"))
        {
            animationOpenDoor.Play("CloseDoor");
        }
    }

}
