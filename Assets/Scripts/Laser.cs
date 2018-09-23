using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float Thickness = 0.01f;
    MeshRenderer beam;
    public SteamVR_TrackedController controller;
    SteamVR_Controller.Device controllerDevice;
    bool wasHitting;
    Collider lastHitCollider;

    void Start ()
    {
        beam = transform.Find("Beam").GetComponent<MeshRenderer>();
    }
	
	void Update ()
    {
        if (controllerDevice == null && controller != null) controllerDevice = SteamVR_Controller.Input((int)controller.controllerIndex);

        RaycastHit hitInfo;
        bool isHitting = Physics.Raycast(new Ray(transform.position, transform.forward), out hitInfo);
        if (isHitting)
        {
            transform.localScale = new Vector3(Thickness, Thickness, hitInfo.distance);
            beam.enabled = true;
            //if(lastHitCollider != hitInfo.collider) controllerDevice.TriggerHapticPulse(500);
            lastHitCollider = hitInfo.collider;
        }
        else beam.enabled = false;

        //if (controllerDevice != null && isHitting && !wasHitting) controllerDevice.TriggerHapticPulse(500);
        //if (controllerDevice != null && !isHitting && wasHitting) controllerDevice.TriggerHapticPulse(200);

        wasHitting = isHitting;
    }
}
