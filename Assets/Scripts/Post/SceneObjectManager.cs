using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectManager : MonoBehaviour {

    public GameObject firstBG;
    public GameObject secondBG;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            firstBG.SetActive(false);
            secondBG.SetActive(true);
        }
    }
}
