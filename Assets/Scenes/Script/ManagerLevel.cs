using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLevel : MonoBehaviour
{
    [HideInInspector] public bool isBlockTouch;
    [HideInInspector] public bool isGameOver;
    [SerializeField] private GameObject Character;
    [SerializeField] private GameObject UIContinue;
    [SerializeField] private GameObject UIFail;
    [SerializeField] private Transform StartPositionCharacter;
    [HideInInspector] public LocationManager[] locationManagers;
    private ManagerGUI _managerGUI;
    [SerializeField] private Canvas canvas;
    public int NumeStars;
    private float procent;
    private bool isStartGame;
    private float MaxDistance;
    public Transform FinishPosition;
    private int NumeLocation;
    // Start is called before the first frame update
    void Start()
    {
        locationManagers = GetComponentsInChildren<LocationManager>();
        ActiveNextLocation();
        CreateGUI();
        CreateCharacter();
        ManagerGame.isStartGame = true;     
    }
    
    private void CreateGUI()
    {
        canvas.worldCamera = Camera.main;
        canvas = Instantiate(canvas);
        canvas.transform.SetParent(transform, false);
        _managerGUI = canvas.GetComponent<ManagerGUI>();   
    }
    public void CreateCharacter()
    {
        GameObject character = Instantiate(Character, StartPositionCharacter.position, StartPositionCharacter.rotation);
        character.transform.SetParent(transform, false);
    }
    public void AnaliseProgressBar()
    {
        procent = 1.0f; //
        if (procent < 0.3f)
        {
            NumeStars = 1;
        }
        else if (procent < 0.8f)
        {
            NumeStars = 2;
        }
        else
        {
            NumeStars = 3;
        }
        _managerGUI.sliderBar.value = procent;
    }
    public void AnaliseGame()
    {        
        if (!isGameOver || procent >= 0.9f)
        {
            isGameOver = true;
            isBlockTouch = true;
            _managerGUI.GUILevel.SetActive(false);          
            Instantiate(UIContinue).transform.SetParent(canvas.transform, false);           
        }
        else
        {
            if (!isGameOver && NumeStars==1)
            {
                isGameOver = true;
                isBlockTouch = true;
                _managerGUI.GUILevel.SetActive(false);
                Instantiate(UIFail).transform.SetParent(canvas.transform, false);
            }
        }         
    }
    
    public void InstantiateObject(GameObject insObj, Vector3 position, Quaternion rotation, Vector3 scale, float timeDestroy)
    {
        GameObject tempObj = Instantiate(insObj, position, rotation);
        tempObj.transform.SetParent(transform, false);
        tempObj.transform.localScale = tempObj.transform.localScale + scale;
        Destroy(tempObj,timeDestroy);
    }

    public void Continue()
    {
        if (!isGameOver)
        {            
            isGameOver = true;
            ManagerGame.isBlockMove = true;
            _managerGUI.GUILevel.SetActive(false);
            StartCoroutine(InstantiateContinue());
        }
    }
    public void GameOver()
    {
        if (!isGameOver)
        {           
            isGameOver = true;
            _managerGUI.GUILevel.SetActive(false);
            StartCoroutine(InstantiateFail());
        }
    }


    private IEnumerator InstantiateFail()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(UIFail).transform.SetParent(canvas.transform, false);
    }

    private IEnumerator InstantiateContinue()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(UIContinue).transform.SetParent(canvas.transform, false);
    }

    public void SetProgressLine(Vector3 CharPosition)
    {
        if (!isStartGame)
        {
            isStartGame = true;
            MaxDistance = Vector3.Distance(FinishPosition.position, CharPosition);
        }
        float CurrentDistance = Vector3.Distance(FinishPosition.position, CharPosition);
        float progress = 1 - (CurrentDistance / MaxDistance);
        _managerGUI.sliderBar.value = progress;
    }

    public void ActiveNextLocation()
    {
        if (NumeLocation < locationManagers.Length)
        {
            locationManagers[NumeLocation].isActiveLevel = true;
           StartCoroutine(locationManagers[NumeLocation].ActivitiBots());
            NumeLocation++;
        }        
    }

    public Transform SearchBots()
    {
        return locationManagers[NumeLocation-1].SearchBot();
    }

    public LocationManager SearchActiveLocation()
    {
        return locationManagers[NumeLocation-1];
    }
    private IEnumerator CurActiveNextLocation()
    {
        yield return new WaitForSeconds(0.5f);
        ActiveNextLocation();
    }   
}
