using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class CamTimelineManager : MonoBehaviour {

    public PlayableDirector pd;
    public Animator anim;
    public CinemachineVirtualCamera titleCam;
    public CinemachineVirtualCamera gameplayCam;
    public CinemachineVirtualCamera cutsceneCam;
    [HideInInspector]
    public bool secondPlay;

    private float timeToChange;
    private float firstTimeLeft;
    private float secondTimeLeft;
    private bool fistStop;
    private bool secondStop;

    void Start () {
        firstTimeLeft = .1f;
        secondTimeLeft = 7.6f;
        pd = GetComponent<PlayableDirector>();
        fistStop = false;
        secondStop = false;
        secondPlay = false;
        FindObjectOfType<AudioManager>().Play("AmbientWind");
        titleCam.Priority = 1;
        gameplayCam.Priority = 0;
        cutsceneCam.Priority = 0;
    }
	

	void Update () {
        firstTimeLeft -= Time.deltaTime;
        if (firstTimeLeft < 0 && !fistStop)
        {
            pd.Pause();
        }
        if (Input.anyKey && !fistStop)
        {
            pd.Play();
            titleCam.Priority = 0;
            gameplayCam.Priority = 1;
            anim.SetBool("Permiso", true);
            fistStop = true;
            FindObjectOfType<AudioManager>().Play("EntryLevel");
            secondStop = true;
        }

        if (secondStop)
        {
            secondTimeLeft -= Time.deltaTime;
            if (secondTimeLeft < 0)
            {
                pd.Pause();
            }
            if (secondPlay)
            {
                pd.Play();
            }
        }      
    }
}
