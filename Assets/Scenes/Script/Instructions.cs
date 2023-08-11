using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    private bool isPlayer;
    [SerializeField] private Transform Key;
    [SerializeField] private SphereCollider MyTarget;
    [SerializeField] private SphereCollider MyScaner;
    [SerializeField] private bool isAutorotate;
    
    [HideInInspector] public string tagMyEnemy;
    [SerializeField] private GameObject[] myWeapon;
    [SerializeField] private Transform shooter;
    public Transform PointRespawnGun;
    public Transform target;
    [HideInInspector] public Vector3 DeathTarget;
    private float distanceToTarget;
    public bool isShot;
    private Gun gun;
    private ManagerLevel managerLevel;
    [SerializeField] private Material materialBullet;
    private Indicators indicators;
    [SerializeField] private float DelayShot;
    [SerializeField] float RechargeTime;
    [HideInInspector] public bool isRecharge;
    [SerializeField] private int NumeBullet;
    private int StartNumeBullet;
    [SerializeField] private Transform AimTransform;
    public Transform RamTransform;
    private Transform Coridor;
    private bool isActivenaviganion;
    [SerializeField] private GameObject JetPackObject;
    [SerializeField] private Transform JetPackPosition;
    public JetPack jetPack;
    private float distanceToNewLocation;
    private bool isBlockTarget;
    public Vector3 OffsetBody;
    public Quaternion OffsetAim;
    public Quaternion StartTransformGun;
    private bool isCheckBot;
    public enum IntelligenceList
    {
        Knife = 0,
        Gun = 1,
        Machine = 2,
        NotWeapon = 3
    }
    [SerializeField]
    private IntelligenceList typeBattle;

    void Start()
    {

        MyScaner = GetComponent<SphereCollider>();
        isBlockTarget = true;
        StartNumeBullet = NumeBullet;
        indicators = GetComponentInParent<Indicators>();
        managerLevel = GetComponentInParent<ManagerLevel>();
        if (PointRespawnGun==null)
        {
            PointRespawnGun = transform;
        }
        else
        {
            StartTransformGun = PointRespawnGun.localRotation;
        }
        CreateGun();
        //CreateJetPack();
        if (indicators != null)
        {
            isPlayer = indicators.isPlayer;
            if (isPlayer)
            {
                tagMyEnemy = "Bot";
            }
            else
            {
                tagMyEnemy = "Player";
            }
        }
        StartCoroutine(ActiveTarget());
    }
    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.CompareTag("DeathZone"))
        {
           if (isPlayer)
           {
             shooter.GetComponent<MoveCharacter>().DeactiveCharacter();
             MMVibrationManager.Haptic(HapticTypes.None, false, true, this);
           }
           else
           {
             shooter.GetComponent<BotControl>().DestroyBot();
           }
        }      
    }
    private void OnTriggerStay(Collider other)
    {      
            if (isRecharge && RechargeTime!=0)
            {
                target = null;
            }
            else
            {
                if (other.gameObject.CompareTag(tagMyEnemy) && target == null)
                {
                    target = other.transform;
                }
            }

        if (other.gameObject.CompareTag("Fuel"))
        {
            if (isPlayer)
            {
                float distanceToFuel = Vector3.Distance(transform.position, other.gameObject.transform.position);
                if (distanceToFuel < 3)
                {
                    indicators.AddFuel(5);
                    Destroy(other.gameObject);
                }
            }
        }

        if (other.gameObject.CompareTag("Key"))
        {
            if (isPlayer)
            {
                float distanceToFuel = Vector3.Distance(transform.position, other.gameObject.transform.position);
                if (distanceToFuel < 2)
                {
                    HideGun();
                    Destroy(other.gameObject);
                }
            }
        }
    }   
    void Update()
    {        
        if (target!=null && !isBlockTarget)
        {
            if (!isPlayer && MyScaner!=null)
            {
                MyScaner.radius = 0;
            }                      
            AutoRotate();
            ActiveAim();
            distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (!isShot)
            {
                if (NumeBullet > 0)
                {
                    StartCoroutine(Shot());
                }
                else
                {
                    if (!isRecharge)
                    {
                        StartCoroutine(CurRechargeTime());
                    }
                }
            }
            else
            {
                gun.NotShot();
            }


            if (indicators != null)
            {
                if (isPlayer)
                {
                    if (distanceToTarget > 12)
                    {
                        target = null;
                    }
                }
                else
                {
                    if (distanceToTarget > 40)
                    {
                        target = null;
                    }
                }
            }
           
        }
        else
        {
            if (PointRespawnGun!=null)
            {
                PointRespawnGun.localRotation = StartTransformGun;
            }
           
            if (gun != null)
            {
               
                MyScaner.radius = 12;
                gun.NotShot();
                CheckBot();
            }          
         
        }      
    }

    private void CreateGun()
    {
        GameObject Weapon;
            switch (typeBattle)
            { 
                case IntelligenceList.Knife:
                     Weapon = Instantiate(myWeapon[0], PointRespawnGun.position, PointRespawnGun.rotation);
                     Weapon.transform.SetParent(PointRespawnGun);
                    break;

                case IntelligenceList.Gun:
                    Weapon = Instantiate(myWeapon[1], PointRespawnGun.position, PointRespawnGun.rotation);
                    Weapon.transform.SetParent(PointRespawnGun);
                    gun = Weapon.GetComponent<Gun>();
                    gun.materialBullet = materialBullet;
                    gun.Parent = managerLevel.transform;
                    break;

                case IntelligenceList.Machine:
                    if (myWeapon[2]!=null)
                    {
                        Weapon = Instantiate(myWeapon[2], PointRespawnGun.position, PointRespawnGun.rotation);
                        Weapon.transform.SetParent(PointRespawnGun);
                        gun = Weapon.GetComponent<Gun>();
                        gun.materialBullet = materialBullet;
                        gun.Parent = managerLevel.transform;
                    }                   
                    break;

                case IntelligenceList.NotWeapon:
                     break;
            }  
    }

    public void DestroyGun()
    {
        Destroy(PointRespawnGun.gameObject);
        Destroy(gameObject);
    }
    private IEnumerator ActiveTarget()
    {       
        yield return new WaitForSeconds(0.7f);
        isBlockTarget = false;
    }
    private void CreateJetPack()
    {
        if (JetPackObject!=null)
        {
            GameObject jet = Instantiate(JetPackObject);
            jet.transform.SetParent(JetPackPosition, false);
            jetPack = jet.GetComponent<JetPack>();
        }
    }

    private IEnumerator Shot()
    {
        switch (typeBattle)
        {
            case IntelligenceList.Knife:             
                break;

            case IntelligenceList.Gun:
                isShot = true;
                NumeBullet--;
                gun.Shot(isPlayer);
                yield return new WaitForSeconds(DelayShot);
                isShot = false;
                break;

            case IntelligenceList.Machine:
                isShot = true;
                NumeBullet--;
                gun.Shot(isPlayer);
                MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);
                MyTarget.radius = 20;
                yield return new WaitForSeconds(0.1f);
                MyTarget.radius = 0.5f;
                yield return new WaitForSeconds(DelayShot-0.1f);               
                isShot = false;
                break;
        }
       
    }
    private IEnumerator CurRechargeTime()
    {      
        isRecharge = true;
        yield return new WaitForSeconds(RechargeTime);
        isRecharge = false;
        NumeBullet=StartNumeBullet;     
    }
    public void Death()
    {
        if (isPlayer)
        {
            shooter.GetComponent<MoveCharacter>().DeactiveCharacter();
        }
        else
        {
            shooter.GetComponent<BotControl>().DeactivateBot();
        }

    } 
  private void AutoRotate()
    {
        if (isAutorotate)
        {
            DeathTarget = target.position;
            Vector3 focus = Vector3.Scale(target.transform.position, new Vector3(1, 0, 1));
            focus.y = shooter.position.y;
            shooter.LookAt(focus);
            shooter.eulerAngles = shooter.eulerAngles - OffsetBody;
            PointRespawnGun.LookAt(target);
        }
       // Quaternion lerp=transform.rotation* OffsetAim;
       // PointRespawnGun.rotation = Quaternion.Lerp(PointRespawnGun.rotation, lerp, 5 * Time.deltaTime);
    }

    private void ActiveAim()
    {
        if (AimTransform != null)
        {
            isCheckBot = false;
            AimTransform.localScale = Vector3.one;
            RamTransform.localEulerAngles = new Vector3(90f, 0f, 0f);
            AimTransform.position = target.position;
        }       
    }

    private void DeactiveAim()
    {
        if (AimTransform != null)
        {
            if (AimTransform.position!= transform.position)
            {
               // AimTransform.localScale = Vector3.zero;
                AimTransform.position = transform.position;
            }
        }
    }
    public void ActiveNavigation(Transform coridor, float distance)
    {
       AimTransform.localScale = Vector3.one;
       AimTransform.position = shooter.position;
       Coridor = coridor;
     //  isActivenaviganion = true;
       distanceToNewLocation = distance;
    }
    public void DeactiveNavigation()
    {
       // isActivenaviganion = false;
        AimTransform.localScale = Vector3.zero;
        AimTransform.position = transform.position;
        Coridor = null;
        RamTransform.localEulerAngles = new Vector3(90f,0f,0f);
    }  
    
    private void CheckBot()
    {       
        if (AimTransform != null)
        {
            if (AimTransform.position != transform.position)
            {
                // AimTransform.localScale = Vector3.zero;
                AimTransform.position = transform.position;
            }
            if (!isCheckBot)
            {
                isCheckBot = true;
                ActiveNavigation(managerLevel.SearchBots(), 5);
            }
        }

        if (Coridor != null)
        {
            RamTransform.transform.LookAt(Coridor);
            if (Vector3.Distance(Coridor.position, transform.position) < distanceToNewLocation)
            {
                DeactiveNavigation();
            }
        }       
    }  
    
    private void HideGun()
    {
        gun.gameObject.SetActive(false);
        Key.gameObject.SetActive(true);
        LocationManager  locationManager = managerLevel.SearchActiveLocation();
        locationManager.ActiveDoors();
    }

    public void ShowGun()
    {
        gun.gameObject.SetActive(true);
        Key.gameObject.SetActive(false);
    }
}
