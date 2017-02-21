using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectorFuego : MonoBehaviour {
    
    public GameObject puerta;
    public Vector3 movimientoPuerta;
    private bool canOpen = true;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "coffinFire")
        {
            if (other.GetComponent<FuegoCoffin>().getOnFire() && canOpen)
            {
                StartCoroutine(moveDoor(3f));
            }
        }
    }

    public IEnumerator moveDoor(float time)
    {
        canOpen = false;
        puerta.transform.Translate(movimientoPuerta);
        yield return new WaitForSeconds(time);
        Destroy(puerta);
    }
}
