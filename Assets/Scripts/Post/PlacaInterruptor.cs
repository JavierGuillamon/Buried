using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaInterruptor : MonoBehaviour {
    
    [SerializeField] Transform tornilloA;
    [SerializeField] Transform tornilloB;
    [SerializeField] Transform tornilloC;
    [SerializeField] float speed;
    [SerializeField] GameObject door;

    bool rotate;

    private void Update()
    {
        if (rotate)
        {
            tornilloA.Rotate(transform.forward * Time.deltaTime * speed);
            tornilloB.Rotate(transform.forward * Time.deltaTime * speed);
            tornilloC.Rotate(transform.forward * Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rotate = true;
        Destroy(door);
    }

}
