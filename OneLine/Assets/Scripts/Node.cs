using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for each node in the path
public class Node : MonoBehaviour
{
    public Renderer rend;

    private Vector3 mousePosition;

    private GameObject path;

    private Path pathFuns;

    
    // Start is called before the first frame update
    void Start()
    {
        // hides node on start
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        path = this.gameObject.transform.parent.gameObject;
        pathFuns = path.GetComponent<Path>();
        
    }

    // debug function
    public void changeline() {
        Debug.Log("ping");
    }

    public void ping() {
        Debug.Log("ping");
    }


    // shows node when mouse is over
    void OnMouseOver() {
        rend.enabled = true;
    }

    // hides node when mouse is not over
    void OnMouseExit() {
        rend.enabled = false;
        // cleaner line, see Path.cs
        pathFuns.updateall();
    }

    // drag function
    // see path.cs for more info
    void OnMouseDrag() {
        transform.position = pathFuns.checkDrag(this); 
    }

    // simulates drag for path.cs function
    public void simulateDrag() {
        Vector3 pos = transform.position;
        pos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
