using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;
    [HideInInspector] public Transform Parent;
    [SerializeField] private Transform PointShot;
    [HideInInspector] public Material materialBullet;
    [SerializeField] private ParticleSystem VFX;
    void Start()
    {
        
    }  
    public void Shot(bool whoseBullet)
    {      
        GameObject bullet = Instantiate(Bullet, PointShot.position, PointShot.rotation);
        bullet.GetComponent<Bullet>().isPlayer = whoseBullet;
        bullet.GetComponent<Bullet>().myMaterial = materialBullet;
        bullet.transform.parent = Parent;
        VFXPlay();
    }
    public void NotShot()
    {
        VFXStop();
    }

    private void VFXStop()
    {
        if (VFX.isPlaying)
        {
            VFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
    private void VFXPlay ()
    {
        if (VFX.isStopped)
        {
            VFX.Play(true);
        }
    }
}
