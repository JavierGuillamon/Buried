using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CameraModifier : MonoBehaviour {

    public GameObject gameCamera;
    public float newOffsetX;
    public float newOffsetY;
    public float newOffsetZ;
    public float newAngle;

    private scr_CamMovement camScr;
    private bool permiso;
    private float oldOffsetX;
    private float oldOffsetY;
    private float oldOffsetZ;
    private float oldAngle;
    
    void Start () {
        camScr = gameCamera.GetComponent<scr_CamMovement>();
        permiso = false;
        oldOffsetX = camScr.offsetX;
        oldOffsetY = camScr.offsetY;
        oldOffsetZ = camScr.offsetZ;
        oldAngle = gameCamera.transform.rotation.z;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            camScr.offsetX = newOffsetX;
            camScr.offsetY = newOffsetY;
            camScr.offsetZ = newOffsetZ;
            camScr.angleX = newAngle;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            permiso = false;
            camScr.offsetX = oldOffsetX;
            camScr.offsetY = oldOffsetY;
            camScr.offsetZ = oldOffsetZ;
            camScr.angleX = oldAngle;
        }
    }
}
