using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPositionReference : MonoBehaviour {

    public Transform player;
    public Transform coffin;
    public float percent;
	
	void Update () {
        transform.position = Vector3.Lerp(player.position, coffin.position, percent);
    }
}
