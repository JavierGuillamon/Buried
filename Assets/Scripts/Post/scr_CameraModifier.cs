using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CameraModifier : MonoBehaviour {

    public GameObject gameCamera;
    public float newOffsetX;
    public float newOffsetY;
    public float newOffsetZ;
    public float newAngle;
    public float newFollowSpeed;
    public float newRotationSpeed;

    private scr_CamMovement camScr;
    private float oldOffsetX;
    private float oldOffsetY;
    private float oldOffsetZ;
    private float oldAngle;
    private float oldFollowSpeed;
    private float oldRotationSpeed;

    void Start()
    {
        camScr = gameCamera.GetComponent<scr_CamMovement>();
        oldOffsetX = camScr.offsetX;
        oldOffsetY = camScr.offsetY;
        oldOffsetZ = camScr.offsetZ;
        oldAngle = gameCamera.transform.rotation.z;
        oldFollowSpeed = camScr.followSpeed;
        oldRotationSpeed = camScr.rotationSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            camScr.offsetX = newOffsetX;
            camScr.offsetY = newOffsetY;
            camScr.offsetZ = newOffsetZ;
            camScr.angleX = newAngle;
            camScr.followSpeed = newFollowSpeed;
            camScr.rotationSpeed = newRotationSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            camScr.offsetX = oldOffsetX;
            camScr.offsetY = oldOffsetY;
            camScr.offsetZ = oldOffsetZ;
            camScr.angleX = oldAngle;
            camScr.followSpeed = oldFollowSpeed;
            camScr.rotationSpeed = oldRotationSpeed;
        }
    }
}
