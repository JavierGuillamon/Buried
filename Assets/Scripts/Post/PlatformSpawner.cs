using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {

    public GameObject platformPrefab;
    public float timeNextSpawn;
    public float distance;

    [HideInInspector]
    public Vector3 originalPos;
    [HideInInspector]
    public Vector3 finalPos;

    private float currentTime;

    private void Start()
    {
        originalPos = transform.position;
        finalPos = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        currentTime = timeNextSpawn;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            Instantiate(platformPrefab, transform.position, Quaternion.identity);
            currentTime = timeNextSpawn;
        }

    }
}
