using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJumpSounds : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "platform")
        {
            print(true);    
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "platform")
        {
            print(false);
        }
    }

}
