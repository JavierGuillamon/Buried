using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaMovNormal : MonoBehaviour {

    [SerializeField]
    private ControlChainSpeed control;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            control.setInNormalDetect(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            control.setInNormalDetect(false);
        }
    }
}
