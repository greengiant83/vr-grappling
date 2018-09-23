using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchpadWalk : MonoBehaviour
{
    SteamVR_TrackedController controller;
    GameObject player;
    Rigidbody playerBody;
    Transform playerHead;

    void Start ()
    {
        player = GameObject.Find("Player");
        playerBody = player.GetComponent<Rigidbody>();
        playerHead = GameObject.Find("Camera (eye)").transform;

        controller = gameObject.GetComponent<SteamVR_TrackedController>();
    }
	
	void Update ()
    {
        var device = SteamVR_Controller.Input((int)controller.controllerIndex);
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            var touchPoint = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            var forward = new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized;
            var right = new Vector3(playerHead.right.x, 0, playerHead.right.z).normalized;
            var forwardForce = touchPoint.y * forward;
            var lateralForce = touchPoint.x * right;
            var force = (forwardForce + lateralForce) * 200;
            Debug.DrawRay(player.transform.position, force, Color.green);
            playerBody.AddForce(force);
            
        }
    }
}
