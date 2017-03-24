using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogerAtaudDetector : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<ControllCoffin>().setRecogerAtaud(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<ControllCoffin>().setRecogerAtaud(false);
    }
}
