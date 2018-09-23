using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayer : MonoBehaviour
{
    AudioSource windAudio;
    Rigidbody playerBody;
    
    void Start ()
    {
        windAudio = GetComponent<AudioSource>();
        playerBody = GameObject.Find("Player").GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
        var speed = playerBody.velocity.magnitude;
        if (speed > 0)
        {
            windAudio.volume = speed.Remap(5, 20, 0, 1, true);
            if (!windAudio.isPlaying) windAudio.Play();
        }
    }
}
