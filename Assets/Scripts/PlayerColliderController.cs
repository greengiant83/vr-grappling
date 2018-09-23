using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    GameObject colliderObject;

    CapsuleCollider collider;
    Transform eyes;

	void Start ()
    {
        eyes = GameObject.Find("Camera (eye)").transform;
        colliderObject = transform.Find("Proxy Body").gameObject;
        collider = colliderObject.GetComponent<CapsuleCollider>();
    }
	
	void Update ()
    {
        //playerCollider.center = new Vector3(eyes.localPosition.x, eyes.localPosition.y - (playerCollider.height / 2 - playerCollider.radius), eyes.localPosition.z);
        //collider.center = new Vector3(eyes.localPosition.x, collider.center.y, eyes.localPosition.z);
        colliderObject.transform.localPosition = new Vector3(eyes.localPosition.x, colliderObject.transform.localPosition.y, eyes.localPosition.z);
    }
}
