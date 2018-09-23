using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    bool wasOnGround;
    public bool isOnGround;

    float debounceTime = 5;
    bool isDebouncing = false;

	// Use this for initialization
	void Start () {
        isOnGround = wasOnGround = true;
        debounce();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isDebouncing) return;

        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hitInfo, 0.1f) && hitInfo.collider.gameObject.tag == "Ground")
            isOnGround = true;
        else
            isOnGround = false;

        if (!isOnGround && wasOnGround) GameManager.Instance.StartGame();
        if (isOnGround && !wasOnGround) GameManager.Instance.EndGame(true);
        if (isOnGround != wasOnGround) debounce();
        wasOnGround = isOnGround;
	}

    void debounce()
    {
        isDebouncing = true;
        Invoke("disableDebounce", debounceTime);
    }

    void disableDebounce()
    {
        isDebouncing = false;
    }
}
