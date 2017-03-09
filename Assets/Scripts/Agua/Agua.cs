using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agua : MonoBehaviour {
    public Material aguaHielo, agua;
    public void freeze()
    {
        gameObject.layer = LayerMask.NameToLayer("scenery");
        tag = "hielo";
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = aguaHielo;
    }

    public void unFreeze()
    {
        gameObject.layer = LayerMask.NameToLayer("Water");
        tag = "agua";
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = agua;

    }
}
