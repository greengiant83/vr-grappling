using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAudioRandom : MonoBehaviour
{
	// Use this for initialization
	void Start () {
        Invoke("StartAudio", Random.Range(0, 5));
	}

    void StartAudio()
    {
        GetComponent<AudioSource>().Play();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
