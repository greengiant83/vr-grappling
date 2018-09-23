using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetRingNav : MonoBehaviour
{
    public float Speed = 0.01f;
    public float TowerHeight = 0.5f;
    public GameObject ScoreBonusPrefab;

    Transform Ring;
    Transform Tiles;
    Quaternion rotOffset;
    AudioSource dingSound;
    Vector3 targetPosition;
    Vector3 ringTargetLocalPosition;
    Quaternion targetRot;
    NavMeshAgent navAgent;

    bool wasTraveling;
    bool isRotating;
    

	void Start ()
    {
        Ring = transform.Find("Ring");
        Tiles = GameObject.Find("Tiles").transform;
        rotOffset = Quaternion.AngleAxis(90, Vector3.up);
        dingSound = GetComponent<AudioSource>();
        navAgent = GetComponent<NavMeshAgent>();
        //pickNewTarget();

        //InvokeRepeating("pickNewTarget", 0, 10f);
        //InvokeRepeating("spewPoints", 0, 3f);
    }

    private void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, targetPosition, Speed);
        Ring.rotation = Quaternion.Slerp(Ring.rotation, targetRot, 0.01f);
        Ring.localPosition = Vector3.Lerp(Ring.localPosition, ringTargetLocalPosition, 0.01f);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            pickNewTarget();
        }

        var isTraveling = navAgent.remainingDistance > 0.01f;
        if(!isTraveling && wasTraveling)
        {
            //Debug.Log("Arrived " + transform.position);
        }
        wasTraveling = isTraveling;
    }

    void OnTargetHit()
    {
        spewPoints();
        pickNewTarget();
    }

    void spewPoints()
    {
        GameManager.Instance.AddPoints(100);
        for(int i=0;i<5;i++)
        {
            var scoreClone = Instantiate<GameObject>(ScoreBonusPrefab);
        }
        dingSound.PlayOneShot(dingSound.clip);
    }

    void pickNewTarget()
    {
        var tile = Tiles.GetChild(Random.Range(0, Tiles.childCount));
        var trees = new List<Transform>();
        foreach(Transform child in tile)
        {
            if (child.gameObject.name != "Ground" && child.gameObject.name != "Platform") trees.Add(child);
        }

        int indexA = Random.Range(0, trees.Count);
        int indexB;
        do { indexB = Random.Range(0, trees.Count); } while (indexA == indexB);

        var treeA = trees[indexA];
        var treeB = trees[indexB];
        var position = Vector3.Lerp(treeA.position, treeB.position, 0.5f);
        
        var topA = treeA.TransformPoint(new Vector3(0, 0, TowerHeight)).y;
        var topB = treeB.TransformPoint(new Vector3(0, 0, TowerHeight)).y;
        var maxTop = topA > topB ? topB : topA;
        var bottom = treeA.position.y + 2;
        position.y = Random.Range(bottom, maxTop); // 50
        ringTargetLocalPosition = Vector3.up * (position.y - 50);
        targetPosition = position;
        navAgent.SetDestination(new Vector3(targetPosition.x, 50, targetPosition.z));


        var angleVector = treeA.position - treeB.position;
        angleVector.y = 0;
        //transform.rotation = Quaternion.LookRotation(angleVector) * rotOffset;
        targetRot = Quaternion.LookRotation(angleVector) * rotOffset;
    }
}
