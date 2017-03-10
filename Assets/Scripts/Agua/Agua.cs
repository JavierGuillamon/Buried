using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agua : MonoBehaviour {
    public Material aguaHielo, agua;
    //public GameObject morir;
    public void freeze()
    {
        gameObject.layer = LayerMask.NameToLayer("scenery");
        transform.Find("PlayerSensor").gameObject.SetActive(false);
        tag = "hielo";
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = aguaHielo;
    }

    public void unFreeze()
    {
        gameObject.layer = LayerMask.NameToLayer("Water");
        transform.Find("PlayerSensor").gameObject.SetActive(true);
        tag = "agua";
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = agua;

    }
}
