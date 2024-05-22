using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for each wall object (normal, ice, fire)
public class Box : MonoBehaviour
{
    // holds which type of wall it is
    public bool isIce = false;

    public bool isFire = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // melts the ice block
    public IEnumerator melt() {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Melt");
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }

}
