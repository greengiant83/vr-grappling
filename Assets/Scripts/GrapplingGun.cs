using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public GameObject CordPrefab;
    public bool isHooked;

    static List<GrapplingGun> grapplingGuns = new List<GrapplingGun>();

    SteamVR_TrackedController controller;
    Transform launchPoint;
    Transform attachmentPoint;
    Transform playerHead;
    Rigidbody hook;
    SpringJoint spring;
    GameObject player;
    Rigidbody playerBody;
    BodyController bodyController;
    FixedJoint hookFixJoint;
    Vector3 originalHandLocalPosition;
    Line grappleLine;
    SteamVR_Controller.Device controllerDevice;
    GameObject laser;
    float originalLength;

    void Start()
    {
        grapplingGuns.Add(this);

        player = GameObject.Find("Player");
        bodyController = player.GetComponent<BodyController>();
        playerBody = player.GetComponent<Rigidbody>();
        playerHead = GameObject.Find("Camera (eye)").transform;
        attachmentPoint = transform.Find("Attachment Point");
        launchPoint = transform.Find("Launch Point");
        hook = launchPoint.Find("Hook").GetComponent<Rigidbody>();
        laser = launchPoint.Find("Laser").gameObject;
        grappleLine = transform.Find("Line").GetComponent<Line>();
        //spring = attachmentPoint.GetComponent<SpringJoint>();
        controller = transform.parent.gameObject.GetComponent<SteamVR_TrackedController>();

        grappleLine.IsVisible = false;

        controller.TriggerClicked += Controller_TriggerClicked;
        controller.TriggerUnclicked += Controller_TriggerUnclicked;
    }

    private void Controller_TriggerUnclicked(object sender, ClickedEventArgs e)
    {
        detachHook();
    }

    private void Controller_TriggerClicked(object sender, ClickedEventArgs e)
    {
        fireHook();
    }

    void FixedUpdate()
    {
        if (controllerDevice == null) controllerDevice = SteamVR_Controller.Input((int)controller.controllerIndex);

        if (isHooked)
        {
            
            playerBody.AddForce(spring.currentForce);
            //bodyController.activeBody.AddForce(spring.currentForce);

            var originalHandPosition = controller.transform.parent.TransformPoint(originalHandLocalPosition);
            var originalVector = hook.transform.position - originalHandPosition;
            var currentVector = hook.transform.position - controller.transform.position;
            var pull = (Vector3.Dot(originalVector.normalized, currentVector) - originalVector.magnitude);
            if (pull < 0) pull = 0;

            var newLength = originalLength - pull * 5;
            if (newLength < 0) newLength = 0;
            spring.maxDistance = newLength;

            var currentLength = Vector3.Distance(attachmentPoint.position, hook.transform.position);
            grappleLine.Thickness = (currentLength - spring.maxDistance).Remap(1, 0, 0.001f, 0.03f, true);

            var hapticStrength = (ushort)(currentLength - spring.maxDistance).Remap(0, 1, 100, 500, true);
            if(hapticStrength > 100) controllerDevice.TriggerHapticPulse(hapticStrength);

            if (currentLength < originalLength) originalLength = currentLength; //constantly takes up the slack in the rope
        }
    }

    void fireHook()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(launchPoint.position, launchPoint.forward), out hitInfo)) // && hitInfo.collider.gameObject.tag != "Ground")
        {
            hook.transform.SetParent(null);
            hook.transform.position = hitInfo.point;

            hook.isKinematic = false;
            hookFixJoint = hook.gameObject.AddComponent<FixedJoint>();

            if (hitInfo.rigidbody != null)
            {
                hookFixJoint.connectedBody = hitInfo.rigidbody;
            }
            else
            {
                hook.transform.SetParent(hitInfo.transform);
            }

            spring = attachmentPoint.gameObject.AddComponent<SpringJoint>();
            spring.spring = 100;
            spring.damper = 0.5f;
            spring.autoConfigureConnectedAnchor = false;
            spring.anchor = Vector3.zero;
            spring.connectedAnchor = Vector3.zero;
            spring.maxDistance = originalLength = Vector3.Distance(attachmentPoint.position, hook.transform.position);
            spring.connectedBody = hook;

            grappleLine.IsVisible = true;
            laser.SetActive(false);

            originalHandLocalPosition = controller.transform.localPosition;

            //bodyController.ToggleProxyBody(true);

            isHooked = true;
        }
    }

    void detachHook()
    {
        if (!isHooked) return;
        if (hookFixJoint != null) Destroy(hookFixJoint);
        if (spring != null) Destroy(spring);
        isHooked = false;
        hook.transform.SetParent(launchPoint);
        hook.transform.localPosition = Vector3.zero;
        hook.isKinematic = true;
        grappleLine.IsVisible = false;
        laser.SetActive(true);

        //bodyController.ToggleProxyBody(false);
    }
}
