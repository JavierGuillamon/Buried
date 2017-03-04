using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuegoCoffin : MonoBehaviour {
    public Material matFire, matNormal;
    private bool onFire = false;
    public float fireCoolDown;
    public MeshRenderer rend;
  

    void Update()
    {
        if (onFire)
        {
            StartCoroutine(fireCD(fireCoolDown));
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
        if (other.tag =="fire")
        {
            setOnFire(true);
        }
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
