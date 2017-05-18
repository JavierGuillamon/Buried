using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlacaPresion : MonoBehaviour {

    public Animator anim;
    public string targetTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            anim.SetBool("Permiso", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            anim.SetBool("Permiso", false);
        }
    }
}
