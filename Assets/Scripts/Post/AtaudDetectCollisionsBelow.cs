using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaudDetectCollisionsBelow : MonoBehaviour {
    public Jugador jugadorScript;

    void OnTriggerEnter2D()
    {
        jugadorScript.setCoffinGround(true);
    }
    void OnTriggerExit2D()
    {
        jugadorScript.setCoffinGround(false);
    }
    void OnTriggerStay2D()
    {
        jugadorScript.setCoffinGround(true);
    }
}
