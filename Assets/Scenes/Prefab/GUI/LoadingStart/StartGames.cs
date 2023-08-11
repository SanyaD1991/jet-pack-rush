using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGames : MonoBehaviour
{
    public float TimeLoading; 
    public GameObject Loading;
    private void Start()
    {
       // ManagerGame.SendLevelStart(0);
        StartCoroutine(LoadingGame());
    }
    IEnumerator LoadingGame()
    {       
       
        yield return new WaitForSeconds(TimeLoading);
       // ManagerGame.SendLevelFinish(0);
        ManagerGame.CreateLevel();
        Loading.SetActive(true);
        yield return new WaitForSeconds(1f);       
        Destroy(GetComponentInParent<Canvas>().gameObject);
    }
}
