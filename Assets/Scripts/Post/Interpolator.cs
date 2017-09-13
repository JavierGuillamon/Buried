using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolator : MonoBehaviour {


    public float speed = 3;
    public Transform target;
    Vector3 offset;

    void Start () {
        offset = transform.position - target.position;
        transform.SetParent(null);
    }
	
	void Update () {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime* speed);
	}
}
