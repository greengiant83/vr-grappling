using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfter : MonoBehaviour
{
    public float KillTimeInSeconds = 1;
    public float Variance = 0;
	
	void Start ()
    {
        var time = KillTimeInSeconds + Random.Range(-Variance, Variance);
        Invoke("Suicide", time);
	}
	
    void Suicide()
    {
        Destroy(this.gameObject);
    }
}
