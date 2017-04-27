using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuegoCoffin : MonoBehaviour {
    public Material matFire, matNormal;
    private bool onFire = false;
    public float fireCoolDown;
    public MeshRenderer rend;

    private IEnumerator coroutine;
    void Update()
    {
        if (onFire)
        {
            coroutine = fireCD(fireCoolDown);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator fireCD(float time)
    { 
        rend.sharedMaterial = matFire;
        yield return new WaitForSeconds(time);
        setOnFire(false);
        rend.sharedMaterial = matNormal;
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "fire")
        {
            setOnFire(true);
        }
        else if (other.tag == "agua" && onFire)
        {
            StopCoroutine(coroutine);
            setOnFire(false);
            rend.sharedMaterial = matNormal;
        }
        else if (other.tag == "hielo" && onFire)
            other.GetComponent<Agua>().unFreeze();
    }
    
    public void doFuego(float cooldown)
    {
        fireCoolDown = cooldown;
        onFire = true;
    }

    public void setOnFire(bool set)
    {
        onFire = set;
    }
    public bool getOnFire()
    {
        return onFire;
    }
}
