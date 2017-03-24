using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombraDetect : MonoBehaviour {
    public SombraMov sombramov;
    public ControlManager control;
    public Rigidbody2D playerrb;
    private Vector3 scale;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coffin")
        {
            other.gameObject.layer = LayerMask.NameToLayer("sombraCoffin");
            scale = other.GetComponent<Transform>().localScale;
            other.GetComponent<Transform>().localScale = new Vector3(1, 1, 0);
            other.GetComponent<Rigidbody2D>().gravityScale = 0;
           // other.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            control.setSombra(true);
            sombramov.enabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Coffin")
        {
            other.gameObject.layer = LayerMask.NameToLayer("coffin");
            other.GetComponent<Transform>().localScale = scale;
            other.GetComponent<Rigidbody2D>().gravityScale = 1;
            control.setMoving(true);
            sombramov.enabled = false;
            playerrb.velocity = Vector2.zero;
            playerrb.angularVelocity = 0;
        }
    }

    
}
