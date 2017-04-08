using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoffinPositionManager : MonoBehaviour {
    public Transform player;
    public Transform coffin;
    public bool playerUp;
    public bool playerGround;
    public bool coffinGround;
    public Vector2 tr;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        tr = player.position - coffin.position;
        if (tr.y > 0) playerUp = true;
        else playerUp = false;
        playerGround = player.GetComponent<Controller2D>().collisions.below;
        
    }

    public void setCoffinGround(bool aux)
    {
        coffinGround = aux;
    }
}
