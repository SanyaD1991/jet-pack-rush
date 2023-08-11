using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour
{
    public bool isLookCamera;
    public Transform transformHeals;
    public Transform transformFuel;
    public Transform transformFuelBuster;
    private Transform target;
    [SerializeField] private Animation hitAnim;
    void Start()
    {       
        target = Camera.main.gameObject.transform;
    }
    public void DestroyLabel()
    {
        Destroy(gameObject);
    }
    void Update()
    {
        if (isLookCamera)
        {
            transform.LookAt(target);
        }
        else
        {
            enabled = false;
        }    
    }
    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    public void hit()
    {
        if (hitAnim!=null) 
        {
            hitAnim.Play();
        }
    }
}
