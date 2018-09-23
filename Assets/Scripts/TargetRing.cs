using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRing : MonoBehaviour
{
    public float Speed = 0.01f;
    public float TowerHeight = 0.5f;

    Transform Tiles;
    Quaternion rotOffset;
    AudioSource dingSound;
    Vector3 targetPosition;
    Quaternion targetRot;
    

	void Start ()
    {
        Tiles = GameObject.Find("Tiles").transform;
        rotOffset = Quaternion.AngleAxis(90, Vector3.up);
        dingSound = GetComponent<AudioSource>();
        pickNewTarget();

        //InvokeRepeating("pickNewTarget", 0, 2f);
	}

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Speed);
    }

    void OnTriggerEnter(Collider other)
    {
        pickNewTarget();
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
        
        var topA = treeA.TransformPoint(new Vector3(0, TowerHeight, 0)).y;
        var topB = treeB.TransformPoint(new Vector3(0, TowerHeight, 0)).y;
        var maxTop = topA > topB ? topB : topA;
        position.y = Random.Range(2, maxTop);

        //transform.position = position;
        targetPosition = position;

        var angleVector = treeA.position - treeB.position;
        angleVector.y = 0;
        //transform.rotation = Quaternion.LookRotation(angleVector) * rotOffset;
        targetRot = Quaternion.LookRotation(angleVector) * rotOffset;

        dingSound.PlayOneShot(dingSound.clip);
    }
}
