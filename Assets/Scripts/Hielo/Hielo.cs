using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hielo : MonoBehaviour {
    public Material matHielo, matNormal;
    public MeshRenderer rend;
    public Rigidbody2D coffinRB;
    public float timePlataformStop;
    private bool canFreeze = true;

    void Start()
    {
        canFreeze = false;
    }

    public void activateHielo()
    {
        rend.sharedMaterial = matHielo;
    }

    public void HieloExplode(float timeInAir)
    {

        canFreeze = true;
        StartCoroutine(waitOnAir(timeInAir));
    }

    private IEnumerator waitOnAir(float time)
    {
        coffinRB.isKinematic = true;
        coffinRB.velocity = Vector3.zero;
        coffinRB.angularVelocity = 0;
        yield return new WaitForSeconds(time);
        coffinRB.isKinematic = false;
        canFreeze = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (canFreeze && other.tag=="plataform")
        {
            StartCoroutine(freeze(timePlataformStop,other));
        }
    }
    private IEnumerator freeze(float time, Collider2D other) {
        other.transform.GetComponent<PlatformController>().enabled = false;
        yield return new WaitForSeconds(time);
        other.transform.GetComponent<PlatformController>().enabled = true;
    }
}
