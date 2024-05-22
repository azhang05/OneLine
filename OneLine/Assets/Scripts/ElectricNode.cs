using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricNode : MonoBehaviour
{
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            Debug.Log("hit electric");
            Player player = other.gameObject.GetComponent<Player>();
            player.onElectric = true;
            if (player.onFire || player.fire) {
                player.onFire = false;
                player.fire = false;
                explosion.SetActive(true);
                StartCoroutine(Explode(1f));
            }
        }
        else if (other.gameObject.tag == "Jumper") {
            PlayerJump jumper = other.gameObject.GetComponent<PlayerJump>();
            Player player = jumper.playerscript;
            player.onElectric = true;
            if (player.onFire || player.fire) {
                player.onFire = false;
                player.fire = false;
                explosion.SetActive(true);
                StartCoroutine(Explode(1f));
            }
        }   
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            Player player = other.gameObject.GetComponent<Player>();
            player.onElectric = false;
            player.electric = true;
            player.electricTimer = 3;
        }
        else if (other.gameObject.tag == "Jumper") {
            PlayerJump jumper = other.gameObject.GetComponent<PlayerJump>();
            Player player = jumper.playerscript;
            player.onElectric = false;
            player.electric = true;
            player.electricTimer = 3;
        }
    }

    IEnumerator Explode(float delayTime)
    {
        Debug.Log("BOOM!!!");
        yield return new WaitForSeconds(delayTime);
        explosion.SetActive(false);
    }
}
