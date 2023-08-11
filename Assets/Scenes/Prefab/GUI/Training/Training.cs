using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Training : MonoBehaviour
{
    [HideInInspector] public GameObject TrainObj;
    [HideInInspector] public Animator animator;
    [Header("Setting AppsFlyerSDK")]
    [SerializeField] private int StepTutorial;
    [SerializeField] private bool isStartTutorial;
    [SerializeField] private bool isEndTutorial;
    [Header("Setting Training")]

    [SerializeField] private string TextTraining;
    public enum Direction
    {
        Infinity = 0,
        Up = 1,
        Right = 2,
        Left = 3,
        Down = 4
    }
    [SerializeField] private Direction HandMovement;
    [SerializeField] private int ShowTrainigLevel;
    private bool isActive;
    [HideInInspector] public TextMeshProUGUI GUITextTraining;   
    
    // Start is called before the first frame update
    void Start()
    {      
        animator.SetInteger("Direction", (int)HandMovement);
        TrainObj.GetComponent<Canvas>().worldCamera = Camera.main;//GameObject.FindGameObjectWithTag("CameraUI").GetComponent<Camera>();
        if (PlayerPrefs.GetInt("NumLevelMetrica") != ShowTrainigLevel)
        {          
            Destroy(gameObject);
        }
        else
        {
            if (isStartTutorial)
            {
                //ManagerGame.AFEvents("tutorial_" + levelDestroy + "_started", "");
            }
                //ManagerGame.AFEvents("tutorial_step_completed", StepTutorial);
            if (isEndTutorial)
            {
                //ManagerGame.AFEvents("tutorial_completed", "");
            }
        }
      

    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetMouseButton(0) && ManagerGame.isBlockMove==false)
        {                     
            Destroy(gameObject);
        }
        if (ManagerGame.isStartGame)
        {
            if (!isActive)
            {
                isActive = true;
                StartCoroutine(ShowTraining());
            }           
        }
        else
        {
            TrainObj.SetActive(false);
        }
    }
    private IEnumerator ShowTraining()
    {
        yield return new WaitForSeconds(2f);
        TrainObj.SetActive(true);
    }  
}
