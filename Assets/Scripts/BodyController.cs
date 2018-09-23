using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    Transform eyes;
    GameObject player;
    GameObject proxy;
    CapsuleCollider playerCollider;
    CapsuleCollider proxyCollider;
    CapsuleCollider activeCollider;
    Transform proxyVisualizer;
    Rigidbody playerBody;
    public Rigidbody proxyBody;
    public Rigidbody activeBody;
    AudioSource windAudio;
    bool isProxyEnabled;

	void Start ()
    {
        eyes = GameObject.Find("Camera (eye)").transform;
        player = GameObject.Find("Player");
        proxy = GameObject.Find("Proxy Body");
        playerCollider = player.GetComponent<CapsuleCollider>();
        proxyCollider = proxy.GetComponent<CapsuleCollider>();
        playerBody = player.GetComponent<Rigidbody>();
        proxyBody = proxy.GetComponent<Rigidbody>();
        activeCollider = playerCollider;
        activeBody = playerBody;
        Debug.Log(activeBody);
        proxyVisualizer = GameObject.Find("Proxy Visual").transform;
        windAudio = GetComponent<AudioSource>();

        proxy.transform.SetParent(null);
        proxy.SetActive(false);
	}
	
	void Update ()
    {
        if(!isProxyEnabled)
        {
            //playerCollider.center = new Vector3(eyes.localPosition.x, eyes.localPosition.y - (playerCollider.height / 2 - playerCollider.radius), eyes.localPosition.z);
            playerCollider.center = new Vector3(eyes.localPosition.x, playerCollider.center.y, eyes.localPosition.z);
        }
        else
        {
            //proxyCollider.center = new Vector3(0, -proxyCollider.height / 2 + proxyCollider.radius, 0);
            //proxyVisualizer.localPosition = proxyCollider.center;
            //proxyVisualizer.localScale = new Vector3(proxyCollider.radius * 2, proxyCollider.height / 2, proxyCollider.radius * 2);
        }

        var speed = activeBody.velocity.magnitude;
        if (speed > 0)
        {
            windAudio.volume = speed.Remap(1, 10, 0, 1, true);
            if (!windAudio.isPlaying) windAudio.Play();
        }
        //else windAudio.Stop();
        //start at 1 ramp to 10
	}

    public void ToggleProxyBody(bool isProxyEnabled)
    {
        if (isProxyEnabled == this.isProxyEnabled) return;
        this.isProxyEnabled = isProxyEnabled;
        if(isProxyEnabled)
        {
            //proxy.transform.position = eyes.position;
            proxy.transform.position = eyes.parent.TransformPoint(new Vector3(eyes.localPosition.x, playerCollider.height / 2, eyes.localPosition.z));
            
            proxyBody.velocity = playerBody.velocity;
            player.transform.SetParent(proxy.transform);
            playerBody.isKinematic = true;
            playerCollider.enabled = false;
        }
        else
        {
            playerBody.velocity = proxyBody.velocity;
            player.transform.SetParent(null);
            playerBody.isKinematic = false;
            playerCollider.enabled = true;

        }
        proxy.SetActive(isProxyEnabled);

        activeCollider = isProxyEnabled ? proxyCollider : playerCollider;
        activeBody = isProxyEnabled ? proxyBody : playerBody;
    }
}

public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2, bool clamped)
    {
        var v = (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        if (clamped) v = Mathf.Clamp(v, from2, to2);
        return v;
    }

}
