using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject RopePrefab;
    public bool isHooked;

    static List<GrapplingHook> grapplingHooks = new List<GrapplingHook>();

    SteamVR_TrackedController controller;
    Transform launchPoint;
    Transform attachmentPoint;
    Transform playerHead;
    Rigidbody hook;
    SpringJoint spring;
    GameObject player;
    Rigidbody playerBody;
    Cord rope;
    BodyController bodyController;
    FixedJoint hookFixJoint;
    Vector3 originalHandLocalPosition;
    Line grappleLine;
    float originalLength;

    void Start()
    {
        grapplingHooks.Add(this);

        player = GameObject.Find("Player");
        bodyController = player.GetComponent<BodyController>();
        playerBody = player.GetComponent<Rigidbody>();
        playerHead = GameObject.Find("Camera (eye)").transform;
        attachmentPoint = transform.Find("Attachment Point");
        launchPoint = transform.Find("Launch Point");
        hook = launchPoint.Find("Hook").GetComponent<Rigidbody>();
        grappleLine = transform.Find("Line").GetComponent<Line>();

        controller = transform.parent.gameObject.GetComponent<SteamVR_TrackedController>();

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

    void Update ()
    {
        if (isHooked)
        {
            //var delta = Vector3.Distance(controller.transform.localPosition, originalHandPosition);
            //rope.Length = originalLength - delta * 20;

            var originalHandPosition = controller.transform.parent.TransformPoint(originalHandLocalPosition);
            var originalVector = hook.transform.position - originalHandPosition;
            var currentVector = hook.transform.position - controller.transform.position;
            var pull = (Vector3.Dot(originalVector.normalized, currentVector) - originalVector.magnitude);
            
            //if (pull < 0) pull = 0;
            rope.Length = originalLength - pull*15;
            grappleLine.Thickness = pull.Remap(1, 0, 0.005f, 0.02f, true);
            //var projection = originalVector.normalized * -pull;
            //Debug.DrawRay(originalHandPosition, originalVector, Color.black);
            //Debug.DrawRay(controller.transform.position, currentVector, Color.yellow);
            //Debug.DrawRay(originalHandPosition, projection, Color.red);

        }

    }

    void fireHook()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(new Ray(launchPoint.position, launchPoint.forward), out hitInfo) && hitInfo.collider.gameObject.tag != "Ground")
        {
            hook.transform.SetParent(null);
            hook.transform.position = hitInfo.point;

            if(hitInfo.rigidbody != null)
            {
                hook.isKinematic = false;
                hookFixJoint = hook.gameObject.AddComponent<FixedJoint>();
                hookFixJoint.connectedBody = hitInfo.rigidbody;
            }
            else
            {
                hook.isKinematic = true;
            }

            bodyController.ToggleProxyBody(true);

            originalLength = Vector3.Distance(hook.transform.position, attachmentPoint.position);
            originalHandLocalPosition = controller.transform.localPosition;

            rope = Instantiate<GameObject>(RopePrefab).GetComponent<Cord>();
            rope.ConnectedBodyFront = hook;
            rope.ConnectionPointFront = Vector3.zero;
            rope.ConnectedBodyBack = bodyController.proxyBody;
            rope.ConnectionPointBack = Vector3.zero;
            rope.Length = originalLength; //= Vector3.Distance(rope.ConnectedBodyBack.transform.position, rope.ConnectedBodyFront.transform.position) * 0.5f;
            rope.AutoConfigureLength = false;
            isHooked = true;
        }
    }

    void detachHook()
    {
        if (!isHooked) return;
        isHooked = false;
        hook.transform.SetParent(launchPoint);
        hook.transform.localPosition = Vector3.zero;
        hook.isKinematic = true;
        if (rope) Destroy(rope.gameObject);
        if (hookFixJoint) Destroy(hookFixJoint);

        bool anyHooksStillAttached = false;
        foreach(var grapple in grapplingHooks)
        {
            if(grapple.isHooked)
            {
                anyHooksStillAttached = true;
                break;
            }
        }
        if(!anyHooksStillAttached) bodyController.ToggleProxyBody(false);
    }
}
