using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Rigidbody ConnectedBodyFront;
    public Vector3 ConnectionPointFront = new Vector3(0, 0, 0);

    public Rigidbody ConnectedBodyBack;
    public Vector3 ConnectionPointBack = new Vector3(0, 0, 0);

    public bool AutoConfigureLength = true;
    public float Length = 1;

    ConfigurableJoint jointA;
    ConfigurableJoint jointB;

    float oldLength;

    private void Start()
    {
        if (AutoConfigureLength) Length = Vector3.Distance(ConnectedBodyFront.transform.position, ConnectedBodyBack.transform.position);
        //TODO: when instantiating the ropes orientation should align with how it will be actually be connected
        this.transform.position = Vector3.Lerp(ConnectedBodyFront.position, ConnectedBodyBack.position, 0.5f);
        this.transform.localScale = new Vector3(0.1f, 0.1f, Length);
        this.transform.LookAt(ConnectedBodyFront.position);

        jointA = gameObject.AddComponent<ConfigurableJoint>();
        jointA.xMotion = ConfigurableJointMotion.Locked;
        jointA.yMotion = ConfigurableJointMotion.Locked;
        jointA.zMotion = ConfigurableJointMotion.Locked;
        jointA.autoConfigureConnectedAnchor = false;
        jointA.connectedBody = ConnectedBodyFront;
        jointA.anchor = new Vector3(0, 0, 0.5f);
        jointA.connectedAnchor = ConnectionPointFront;

        jointB = gameObject.AddComponent<ConfigurableJoint>();
        jointB.xMotion = ConfigurableJointMotion.Locked;
        jointB.yMotion = ConfigurableJointMotion.Locked;
        jointB.zMotion = ConfigurableJointMotion.Locked;
        jointB.autoConfigureConnectedAnchor = false;
        jointB.connectedBody = ConnectedBodyBack;
        jointB.anchor = new Vector3(0, 0, -0.5f);
        jointB.connectedAnchor = ConnectionPointBack;

        

        

        oldLength = Length;
    }

    void Update ()
    {
        if(Length != oldLength)
        {
            this.transform.localScale = new Vector3(0.1f, 0.1f, Length);
            jointA.connectedAnchor = ConnectionPointFront;
            jointB.connectedAnchor = ConnectionPointBack;
        }
	}
}


//public class Rope : MonoBehaviour
//{
//    public Transform EndPointA;
//    public Transform EndPointB;

//    private void Start()
//    {
//        transform.SetParent(null);
//    }

//    void Update()
//    {
//        transform.position = Vector3.Lerp(EndPointA.position, EndPointB.position, 0.5f);
//        transform.LookAt(EndPointA);
//        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(EndPointA.position, EndPointB.position));
//    }
//}
