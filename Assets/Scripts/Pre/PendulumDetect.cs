using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumDetect : MonoBehaviour {

    public ControlManager control;

    [Header("Draw box gizmos")]
    public BoxCollider2D col;
    public Vector2 offset;
    public Vector2 size;
    public Color color= new Color(1, 0, 0, .5f);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coffin")
        {
            GameObject coffin = other.gameObject;
            coffin.GetComponent<Rigidbody2D>().constraints= RigidbodyConstraints2D.FreezePosition;
            control.setSwing(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Coffin")
        {
            GameObject coffin = other.gameObject;
            coffin.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            control.setSwing(false);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube((Vector2)transform.position+offset,size);
    }

}
