using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTraslation : MonoBehaviour {

    public GameObject prefabSpawner;
    public float speed;
    public float distance;
    public float timeToDestroy;

    private Vector3 startPos;
    private Vector3 finalPos;
    private float lifeTime;

	void Start ()
    {
        startPos = transform.position;
        finalPos = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
	}
	
	void Update ()
    {
        lifeTime += Time.deltaTime;
        transform.position = Vector3.Lerp(startPos, finalPos, lifeTime * speed);
        if (lifeTime > timeToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
