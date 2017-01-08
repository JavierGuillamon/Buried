using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumDetect : MonoBehaviour {

    public ControlManager control;

    [Header("Draw box gizmos")]
    public BoxCollider2D col;
    public Vector2 pos;
    public Vector2 size;
    public Color color= new Color(1, 0, 0, .5f);
	// Use this for initialization
	void Start () {
       // pos = transform.position;
        //size = new Vector2(col.size.y, col.size.x);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coffin")
        {
            GameObject coffin = other.gameObject;
            coffin.GetComponent<Rigidbody2D>().constraints= RigidbodyConstraints2D.FreezePosition;
            control.setResize(false);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Coffin")
        {
            GameObject coffin = other.gameObject;
            coffin.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            control.setResize(true);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(pos,size);
    }

}
