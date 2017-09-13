using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneActivator : MonoBehaviour {

    public GameObject timelineMan;

    private CamTimelineManager timelineScr;
    public float objects;
    private Animator anim;

    private void Start()
    {
        timelineScr = timelineMan.GetComponent<CamTimelineManager>();
        objects = 0;
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "TriggerPlacaPresion")
        {
            objects += 1;
            if (objects >= 3)
            {
                timelineScr.secondPlay = true;
                anim.SetBool("Permiso", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TriggerPlacaPresion")
        {
            objects -= 1;
        }
    }
}
