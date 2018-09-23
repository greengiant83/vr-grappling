using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollider : MonoBehaviour {
    public GameObject Target;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Proxy Body")
        {
            Target.SendMessage("OnTargetHit");
        }
    }
}
