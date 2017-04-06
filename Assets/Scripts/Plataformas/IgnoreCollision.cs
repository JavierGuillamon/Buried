using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

    public Collider2D col;

    void OnTriggerEnter2D(Collider2D other)
    {
        Physics2D.IgnoreCollision(col, other,true);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Physics2D.IgnoreCollision(col, other,false);
    }
}
