using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sombraSensor : MonoBehaviour {
   
	public int position;
    public SombraCoffinDetect coffinDetect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "scenerySombra")
        {
            switch (position)
            {
                case 1:
                    coffinDetect.topLeft = true;
                    break;
                case 2:
                    coffinDetect.topRight = true;
                    break;
                case 3:
                    coffinDetect.bottomLeft = true;
                    break;
                case 4:
                    coffinDetect.bottomRight = true;
                    break;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "scenerySombra")
        {
            switch (position)
            {
                case 1:
                    coffinDetect.topLeft = false;
                    break;
                case 2:
                    coffinDetect.topRight = false;
                    break;
                case 3:
                    coffinDetect.bottomLeft = false;
                    break;
                case 4:
                    coffinDetect.bottomRight = false;
                    break;
            }
        }
    }
}
