using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaMovSlow : MonoBehaviour
{
    [SerializeField]
    private ControlChainSpeed control;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            control.setInSlowDetect(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            control.setInSlowDetect(false);
        }
    }
}