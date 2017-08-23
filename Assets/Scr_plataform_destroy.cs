using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_plataform_destroy : MonoBehaviour {
    public bool player_inside = false;
    public bool coffin_inside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            player_inside = true;
        else if (collision.tag == "Coffin")
            coffin_inside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            player_inside = false;
        else if (collision.tag == "Coffin")
            coffin_inside = false;
    }
    public void Update()
    {
        if (player_inside && coffin_inside)
            Destroy(gameObject, 3f);
    }
}
