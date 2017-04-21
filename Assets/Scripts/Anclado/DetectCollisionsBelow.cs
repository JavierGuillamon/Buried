using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisionsBelow : MonoBehaviour {
    public PlayerCoffinPositionManager pcpm;

	void OnTriggerEnter2D()
    {
        pcpm.setCoffinGround(true);
    }
    void OnTriggerExit2D()
    {
        pcpm.setCoffinGround(false);
    }
    void OnTriggerStay2D()
    {
        pcpm.setCoffinGround(true);
    }
}
