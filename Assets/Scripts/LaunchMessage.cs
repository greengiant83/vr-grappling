using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMessage : MonoBehaviour
{
    Transform eyes;
    Rigidbody body;
    Rigidbody playerBody;
	
	void Start ()
    {
        eyes = GameObject.Find("Camera (eye)").transform;
        body = GetComponent<Rigidbody>();
        playerBody = GameObject.Find("Player").GetComponent<Rigidbody>();
        launch();
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space)) launch();
	}

    void launch()
    {
        transform.position = eyes.TransformPoint(0, 0, 1) + Random.onUnitSphere * 0.5f;
        transform.LookAt(eyes);
        //transform.Rotate(Vector3.up, 180);

        body.velocity = eyes.forward * 3 + playerBody.velocity + Random.insideUnitSphere * 1f;
        body.angularVelocity = Random.onUnitSphere;

    }
}
