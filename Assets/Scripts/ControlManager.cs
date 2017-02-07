using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {

    public bool resize;
    public Chain chainScript;
    public Rigidbody2D rb2d;

    public bool coffinInPlace=false;
    public bool playerInPlace=false;
	
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
        if (coffinInPlace && playerInPlace)
        {
            GetComponent<Player>().enabled = false;
        }
        else
        {
            GetComponent<Player>().enabled = true;
        }
	}


    //GETTERS && SETTERS
    public void setResize(bool aux)
    {
        resize = aux;
    }

    public void setCoffinInPlace(bool aux)
    {
        coffinInPlace = aux;
    }

    public void setPlayerInPlace(bool aux)
    {
        playerInPlace = aux;
    }

    public bool canClimb()
    {
       // Debug.Log(playerInPlace + " " + coffinInPlace);
        if ((playerInPlace == true) && (coffinInPlace == true))
            return true;
        else
            return false;
    }
}
