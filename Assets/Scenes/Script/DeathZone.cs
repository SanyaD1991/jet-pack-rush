using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Wall") && other.gameObject.GetComponentInParent<BotControl>()!=null)
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponentInParent<BotControl>().DestroyBot();
        }
    }
}
