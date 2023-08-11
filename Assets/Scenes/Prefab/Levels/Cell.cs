using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Animation animDoor;
    [SerializeField] private Transform Target;
    [SerializeField] private Transform PositionKey;
    [SerializeField] private Instructions[] instructions;
    public BoxCollider trigger;
    // Start is called before the first frame update
    void Start()
    {       
        instructions = GetComponentsInChildren<Instructions>();
    }

    private void OpenDoor()
    {
        animDoor.Stop("ScaleDoor");
        animDoor.Play("OpenDoor");
        for(int i=0; i< instructions.Length; i++)
        {
            instructions[i].target = Target;
        }
        trigger.enabled = false;
        ManagerGame.isBlockMove = true;
        StartCoroutine(Continue());
    }
    public void ActiveGlowDoor()
    {
        animDoor.Play("ScaleDoor");
        trigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<Instructions>().ShowGun();
            OpenDoor();
        }
    }

    private IEnumerator Continue()
    {
        
        yield return new WaitForSeconds(5);
        GetComponentInParent<ManagerLevel>().Continue();
    }
}
