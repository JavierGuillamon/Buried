﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuego : MonoBehaviour {
    public float timeToDestroy;
    public float timeToPassFire;
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
            }
                
        }
        if (other.tag == "flammable")
        {
            if (onFire) {
                StartCoroutine(waitAndcall(timeToPassFire, other));
            }
        }
    }

    /*void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "coffinFire")
            {
                if (other.GetComponent<FuegoCoffin>().getOnFire())
                {
                    onFire = true;
                }

            }
            if (other.tag == "flammable")
            {
                if (onFire)
                {
                    if(other!=null)
                        other.gameObject.GetComponent<Fuego>().setOnFire(true);
                }
            }
        }*/
        
    
    
    #region IEnumerators
    private IEnumerator waitAndcall(float time, Collider2D other)
    {
        yield return new WaitForSeconds(time);
        if(other!= null)
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
    #endregion
    public void setOnFire(bool set)
    {
        onFire = set;
    }
}
