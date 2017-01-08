using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchSprite : MonoBehaviour {
    public Transform source;
    public Transform target;
    public bool mirrorZ;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 centerPos = (source.position + target.position) / 2f;
        transform.position = centerPos;
        Vector3 direction = target.position - source.position;
        direction = Vector3.Normalize(direction);
        transform.right = direction;
     
        if (mirrorZ)
            transform.right *= -1f;
        Vector3 scale = new Vector3(1, 1, 1);
        scale.x = Vector3.Distance(source.position, target.position);
        transform.localScale = scale;
	}
}
