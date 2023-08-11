using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]public bool isPlayer;
    [HideInInspector] public Material myMaterial;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] ParticleSystem WallShotParticle;
    [SerializeField] ParticleSystem BotlShotParticle;
    private bool isMove;
    void Start()
    {
        isMove = true;
        Destroy(gameObject, 2);
    }
    void Update()
    {
        MoveBullet();
    }
    private void MoveBullet()
    {
        if (isMove)
        {
            transform.position += transform.forward * Time.deltaTime * 20;
        }
    }

    public void DestruyBullet()
    {
        GetComponent<SphereCollider>().enabled = false;
        meshRenderer.enabled = false;
        isMove = false;
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.CompareTag("Wall"))
        {
            DestruyBullet();
            WallShotParticle.Play(true);
        }

        if (other.gameObject.CompareTag("Body"))
        {
            if (other.gameObject.GetComponentInParent<Instructions>()!=null) 
            {
                if (other.gameObject.GetComponentInParent<Indicators>().isPlayer != isPlayer)
                {
                    var heading = other.transform.position - transform.position;
                    var distance = heading.magnitude;
                    var direction = heading / distance;                    
                    other.gameObject.GetComponentInParent<Indicators>().Hit(direction);
                    DestruyBullet();
                    BotlShotParticle.Play(true);
                }
            }
          
        }       
    }
}
