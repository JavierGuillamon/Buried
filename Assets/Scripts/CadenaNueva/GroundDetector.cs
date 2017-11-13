using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour {

    [SerializeField]
    LayerMask terrainLayers;
    [SerializeField]
    float testDistance;

    public bool DetectGround()
    {
        RaycastHit2D r = Physics2D.Raycast(transform.position,-Vector3.up,testDistance,terrainLayers);
        return r.collider != null;
    }
}
