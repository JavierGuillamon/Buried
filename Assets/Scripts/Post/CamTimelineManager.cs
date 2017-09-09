using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CamTimelineManager : MonoBehaviour {

    public PlayableDirector pd;
    public Animator anim;

    private float timeToChange;
    private float timeLeft;
    private bool stop;


    void Start () {
        timeLeft = .1f;
        pd = GetComponent<PlayableDirector>();
        stop = false;
        FindObjectOfType<AudioManager>().Play("AmbientWind");
    }
	

	void Update () {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0 && !stop)
        {
            pd.Pause();
        }
        if (Input.anyKey && !stop)
        {
            pd.Play();
            anim.SetBool("Permiso", true);
            stop = true;
            FindObjectOfType<AudioManager>().Play("EntryLevel");
        }
    }
}
