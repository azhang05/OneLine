using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// script for each switch object
public class Switch : MonoBehaviour
{

    public GameObject door;

    GameObject on;
    GameObject off;


    // Start is called before the first frame update
    void Start()
    {
        on = transform.GetChild(0).gameObject;
        off = transform.GetChild(1).gameObject;
        on.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // called by player
    // switches the door
    // sets button to on
    public void Hit() {
        door.SetActive(false);
        on.SetActive(true);
        off.SetActive(false);

    }

}
