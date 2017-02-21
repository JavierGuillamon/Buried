using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombraDetect : MonoBehaviour {
    public SombraMov sombramov;
    public Player player;
    //public ControlManager control;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coffin")
        {
            player.enabled = false;
            //control.setResize(false);
            sombramov.enabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Coffin")
        {
            player.enabled = true;
            //control.setResize(true);
            sombramov.enabled = false;

        }
    }
}
