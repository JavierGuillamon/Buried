using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuego : MonoBehaviour {
    public float timeToDestroyWithCoffin;
    public float timeToDestroy;
    public Material mat;
    public bool onFire = false, coffin=false;

    void FixedUpdate()
    {
        StartCoroutine(vibrate());
        if (onFire)
        {
            if(!coffin) StartCoroutine(waitAndDestroy(timeToDestroy));
        }              
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "coffinFire")
        {
            if (other.GetComponent<FuegoCoffin>().getOnFire())
            {
                onFire = true;
                coffin = true;
                StartCoroutine(waitAndDestroy(timeToDestroyWithCoffin));
            }
                
        }
        if (other.tag == "flammable")
        {
            if (onFire) {
                StartCoroutine(waitAndcall(timeToDestroy, other));
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "coffinFire")
        {
            if (other.GetComponent<FuegoCoffin>().getOnFire())
            {
                onFire = true;
                //StartCoroutine(waitAndDestroy(5000));
            }

        }
        if (other.tag == "flammable")
        {
            // Debug.Log(onFire+" "+gameObject.name);
            if (onFire)
            {
                other.gameObject.GetComponent<Fuego>().setOnFire(true);
            }
        }
    }
    private IEnumerator waitAndcall(float time, Collider2D other)
    {
        yield return new WaitForSeconds(time);
        other.gameObject.GetComponent<Fuego>().setOnFire(true);
    }

    private IEnumerator waitAndDestroy(float time)
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material = mat;
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
    
    private IEnumerator vibrate()
    {
        gameObject.transform.position += new Vector3(0.00001f,0,0);
        yield return new WaitForSeconds(0.01f);
        gameObject.transform.position -= new Vector3(0.00001f, 0, 0);
        yield return new WaitForSeconds(0.01f);
    }

    public void setOnFire(bool set)
    {
        onFire = set;
    }
}
