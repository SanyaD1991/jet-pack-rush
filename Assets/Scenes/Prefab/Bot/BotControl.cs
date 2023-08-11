using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotControl : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] private Transform[] myZoneMove;
    [SerializeField] private Rigidbody pelvis;
    private bool isMyZone;
    public new Rigidbody rigidbody;  
    private bool isBlockMove;
    private bool isTargetFound;
    private Transform StartPosition;
    private RespawnBot respawnBot;
    private Instructions instructions;
    private bool isAnaliseRandom;
    public Animator animator;
    private Vector3 force;

    public enum IntelligenceList
    {       
        Easy = 0,
        Medium = 1,
        High = 2,
        Hostages = 3
    }
   
    public IntelligenceList typeBot;
    // Start is called before the first frame update
    void Start()
    {
        if (pelvis!=null)
        {
            pelvis.isKinematic = true;
        }
       
        if (animator!=null) 
        {
            animator.SetFloat("CycleOffset", Random.Range(0.0f, 1.0f));
        }
        agent = GetComponent<NavMeshAgent>();
        if (GetComponentInParent<RespawnBot>()!=null)
        {
            respawnBot = GetComponentInParent<RespawnBot>();
            isMyZone = false;
        }
        else
        {
            isMyZone = true;
        }
        
        instructions = GetComponentInChildren<Instructions>();
        StartCoroutine(RandomPosition(1));      
    }

    void Update()
    {
        if (!isBlockMove)
        {
            if (animator != null && agent!=null)
            {  
                animator.SetFloat("Velosity", agent.velocity.magnitude); 
            }

            if (instructions.target!=null)
            {
                switch (typeBot)
                {
                    case IntelligenceList.Easy:
                        EasyMove();
                        break;
                    case IntelligenceList.Medium:
                        MediumMove();
                        break;
                    case IntelligenceList.High:

                        break;
                    case IntelligenceList.Hostages:
                        HostagesMove();
                        break;
                }
            }
            else
            {
                if (isTargetFound)
                {
                    isTargetFound = false;
                    StartCoroutine(RandomPosition(5));
                }
            }

        }
        else
        {
            if (animator.enabled)
            {
                animator.SetBool("isAttack", false);
            }
        }      
    } 

    public void DeactivateBot()
    {
        isBlockMove = true;
        if (!isMyZone)
        {
            GetComponentInParent<RespawnBot>().CreateBot();
        }
        if (animator!=null)
        {
            animator.enabled = false;
        }
        instructions.DestroyGun();
        // instructions.isDeath = true;
        Destroy(GetComponent<NavMeshAgent>());
        //  rigidbody.isKinematic = false;
        //  rigidbody.useGravity = true;       
          //Destroy(GetComponent<BotControl>());
          Destroy(GetComponent<Indicators>());     
          GetComponentInParent<LocationManager>().isAllDeathBot();
          StartCoroutine(ActiveGravity());
    }
    public void DestroyBot()
    {        
        Destroy(gameObject);
    }

    public void Force(Vector3 Diraction)
    {
        force = Diraction; 
    }

    private IEnumerator RandomPosition(float time)
    {
        isAnaliseRandom = true; 
        float randPositionX = 0;
        float randPositionY = 0;
        float randPositionZ = 0;
        if (isMyZone)
        {
            randPositionX = Random.Range(myZoneMove[0].position.x, myZoneMove[1].position.x);
            randPositionY = transform.position.y;
            randPositionZ = Random.Range(myZoneMove[0].position.z, myZoneMove[1].position.z);
        }
        else
        {
            randPositionX = Random.Range(respawnBot.ZonaFreePosition[0].position.x, respawnBot.ZonaFreePosition[1].position.x);
            randPositionY = respawnBot.ZonaFreePosition[0].position.y;
            randPositionZ = Random.Range(respawnBot.ZonaFreePosition[0].position.z, respawnBot.ZonaFreePosition[1].position.z);
        
        }
        Vector3 randomPosition = new Vector3(randPositionX, randPositionY, randPositionZ);
        if (agent!=null)
        {
            agent.stoppingDistance = 0;
            agent.SetDestination(randomPosition);
        }
       
        yield return new WaitForSeconds(time);
        isTargetFound = true;
        isAnaliseRandom = false;
    }
    private IEnumerator RandomChase(float time)
    {
        isAnaliseRandom = true;
        float randPositionX = Random.Range(-5.0f,5.0f);
        float randPositionZ = Random.Range(-5.0f, 5.0f);
        Vector3 randomPosition = new Vector3(instructions.target.position.x+ randPositionX, instructions.target.position.y, instructions.target.position.z+ randPositionZ);
        if (agent!=null)
        {
            agent.SetDestination(randomPosition);
        }
        
        yield return new WaitForSeconds(time);
        isAnaliseRandom = false;
    }

    private void EasyMove()
    {
        isTargetFound = true;
        if (agent!=null)
        {
            agent.stoppingDistance = 2;
            agent.SetDestination(instructions.target.position);
        }      
     
        if (Vector3.Distance(instructions.target.position, transform.position)<2.5f && instructions.target!=null)
        {
            if (animator != null)
            {
                animator.SetBool("isAttack", true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isAttack", false);
            }
        }
    }  
    private void MediumMove()
    {
        isTargetFound = true;      
        if (instructions.isRecharge)
        {
            if (!isAnaliseRandom)
            {
                StartCoroutine(RandomPosition(2));
            }
        }
        else
        {
            if (agent != null)
            {
                agent.stoppingDistance = 10;
                agent.SetDestination(instructions.target.position);
            }
            if (instructions.isShot)
            {
                animator.SetBool("isAttack", true);
            }
            else
            {
                animator.SetBool("isAttack", false);
            }
        }  
    }
     private void HighMove()
    {

    }

    private void HostagesMove()
    {
        if (instructions.target == null)
        {
            if (!isAnaliseRandom)
            {
              //  StartCoroutine(RandomPosition(2));
            }
        }
        else
        {           
            agent.stoppingDistance = 2;
            agent.SetDestination(instructions.target.position);
        }
    }

    private IEnumerator ActiveGravity()
    {        
        yield return new WaitForSeconds(0.05f);
        pelvis.isKinematic = false;
        rigidbody.AddForce(force * 100, ForceMode.Impulse);
    }

}
