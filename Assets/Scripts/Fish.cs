using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fish : MonoBehaviour
{
    public Transform LeftEye;
    public Transform RightEye;
    public Animator animator;

    NavMeshAgent agent;
    Ray ray;
    Transform player;
    Quaternion eyeOffset;
    float sizeDif;

	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Proxy Body").transform;
        animator.SetFloat("Animation Offset", Random.value);
        

        InvokeRepeating("UpdatePath", 0, 0.15f);
        

        float spread = 0.75f;
        float originalScale = transform.localScale.x;
        agent.speed *= Random.Range(1 - spread, 1 + spread);
        agent.acceleration *= Random.Range(1 - spread, 1 + spread);
        agent.angularSpeed *= Random.Range(1 - spread, 1 + spread);
        transform.localScale *= Random.Range(1 - spread, 1 + spread);
        sizeDif = transform.localScale.x - originalScale;
        sizeDif = sizeDif.Remap(2, -2, 0.5f, 2f, true);
        animator.SetFloat("Speed", sizeDif);

        eyeOffset = Quaternion.Euler(0, 90, 0);
    }

    void UpdatePath()
    {
        agent.SetDestination(new Vector3(player.position.x, 50, player.position.z));
    }
	
	void Update ()
    {
        //Debug.Log(agent.speed + ": " + agent.desiredVelocity.magnitude);
        LeftEye.LookAt(player.position, eyeOffset);
        RightEye.LookAt(player.position, eyeOffset);
	}
}

public static class HelperClasses
{
    public static void LookAt(this Transform transform, Vector3 target, Quaternion offset)
    {
        Quaternion rot = Quaternion.LookRotation(target - transform.position);
        rot *= offset;
        transform.rotation = rot;
    }
}

