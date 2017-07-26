using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {

    public bool moving;
    public bool sombra;
    public bool swing;
    public bool climb;
    public bool resize;
    //public Chain chainScript;
    public Rigidbody2D rb2d;

    public bool coffinInPlace=false;
    public bool playerInPlace=false;
	
	void Update () {
        //moving
       // if(resize)
            //chainScript.setResize(true);
        //else
            //chainScript.setResize(false);
        if (moving)
        {
            //chainScript.setResize(true);
            resize = true;
            rb2d.isKinematic = true;
            GetComponent<Player>().enabled = true;
            GetComponent<PlayerController>().enabled = false;
        }
        //swing
        if(swing)
        {
            //chainScript.setResize(false);
            resize = false;
            rb2d.isKinematic = false;
            GetComponent<Player>().enabled = false;
            GetComponent<PlayerController>().enabled = true;
        }

        if (sombra)
        {
            //chainScript.setResize(false);
            resize = false;
            rb2d.isKinematic = false;
            GetComponent<Player>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
        }

        if (climb)
        {
            //chainScript.setResize(true);
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
            //GetComponent<Player>().enabled = true;
        }
	}


    //GETTERS && SETTERS
    public void setMoving(bool aux)
    {
        moving = aux;
        //swing = !aux;
        //sombra = !aux;
    }

    public void setSwing(bool aux)
    {
        swing = aux;
        moving = !aux;
    }

    public void setSombra(bool aux)
    {
        sombra = aux;
       // swing = !aux;
       // moving = !aux;

    }

    public void setClimb(bool aux)
    {
        climb = aux;
        moving = !aux;
    }

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
        if ((playerInPlace == true) && (coffinInPlace == true))
            return true;
        else
            return false;
    }
}
