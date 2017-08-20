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
    [Header("Movimiento circular")]
    public bool circular;
    public Transform center;
    public float degreesPerSecond = -65.0f;
    private Vector3 v1, v2;
    void Start () {
        currentPoint = points[pointSelection];
        if (circular)
        {
            v1 = transform.position - center.position;
           // v2 = platformContainer.transform.position - center.position;
        }
	}
	
	void Update () {
        if (!circular)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, moveSpeed * Time.deltaTime);
            platformContainer.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, moveSpeed * Time.deltaTime);
            if (platform.transform.position == currentPoint.position)
            {
                pointSelection++;
                if (pointSelection == points.Length)
                    pointSelection = 0;
                currentPoint = points[pointSelection];
            }
        }
        else
        {
            v1 = Quaternion.AngleAxis(degreesPerSecond * Time.deltaTime, Vector3.forward) * v1;
            transform.position = center.position + v1;
           // v2 = Quaternion.AngleAxis(degreesPerSecond * Time.deltaTime, Vector3.forward) * v2;
            //platformContainer.transform.position = center.position + v2;
        }
    }
   
}
