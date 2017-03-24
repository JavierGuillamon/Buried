using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombraOnRails : MonoBehaviour {
    public Transform pointLeft;
    public Transform pointRight;
    public Transform pointTop;
    public Transform pointDown;
    private SombraMov sombra;
    public Vector3 dir;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        sombra = other.GetComponent<SombraMov>();
        sombra.setTragets(pointLeft, pointRight, pointTop, pointDown,dir);
    }
    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + dir, Color.red);
    }
}
