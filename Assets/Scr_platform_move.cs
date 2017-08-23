using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_platform_move : MonoBehaviour {

    public bool player_inside = false;
    public bool coffin_inside = false;
    public GameObject platform;
    public float speed;
    public Transform target1;
    public Transform target2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            player_inside = true;
        else if (collision.tag == "Coffin")
            coffin_inside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            player_inside = false;
        else if (collision.tag == "Coffin")
            coffin_inside = false;
    }
    void Update()
    {
        float step = speed * Time.deltaTime;
        if (player_inside && coffin_inside)
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, target1.position, step);
        else
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, target2.position, step);
    }
}
