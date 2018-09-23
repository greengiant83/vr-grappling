using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDotProduct : MonoBehaviour
{
    public Transform MainObject;
    public Transform SecondaryObject;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        var mainVector = MainObject.localPosition;
        var secondaryVector = SecondaryObject.localPosition;
        var dot = Vector3.Dot(mainVector.normalized, secondaryVector);
        var projection = mainVector.normalized * dot;

        Debug.DrawLine(transform.position, MainObject.position, Color.yellow);
        Debug.DrawLine(transform.position, SecondaryObject.position, Color.black);
        Debug.DrawRay(transform.position, projection, Color.cyan);
    }
}
