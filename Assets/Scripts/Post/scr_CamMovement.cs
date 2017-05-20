using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CamMovement : MonoBehaviour {

    public Transform coffin;
    public Transform player;
    public float percent;
    public float followSpeed;
    public float rotationSpeed;
    public float offsetX;
    public float offsetY;
    [HideInInspector]
    public float offsetZ;
    [HideInInspector]
    public float angleX;
    [HideInInspector]
    public float totalZ;

    private float distance;
    private float scaledDistance;
    private Vector3 difference;
    private Vector3 focusPoint;
    private float currentAngle;


    void Start()
    {
        totalZ = transform.position.z;
    }

    Vector3 temposition;

    void Update()
    {
        focusPoint = Vector3.Lerp(player.position, coffin.position, percent);
        temposition = Vector3.Lerp(temposition, new Vector3(focusPoint.x + offsetX, focusPoint.y + offsetY, totalZ + offsetZ), Time.deltaTime * followSpeed);
        transform.position = Vector3.Lerp(transform.position, temposition, Time.deltaTime * followSpeed);
        Quaternion target = Quaternion.Euler(angleX, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotationSpeed);
    }
}
