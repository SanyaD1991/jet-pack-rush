using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerUI : MonoBehaviour
{
    public bool isLoopAnim;
    public GameObject destroyObject;
    public GameObject[] ObjectAnim;
    public bool isDestroy;
    public float TimeLife;
    public bool isLooTheCamera;
    void Start()
    {
        if (isDestroy)
        {
            if (destroyObject==null)
            {
                Destroy(gameObject, TimeLife);
            }
            else
            {
                Destroy(destroyObject, TimeLife);
            }
          
        }
        
        if (isLooTheCamera)
        {
            transform.LookAt(Camera.main.transform);
        }
        GetObjectAnim();
    }

    public void DestroyObject()
    {
        gameObject.SetActive(false);
        transform.LookAt(Camera.main.transform);
    }

    public void GetObjectAnim()
    {
        if (ObjectAnim.Length!=0)
        {
            if (isLoopAnim)
            {
                ObjectAnim[0].SetActive(true);
            }
            else
            {
                ObjectAnim[1].SetActive(true);
            }
        }
    }
}
