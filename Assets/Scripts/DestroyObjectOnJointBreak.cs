using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectOnJointBreak : MonoBehaviour
{
    public GameObject[] ObjectsToDestroy;

    BallSpawner spawner;

	void Start ()
    {
        spawner = transform.parent.parent.GetComponent<BallSpawner>();
	}
	
	void Update ()
    {		
	}

    private void OnJointBreak(float breakForce)
    {
        if(ObjectsToDestroy != null) foreach(var item in ObjectsToDestroy) Destroy(item);
        spawner.OnLinkSnapped();
    }
}
