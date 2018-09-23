using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cord : MonoBehaviour
{
    public Rigidbody ConnectedBodyFront;
    public Vector3 ConnectionPointFront = new Vector3(0, 0, 0);

    public Rigidbody ConnectedBodyBack;
    public Vector3 ConnectionPointBack = new Vector3(0, 0, 0);

    public bool AutoConfigureLength = true;
    public float Length = 1;

    public float Thickness = 0.01f;

    SpringJoint joint;
    float oldLength;

    void Start ()
    {
        if (AutoConfigureLength) Length = Vector3.Distance(ConnectedBodyFront.transform.position, ConnectedBodyBack.transform.position);

        joint = ConnectedBodyFront.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = ConnectedBodyBack;
        joint.anchor = ConnectionPointFront;
        joint.connectedAnchor = ConnectionPointBack;
        joint.maxDistance = Length;
        joint.spring = 10;

        transform.localScale = Vector3.one * Thickness;
        transform.SetParent(null);

        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnDestroy()
    {
        if (joint != null) Destroy(joint);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(ConnectedBodyFront.transform.position, ConnectedBodyBack.transform.position, 0.5f);
        transform.LookAt(ConnectedBodyFront.transform.position);
        transform.localScale = new Vector3(Thickness, Thickness, Vector3.Distance(ConnectedBodyFront.transform.position, ConnectedBodyBack.transform.position));

        if (Length != oldLength)
        {
            if (Length < 0) Length = 0;
            joint.maxDistance = Length;
            oldLength = Length;
        }

        /*var delta = ConnectedBodyFront.transform.position - ConnectedBodyBack.transform.position;
        var deltaLength = delta.magnitude;
        var maxStrength = 20f;
        var strength = deltaLength.Remap(0, Length, 0, maxStrength, true);
        var force = delta.normalized * strength;
        ConnectedBodyBack.AddForce(force);
        ConnectedBodyFront.AddForce(-force);*/
    }
}
