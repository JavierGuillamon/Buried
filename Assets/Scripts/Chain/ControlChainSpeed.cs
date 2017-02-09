using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlChainSpeed : MonoBehaviour {
    [SerializeField]
    private Player controlPlayer;
    [SerializeField]
    private ControllCoffin controlCoffin;

    private bool inNormalDetect=true;
    private bool inSlowDetect=true;

	
	// Update is called once per frame
	void Update () {
        if (inNormalDetect)
            controlCoffin.reset();
        else if (inSlowDetect)
            controlPlayer.setMoveSpeed(controlCoffin.getSpeedOut());
        else
        {
            if (controlCoffin.leftOrRight())
                controlPlayer.freezeLeft();
            else
                controlPlayer.freezeRight();
        }
	}

    public void setInNormalDetect(bool aux)
    {
        inNormalDetect = aux;
    }

    public void setInSlowDetect(bool aux)
    {
        inSlowDetect = aux;
    }
}
