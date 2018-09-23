using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour {
    Transform player;
    Vector3 resetPosition;
    Quaternion resetRotation;

	void Start ()
    {
        player = GameObject.Find("Player").transform;
        resetPosition = player.position;
        resetRotation = player.rotation;
	}
	
	void Update ()
    {		
	}

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            player.position = resetPosition;
            player.rotation = resetRotation;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
