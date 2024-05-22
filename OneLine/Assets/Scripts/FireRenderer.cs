using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* REMOVE THIS SCRIPT FROM THE LAST FIRE NODE IN A SEQUENCE, OR THE RENDERER WILL NOT WORK */
// script for rendering the fire path
// put script on a fire node, then attach the next fire node to the nextFire variable
// as stated above, script should not be on the last fire node
// only handles rendering, see Path.cs for more info on path logic
public class FireRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextFire;

    LineRenderer lineRenderer;

    void Start()
    {

        // sets up line renderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.08f;
        lineRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2;
        lineRenderer.sortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // sets line renderer positions (current and next fire node)
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, nextFire.transform.position);
    }
}
