using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {

    public bool resize;
    public Chain chainScript;
    public Rigidbody2D rb2d;
 
	void Start () {
		
	}
	
	void Update () {
        //moving
        if (resize)
        {
            chainScript.setResize(true);
            rb2d.isKinematic = true;
            GetComponent<Player>().enabled = true;
            GetComponent<PlayerController>().enabled = false;
        }
        //swing
        else
        {
            chainScript.setResize(false);
            rb2d.isKinematic = false;
            GetComponent<Player>().enabled = false;
            GetComponent<PlayerController>().enabled = true;
        }
	}


    //GETTERS && SETTERS
    public void setResize(bool aux)
    {
        resize = aux;
    }
}
