using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CamMov : MonoBehaviour {

    public Transform Target;
    public float OffsetX;
    public float OffsetY;
    public float FollowSpeed;

    void LateUpdate () {
        transform.position = Vector3.Lerp(transform.position, new Vector3(Target.position.x + OffsetX, Target.position.y + OffsetY, transform.position.z), Time.deltaTime * FollowSpeed);
    }
}
