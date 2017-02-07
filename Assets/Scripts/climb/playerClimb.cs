using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerClimb : MonoBehaviour {
    [SerializeField]
    private ControlManager manager;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private float speed;
    [Header("Inputs de movimiento")]
    [Tooltip("Input de movimiento en el eje X")]
    [SerializeField]
    private string StringInputY;
    [Header("Draw box gizmos")]
    public BoxCollider2D col;
    public Vector2 offset;
    public Vector2 size;
    public Color color = new Color(1, 0, 0, .5f);
    // Update is called once per frame
    void Update () {
        //Debug.Log(manager.canClimb());
        if (manager.canClimb())
        {
            Vector2 input = new Vector2(0,Input.GetAxisRaw(StringInputY));
            Vector3 velocity = input * speed * Time.deltaTime;
            Player.transform.Translate(velocity);
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("DENTRO");
            manager.setPlayerInPlace(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("FUERA");
            manager.setPlayerInPlace(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube((Vector2)transform.position+offset, size);
    }
}
