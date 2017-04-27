using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActivator : MonoBehaviour {
    [SerializeField]
    ParticleSystem particles;

    void OnTriggerEnter2D(Collider2D col)
    {
        particles.Play();
    }
    void OnTriggerExit2D(Collider2D col)
    {
        particles.Stop();
    }
}
