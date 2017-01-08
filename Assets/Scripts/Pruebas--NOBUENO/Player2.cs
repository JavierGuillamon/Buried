using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player2 : MonoBehaviour
{

    [Header("Valores de movimiento")]
    [Tooltip("Velocidad de movimiento")]
    [SerializeField]
    private float moveSpeed = 6;
    private float auxMoveSpeed;
    [Tooltip("Altura del salto")]
    [SerializeField]
    private float jumpHeight = 4;
    private float auxJumpHeight;
    [Tooltip("Tiempo que pasa en el aire")]
    [SerializeField]
    private float timeJump = .4f;
    private float auxTimeJump;
    [Tooltip("Aceleración en el aire")]
    [SerializeField]
    private float accelerationTimeAirborne=.2f;
    [Tooltip("Aceleración en el suelo")]
    [SerializeField]
    private float acceletarionTimeGrounded =.1f;
    

    [Header("Inputs de movimiento")]
    [Tooltip("Input de movimiento en el eje X")]
    [SerializeField]
    private string inputX;
    [Tooltip("Input de movimiento en el eje Y")]
    [SerializeField]
    private string inputY;
   /* [SerializeField]
    [Tooltip("Tecla con la que cogemos el ataud")]
    private KeyCode coffin;
    [Header("Referencias a otros Scripts")]
    [SerializeField]
    private ControllCoffin controlCoffin;*/

    private float gravity;
    private float jumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;
    private bool canMoveLeft=true, canMoveRight=true;
    private Controller2D controller;
	
	// Use this for initialization
	void Start ()
    {
		controller = GetComponent<Controller2D> ();
		
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeJump, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeJump;
        auxMoveSpeed = moveSpeed;
        auxJumpHeight = jumpHeight;
        auxTimeJump = timeJump;
    }
	
	void Update()
    {
        Move();
	}

    private void Move()
    { 
        //Eliminar estas lineas una vez tengamos claro los valores de JumpHeight y TimeJump
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeJump;
        //------------------------------------------------------------
        Vector2 input;
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        input = new Vector2(Input.GetAxisRaw(inputX), Input.GetAxisRaw(inputY));
        if (!canMoveLeft && Input.GetAxisRaw(inputX) < 0)
            input = new Vector2(0, Input.GetAxisRaw(inputY));
        else if (!canMoveRight && Input.GetAxisRaw(inputX) > 0)
            input = new Vector2(0, Input.GetAxisRaw(inputY));

        if (input.y > 0 && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? acceletarionTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

  

    //GETTERS && SETTERS
    public void setMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }
    public void resetSpeed()
    {
        moveSpeed = auxMoveSpeed;
    }

    public void freezeLeft()
    {
        canMoveLeft = false;
    }
    public void freezeRight()
    {
        canMoveRight = false;
    }

    public void resetFreeze()
    {
        canMoveRight = true;
        canMoveLeft = true;
    }
    public void setJumpHeight(float jump)
    {
        jumpHeight = jump;
    }
    public void setTimeJump(float time)
    {
        timeJump = time;
    }
    public void resetJumpHeight()
    {
        jumpHeight = auxJumpHeight;
    }
    public void resetTimeJump()
    {
        timeJump = auxTimeJump;
    }

}
