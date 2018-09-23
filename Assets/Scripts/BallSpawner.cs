using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject SpawnPrefab;

	void Start ()
    {
        //InvokeRepeating("respawn", 0, 2);
	}
	
	void Update ()
    {		
	}

    public void OnLinkSnapped()
    {
        Invoke("respawn", 2);
    }

    void respawn()
    {
        var clone = Instantiate<GameObject>(SpawnPrefab);
        clone.transform.SetParent(this.transform);
        clone.transform.localPosition = Vector3.zero;
        
    }
}
