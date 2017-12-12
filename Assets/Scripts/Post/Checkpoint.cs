using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField] Transform target;
    [SerializeField] Transform player;
    [SerializeField] Transform coffin;
    [SerializeField] Transform chain;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.position = target.position;
        coffin.position = target.position;
        chain.position = target.position;
    }

}
