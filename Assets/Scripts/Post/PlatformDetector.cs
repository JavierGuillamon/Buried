using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetector : MonoBehaviour {
    [SerializeField]
    Transform movingPlatform;
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag!="chain")
            other.gameObject.transform.parent = movingPlatform.transform;
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag != "chain")
            other.gameObject.transform.parent = null;
    }
}
