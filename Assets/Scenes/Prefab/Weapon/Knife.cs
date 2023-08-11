using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [HideInInspector] public bool isPlayer;
    [SerializeField] private SphereCollider sphereCollider;
    private bool isPausaHit;   

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {  
        if (other.gameObject.CompareTag("Body"))
        {
            if (other.gameObject.GetComponentInParent<Instructions>() != null)
            {
                if (other.gameObject.GetComponentInParent<Indicators>().isPlayer != isPlayer)
                {
                    if (!isPausaHit)
                    {
                        StartCoroutine(PausaHit(other));
                    }          
                }
            }
        }
    }
    private IEnumerator PausaHit(Collider collider)
    {

        isPausaHit = true;
        collider.gameObject.GetComponentInParent<Indicators>().Hit(transform.forward);
        sphereCollider.enabled = true;
        yield return new WaitForSeconds(1);
        sphereCollider.enabled = false;
        isPausaHit = false;
    }
}
