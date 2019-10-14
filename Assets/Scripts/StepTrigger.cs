using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTrigger : MonoBehaviour
{
    public GameObject particle;
    private bool hasExplode = false;

    public void Explode()
    {
        if (hasExplode)
        {
            return;
        }

        hasExplode = true;
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        Vector3 explosionPos = transform.position;

        foreach (Rigidbody rgd in rigidbodies)
        {
            rgd.isKinematic = false;
            rgd.AddExplosionForce(100f, explosionPos, 5f, 3.0F);
        }
      
        Destroy(this.gameObject, 5f);
        Instantiate(particle,transform.position,Quaternion.identity,transform);
    }
}
