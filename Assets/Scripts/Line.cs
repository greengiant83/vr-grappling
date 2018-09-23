using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Transform endPointA;
    public Transform endPointB;
    public float Thickness = 0.01f;
    public bool IsVisible
    {
        get
        {
            if(!renderer) renderer = GetComponent<MeshRenderer>();
            return renderer.enabled;
        }
        set
        {
            if (!renderer) renderer = GetComponent<MeshRenderer>();
            renderer.enabled = value;
        }
    }

    MeshRenderer renderer;

    private void Start()
    {
        transform.localScale = Vector3.one * Thickness;
        transform.SetParent(null);
        renderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(endPointA.position, endPointB.position, 0.5f);
        transform.LookAt(endPointA);
        transform.localScale = new Vector3(Thickness, Thickness, Vector3.Distance(endPointA.position, endPointB.position));
    }
}
