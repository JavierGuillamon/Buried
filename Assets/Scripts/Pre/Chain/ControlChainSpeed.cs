using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlChainSpeed : MonoBehaviour {
    [SerializeField]
    private Player controlPlayer;
    [SerializeField]
    private ControllCoffin controlCoffin;
    [SerializeField]
    private Transform Player;
    public float distanceNorm;
    public float distSlow;
    
	void Update () {
        if (Vector2.Distance(transform.position, Player.position) <= distanceNorm)
        {
            controlCoffin.reset();
        }else /*if(Vector2.Distance(transform.position, Player.position) <= distSlow)*/{
            controlPlayer.setMoveSpeed(controlCoffin.getSpeedOut());
        }/*
        else
        {
            if (controlCoffin.leftOrRight())
                controlPlayer.freezeLeft();
            else
                controlPlayer.freezeRight();
        }*/
	}
}
