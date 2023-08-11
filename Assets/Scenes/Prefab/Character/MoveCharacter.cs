using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    private Indicators indicators;
    //setting camera
    private Transform cameraTransform;
    private float smooth = 4.0f;
    public Vector3 offsetPositionCamera = new Vector3(0, 15, -15);
    private bool isBlockMoveCamera;

    //setting Animation
    [SerializeField] private Animator animator;

    //setting Control
    [SerializeField] private GameObject Joystick;
    private Vector3 moveVector;
    private CharacterController characterController;
    private MobileController mobileController;
    [SerializeField] private float speedMove;

    //setting Gravity
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private float gravity = -9.81f;
    private float groundDistance = 0.2f;
    private Vector3 velosity;
    private bool isGrounded;

    //setting Target
    private Instructions instructions;

    //setting JetPack
    [SerializeField] private LayerMask DeathZone;  
    private bool isActiveDelayJetPack;
   
    void Start()
    {        
        indicators = GetComponent<Indicators>();
        instructions = GetComponentInChildren<Instructions>();
        cameraTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        CreateJoystick();       
    }

    [System.Obsolete]
    void Update()
    {
        CharacterMove(ManagerGame.isBlockMove);
        CheckGravity(ManagerGame.isBlockMove);
        CameraMove();    
    }
    private void CharacterMove(bool isBlockMove)
    {
        if (!isBlockMove)
        {
            moveVector = Vector3.zero;
            moveVector.x = mobileController.Horizontal() * speedMove;
            moveVector.z = mobileController.Vertical() * speedMove;
            //Анимация передвижение персонажа
            if (moveVector.x != 0 || moveVector.z != 0)
            {
                //animator.SetBool("isForward", true);
            }
            else
            {
                //animator.SetBool("isForward", false);
            }

            //Поворот персонажа
            if (instructions.target==null) 
            {
                animator.SetBool("isAttack", false);
                if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
                {
                    Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, speedMove, 0.0f);
                    Quaternion rotation = Quaternion.LookRotation(direct);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speedMove * Time.deltaTime);                   
                }               
            }
            else
            {
               animator.SetBool("isAttack", true);
            }

            //Передвижение по направлению           
            characterController.Move(moveVector * Time.deltaTime);
            float velocityZ = Vector3.Dot(moveVector.normalized, transform.forward);
            float velocityX = Vector3.Dot(moveVector.normalized, transform.right);

            animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
            animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
            animator.SetFloat("Intesivity", Mathf.Abs(moveVector.x+ moveVector.z)/2);
        }
        else
        {
            animator.SetFloat("VelocityZ", 0);
            animator.SetFloat("VelocityX", 0);
            animator.SetBool("isFlay", false);
            animator.SetBool("isAttack", false);
            mobileController.HideJoystick();
            indicators.HideIndicator();            
        }
    }

    private void CreateJoystick()
    {
        GameObject obJoystick = Instantiate(Joystick);
        obJoystick.transform.SetParent(GetComponentInParent<ManagerLevel>().transform, false);
        mobileController = obJoystick.GetComponent<MobileController>();
    }

    [System.Obsolete]
    private void CheckGravity(bool isBlockMove)
    {
        if (!isBlockMove)
        {
            if (indicators.Buster > 0)
            {
                instructions.jetPack.ActoveJetPack();
                animator.SetBool("isFlay", true);
                if (transform.position.y > 5)
                {
                    if (!isActiveDelayJetPack)
                    {
                        StartCoroutine(AutoPuskJetPack());
                    }
                }
                else
                {
                    if (!isActiveDelayJetPack)
                    {
                        velosity.y = 3;
                    }
                    else
                    {
                        velosity.y = 0.2f;
                    }
                }
            }
            else
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                if (isGrounded && velosity.y < 0)
                {
                    animator.SetBool("isFlay", false);
                    velosity.y = -8;
                }
                else
                {
                    animator.SetBool("isFlay", true);
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, Vector3.down, out hit, 100.0f, groundMask))
                    {
                        velosity.y = -8;
                        instructions.jetPack.DeactiveJetPack();
                    }
                    else
                    {

                        if (indicators.Fuel > 0)
                        {
                            instructions.jetPack.ActoveJetPack();
                            if (transform.position.y > 3)
                            {
                                if (!isActiveDelayJetPack)
                                {
                                    StartCoroutine(AutoPuskJetPack());
                                }
                            }
                            else
                            {
                                if (!isActiveDelayJetPack)
                                {
                                    velosity.y = 1;
                                }
                                else
                                {
                                    velosity.y = 0.2f;
                                }
                            }
                        }
                        else
                        {
                            velosity.y = -8;
                            instructions.jetPack.DeactiveJetPack();
                        }

                    }

                }
            }           
        }
        else
        {
            instructions.jetPack.DeactiveJetPack();
        }
        velosity.y += gravity * Time.deltaTime;
            characterController.Move(velosity * Time.deltaTime);       
    }
    private void CameraMove()
    {
        if (!isBlockMoveCamera)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, transform.position + offsetPositionCamera, Time.deltaTime * smooth);
        }
    } 
    
    public void DeactiveCharacter()
    {
        GetComponentInParent<ManagerLevel>().GameOver();
        Destroy(GetComponentInChildren<JetPack>());
        Destroy(instructions.gameObject);       
        Destroy(indicators);
        Destroy(GetComponentInChildren<Label>().gameObject);
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(mobileController.gameObject);          
        isBlockMoveCamera = true;
        animator.enabled = false;
        Destroy(GetComponent<MoveCharacter>());
    }
    public void DestroyCharacter()
    {
        GetComponentInParent<ManagerLevel>().GameOver();
        //GetComponentInParent<ManagerLevel>().CreateCharacter();
        Destroy(mobileController.gameObject);
        Destroy(gameObject);
    }

    private IEnumerator AutoPuskJetPack()
    {
        isActiveDelayJetPack = true;
        if (transform.position.y > 3)
        {
            velosity.y = 0.5f;
        } 
        yield return new WaitForSeconds(0.5f);
        isActiveDelayJetPack = false;

    }
}
