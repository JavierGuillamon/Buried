using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    float speed;
    Rigidbody2D rb2d;

   /* [Header("Inputs de movimiento")]
    [Tooltip("Input de movimiento en el eje X")]
    [SerializeField]
    private string StringInputX;
    [Tooltip("Input de movimiento en el eje Y")]
    [SerializeField]
    private string StringInputY;*/

    float inputX, inputY;
    // Use this for initialization
    void Start () {
        rb2d = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () { 
        inputX = InputManager.MainHorizontal();
        Move(inputX);
	}

    void Move(float horizontalInput)
    {
        Vector2 moveVelocity = rb2d.velocity;
        moveVelocity.x = horizontalInput * speed;
        rb2d.AddForce(moveVelocity);
    }

    void OnDisable()
    {
        rb2d.velocity = Vector2.zero;
    }
}
