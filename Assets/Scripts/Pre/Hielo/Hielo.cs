using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hielo : MonoBehaviour {
    public Material matHielo, matNormal;
    public MeshRenderer rend;
    public Rigidbody2D coffinRB;
    public float timePlataformStop;
    public float timeWaterFrezze;
    private bool canFreeze = false;

    private IEnumerator coroutine;
    

    private IEnumerator hieloCD(float time)
    {
        rend.sharedMaterial = matHielo;
        yield return new WaitForSeconds(time);
        canFreeze = false;
        rend.sharedMaterial = matNormal;
    }

   
    public void HieloExplode(float timeInAir)
    {
        canFreeze = true;
        coroutine = hieloCD(timeInAir);
        StartCoroutine(coroutine);
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
        if (canFreeze) 
        {
            if (other.tag=="plataform")
                StartCoroutine(freeze(timePlataformStop,other));
            if (other.tag == "agua")
            {
                other.GetComponent<Agua>().freeze();
            }
        }
    }

    private IEnumerator freeze(float time, Collider2D other) {
      //  other.transform.GetComponent<PlatformController>().enabled = false;
        yield return new WaitForSeconds(time);
       // other.transform.GetComponent<PlatformController>().enabled = true;
    }
}
