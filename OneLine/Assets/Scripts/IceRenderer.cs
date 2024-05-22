using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// script for rendering the ice path
// put script on an ice  node, then attach the next ice node to the nextIce variable
// as stated above, script should not be on the last fire node
// only handles rendering, see Path.cs for more info on path logic
public class IceRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextIce;

    LineRenderer lineRenderer;

    void Start()
    {
        // setting up line renderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.08f;
        lineRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.cyan;
        lineRenderer.positionCount = 2;
        lineRenderer.sortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // sets line renderer positions (current and next ice node)
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, nextIce.transform.position);
    }
}
