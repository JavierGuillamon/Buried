using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombraDetect : MonoBehaviour {
    public SombraMov sombramov;
    public ControlManager control;
    public Rigidbody2D playerrb;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coffin")
        {
            other.gameObject.layer = LayerMask.NameToLayer("sombraCoffin");
            control.setSombra(true);
            sombramov.enabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Coffin")
        {
            other.gameObject.layer = LayerMask.NameToLayer("coffin");
            control.setMoving(true);
            sombramov.enabled = false;
            playerrb.velocity = Vector2.zero;
            playerrb.angularVelocity = 0;
        }
    }
}
