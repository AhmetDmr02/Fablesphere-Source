using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSpikeTrigger : EnemyHitCenter
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "DestructableObject")
        {
            other.transform.GetComponent<DestructableObject>().Explode();
        }
        if (other.transform.tag == "Player")
        {
            HitPlayer();
        }
    }
}
