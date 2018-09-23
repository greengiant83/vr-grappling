using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMachine : MonoBehaviour {
    public Transform Subject;
    public Rigidbody SubjectBody;
    public AudioClip[] Sounds;
    AudioSource speaker;

    

	// Use this for initialization
	void Start () {
        speaker = GetComponent<AudioSource>();
        
        //InvokeRepeating("Tick", 0, 0.5f);
        Tick();
	}
	
	void Tick ()
    {
        var localPoint = transform.InverseTransformPoint(Subject.position);
        //var index = (int)localPoint.y.Remap(-0.5f, 0.5f, 0, Sounds.Length - 1, true);
        var index = (int)localPoint.z.Remap(-0.5f, 0.5f, 0, Sounds.Length - 1, true);
        if (index < Sounds.Length) speaker.PlayOneShot(Sounds[index], 1);

        //var delay = SubjectBody.velocity.magnitude.Remap(50, 0, 0.1f, 2f, true);
        var delay = localPoint.x.Remap(-0.5f, 0.5f, 0.1f, 1f, true);
        Debug.Log(SubjectBody.velocity.magnitude);
        Invoke("Tick", delay);
	}
}
