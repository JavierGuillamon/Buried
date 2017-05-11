using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public GameObject platform;
    public GameObject platformContainer;
    public float moveSpeed;
    public Transform currentPoint;
    public Transform[] points;
    public int pointSelection = 0;
    
	void Start () {
        currentPoint = points[pointSelection];
	}
	
	void Update () {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, moveSpeed * Time.deltaTime);
        platformContainer.transform.position= Vector3.MoveTowards(platform.transform.position, currentPoint.position, moveSpeed * Time.deltaTime);
        if (platform.transform.position== currentPoint.position)
        {
            pointSelection++;
            if (pointSelection == points.Length)
                pointSelection = 0;
            currentPoint = points[pointSelection];
        }
    }
   
}
