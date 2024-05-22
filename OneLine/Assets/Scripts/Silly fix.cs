using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sillyfix : MonoBehaviour
{

    public GameObject blk1;

    public GameObject blk2;

    public GameObject blk3;
    // Start is called before the first frame update
    void Start()
    {
        blk1.SetActive(true);
        blk2.SetActive(true);
        blk3.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator something() {
        yield return new WaitForSeconds(1);
        blk1.SetActive(true);
        blk2.SetActive(true);
        blk3.SetActive(true);

    }
}
