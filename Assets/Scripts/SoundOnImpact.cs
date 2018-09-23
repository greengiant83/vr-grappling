using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoundOnImpact : MonoBehaviour
{
    Rigidbody body;
    public AudioClip Audio;
    public AudioSource AudioSource;
    public GameObject SpawnPrefab;
    float lastSpawnTime;
    float minSpawnTime = 0.5f;

	void Start ()
    {
        body = GetComponent<Rigidbody>();
        
	}
	
	void Update ()
    {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        //var strength = collision.relativeVelocity.magnitude;
        //var minStrength = 10f;
        //var maxStrength = 35f;

        var strength = collision.impulse.magnitude;
        var minStrength = 80f;
        var maxStrength = 400f;

        if (strength > minStrength)
        {
            //Debug.Log(strength);
            if (Time.fixedTime - lastSpawnTime > minSpawnTime)
            {
                AudioSource.PlayOneShot(Audio, strength.Remap(minStrength, maxStrength, 0.25f, 1, true));
                if (SpawnPrefab != null)
                {
                    Instantiate<GameObject>(SpawnPrefab);
                    lastSpawnTime = Time.fixedTime;
                    minSpawnTime = Random.Range(0.1f, 0.5f);
                    GameManager.Instance.AddPoints(-5);
                }
            }
        }
    }
}
