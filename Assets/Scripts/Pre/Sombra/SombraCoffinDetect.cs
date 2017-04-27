using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombraCoffinDetect : MonoBehaviour {

    public bool topLeft, bottomLeft, bottomRight, topRight;

	
    public bool canUseSombra()
    {
        if (topLeft&&topRight||topLeft&&bottomLeft||topRight&&bottomRight||bottomLeft&&bottomRight)
            return true;
        else
            return false;
    }
    

}
