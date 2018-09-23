using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSoundOnWake : MonoBehaviour {
    public AudioClip[] Sounds;

	// Use this for initialization
	void Start () {
        var audio = GetComponent<AudioSource>();
        audio.clip = Sounds[Random.Range(0, Sounds.Length)];
        audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
