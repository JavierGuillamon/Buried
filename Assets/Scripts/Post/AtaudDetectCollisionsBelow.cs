using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaudDetectCollisionsBelow : MonoBehaviour {
    public Jugador jugadorScript;
    public float margin;
    public Collider2D col;
    public LayerMask layerMask;
    public AtaudDetectCollisionsBelow otherDetector;

    private bool isHitting;
    private bool detectCollisions=true;
    void Update()
    {
        if (Physics2D.Raycast(transform.position, -Vector2.up, margin, layerMask))
        {
            jugadorScript.setCoffinGround(true);
                isHitting = true;
            
        }else
        {
            if(!otherDetector.getHitting())
                jugadorScript.setCoffinGround(false);
            isHitting = false;
        }
    }
    

    public bool getHitting()
    {
        return isHitting;
    }
}
