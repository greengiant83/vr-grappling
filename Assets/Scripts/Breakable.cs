using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject BrokenPrefab;
    Vector3 lastCollisionPoint;
    Vector3 lastCollisionVelocity;

    void Start()
    {
    }

    void Update()
    {
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);
        breakObject();
    }*/

    private void OnJointBreak(float breakForce)
    {
        breakObject(breakForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        lastCollisionVelocity = collision.relativeVelocity;
        lastCollisionPoint = collision.contacts[0].point;
        //collision.collider.transform;
    }

    void breakObject(float force)
    {
        //min 4000
        //max 17000
        Debug.Log("Force: " + force);
        float lowForce = 4000;
        float highForce = 17000;
        float explosiveForce = force.Remap(lowForce, highForce, 500, 2500, true);
        //explosiveForce = 500;
        var brokenObject = Instantiate<GameObject>(BrokenPrefab);
        brokenObject.transform.position = transform.position;
        brokenObject.transform.rotation = transform.rotation;
        brokenObject.transform.localScale = transform.localScale;

        var colliders = Physics.OverlapSphere(lastCollisionPoint, 5f);
        var explosionPoint = lastCollisionPoint - (lastCollisionVelocity.normalized * force.Remap(lowForce, highForce, 1, 3, true)); 
        //high 3, 2500
        foreach(var collider in colliders)
        {
            if(collider.attachedRigidbody) collider.attachedRigidbody.AddExplosionForce(explosiveForce, explosionPoint, 5);
        }

        Destroy(this.gameObject);
        
    }
}
