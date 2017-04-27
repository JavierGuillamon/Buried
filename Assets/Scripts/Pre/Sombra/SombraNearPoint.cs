using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombraNearPoint : MonoBehaviour {
    public Transform pointLeft;
    public Transform pointRight;
    public Transform pointTop;
    public Transform pointDown;
    public float distance;
    public Transform pointLeftD;
    public Transform pointRightD;
    public Transform pointTopD;
    public Transform pointDownD;
    public Vector3 dir;
    public GameObject Coffin;
    private bool touch=false;
	void Update () {
        check();
	}
    
    void check()
    {
       // Debug.Log("DIST: "+Vector2.Distance(transform.position, Coffin.transform.position));
        if (Vector2.Distance(transform.position, Coffin.transform.position)<=distance)
        {
            touch = !touch;
            if (!touch) Coffin.GetComponent<SombraMov>().setTragets(pointLeft, pointRight, pointTop, pointDown,dir);
            else Coffin.GetComponent<SombraMov>().setTragets(pointLeftD, pointRightD, pointTopD, pointDownD,dir);
        }
    }
    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position+dir, Color.red);
    }
}
