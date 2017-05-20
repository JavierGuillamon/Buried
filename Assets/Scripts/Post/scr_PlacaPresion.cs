using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlacaPresion : MonoBehaviour {

    public Animator anim;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetBool("Permiso", true);
        }
        if (collision.tag == "Coffin")
        {
            anim.SetBool("Permiso", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetBool("Permiso", false);
        }
        if (collision.tag == "Coffin")
        {
            anim.SetBool("Permiso", false);
        }
    }
}
