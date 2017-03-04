using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombraMov : MonoBehaviour {


    [Header("Valores de movimiento")]
    [Tooltip("Velocidad de movimiento")]
    [SerializeField]
    private float moveSpeed = 6;


    [Header("Inputs de movimiento")]
    [Tooltip("Input de movimiento en el eje X")]
    [SerializeField]
    private string inputX;
    [Tooltip("Input de movimiento en el eje Y")]
    [SerializeField]
    private string inputY;

    private Rigidbody2D rb;
    float input_X, input_Y;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        input_X = Input.GetAxisRaw(inputX);
        input_Y = Input.GetAxisRaw(inputY);
        rb.MovePosition(rb.position + new Vector2(input_X, input_Y).normalized * moveSpeed * Time.deltaTime);
    }
    
}