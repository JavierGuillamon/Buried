using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombraMov : MonoBehaviour {

    public Transform pointLeft=null;
    public Transform pointRight=null;
    public Transform pointTop=null;
    public Transform pointDown=null;

    bool vertical;
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
    public Vector3 rejectDirection;
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
        /*input_X = Input.GetAxisRaw(inputX);
        input_Y = Input.GetAxisRaw(inputY);
        rb.MovePosition(rb.position + new Vector2(input_X, input_Y).normalized * moveSpeed * Time.deltaTime);*/
        string input;
        if (vertical) input = inputY;
        else input = inputX;
        
        if (Input.GetAxisRaw(inputX) > 0)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), pointRight.position, moveSpeed * Time.deltaTime);
        }
        else if (Input.GetAxisRaw(inputX) < 0)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), pointLeft.position, moveSpeed * Time.deltaTime);
        }
        else if (Input.GetAxisRaw(inputY) > 0)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), pointTop.position, moveSpeed * Time.deltaTime);
        }
        else if (Input.GetAxisRaw(inputY) < 0)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), pointDown.position, moveSpeed * Time.deltaTime);
        }
    }
    
    public void setTragets(Transform left, Transform right, Transform top, Transform down, Vector3 reject)
    {
        pointLeft = left;
        pointRight = right;
        pointTop = top;
        pointDown = down;
        rejectDirection = reject;
    }

    public void Reject()
    {
        gameObject.layer = LayerMask.NameToLayer("sombra");
        rb.AddForce(rejectDirection * 500);
    }
}