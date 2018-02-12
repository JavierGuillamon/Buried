using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinOrientation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float angle = Vector3.Angle(Vector3.up, transform.parent.up);
        angle = Mathf.Abs(angle) * -1;
        transform.localEulerAngles = new Vector3(angle,0,0);
	}
}
