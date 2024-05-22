using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    public GameObject levelCompleteMenuUI;

    void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.tag == "Finish") // checks for node with "Finish" tag
        // {
        //     Debug.Log("hit end");
        //     levelCompleteMenuUI.SetActive(true);
        //     Time.timeScale = 0f;
        // }
    }
}
