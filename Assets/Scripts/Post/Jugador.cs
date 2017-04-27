using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Jugador : MonoBehaviour {
    //controlMannager
    private bool moving;
    private bool sombra;
    private bool swing;
    private bool climb;
    private bool resize;
    public Chain chainScript;
    public Rigidbody2D rb2d;
    //end

    //Player
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
    private float accelerationTimeAirborne = .2f;
    [Tooltip("Aceleración en el suelo")]
    [SerializeField]
    private float acceletarionTimeGrounded = .1f;
    private float gravity;
    private float jumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;
    private bool canMoveLeft = true, canMoveRight = true;
    private Controller2D controller;
    //end

    //PlayerController
    [SerializeField]
    float speed;
    //end

    //PlayerCoffinPositionManager
    public Transform player;
    public Transform coffin;
    public bool coffinUp;
    public bool playerGround;
    public bool coffinGround;
    public Vector2 distanciaJugadorAtaud;
    public float deadZone;
    [SerializeField]
    private float speedTakeCoffin = 10;
    public Rigidbody2D rb2dc;
    [SerializeField]
    private float distanceToTakeCoffin = .75f;
    private bool coffinTaken;
    private bool click = false;
    public Ataud controlCoffin;
    [SerializeField]
    DistanceJoint2D joint;
    //end
    [Tooltip("Velocidad de movimiento cogiendo el ataud")]
    [SerializeField]
    private float moveSpeedWithCoffin;
    [SerializeField]
    private float jumpHeightWithCoffin;
    [SerializeField]
    private float timeJumpWithCoffin;


    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeJump;
        auxMoveSpeed = moveSpeed;
        auxJumpHeight = jumpHeight;
        auxTimeJump = timeJump;

        moving = true;
    }
	
	void Update () {
        if (!InputManager.RightTrigger())
        {



            //moving
            if (resize)
                chainScript.setResize(true);
            else
                chainScript.setResize(false);


            //SI se puede mover y no se esta preprarando para lanzar
            if (moving)
            {
                resize = true;
                rb2d.isKinematic = true;
                Move();
            }
            if (swing)
            {
                resize = false;
                rb2d.isKinematic = false;
                MoveSwing();
            }

            if (sombra)
            {
                resize = false;
                rb2d.isKinematic = false;
                GetComponent<Player>().enabled = false;
                GetComponent<PlayerController>().enabled = false;
            }

            if (climb)
            {
                rb2d.isKinematic = false;
                MoveSwing();
            }
        }
    }

    void FixedUpdate()
    {
        distanciaJugadorAtaud = player.position - coffin.position;
        if (distanciaJugadorAtaud.y < deadZone) coffinUp = true;
        else coffinUp = false;
        playerGround = player.GetComponent<Controller2D>().collisions.below;
        React();
    }

    private void Move()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeJump;

        Vector2 input;
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        input = new Vector2(InputManager.MainHorizontal(), System.Convert.ToInt32(InputManager.AButton()));
        if (!canMoveLeft && InputManager.MainHorizontal() < 0)
            input = new Vector2(0, System.Convert.ToInt32(InputManager.AButton()));
        else if (!canMoveRight && InputManager.MainHorizontal() > 0)
            input = new Vector2(0, System.Convert.ToInt32(InputManager.AButton()));

        if (input.y > 0 && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? acceletarionTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void MoveSwing()
    {
        Vector2 moveVelocity = rb2d.velocity;
        moveVelocity = Vector2.right * InputManager.MainHorizontal() * speed;
        rb2d.AddForce(moveVelocity);
    }

    private void React()
    {
        if (playerGround && coffinGround)
        {
            rb2dc.mass = 1;
            rb2dc.constraints = RigidbodyConstraints2D.FreezeRotation;

            controlCoffin.ataudColgando = false;
            rb2d.mass = 1;
            swing = false;
            climb = false;
            //atraer
            controlCoffin.setCanTake(true);
            if (InputManager.LeftTrigger())
            {
                controlCoffin.setRecogerAtaud(true);
            }
        }
        else if (playerGround && !coffinGround)
        {
            rb2dc.mass = 1;
            rb2dc.constraints = RigidbodyConstraints2D.None;
            controlCoffin.ataudColgando = true;
            rb2d.mass = 100;
            swing = false;
            climb = false;
            if (!coffinUp)
            {
                //jugador en el suelo ataud por debajo
                //atraer
                controlCoffin.setCanTake(true);
                if (InputManager.LeftTrigger())
                {
                    controlCoffin.setRecogerAtaud(true);
                }
            }
            else
            {
                controlCoffin.setCanTake(false);
            }
        }
        else if (!playerGround && coffinGround)
        {
            rb2dc.mass = 100;
            rb2dc.constraints = RigidbodyConstraints2D.FreezeRotation;
            controlCoffin.ataudColgando = false;
            rb2d.mass = 1;
            if (coffinUp)
            {
                //ataud en el suelo por encima del jugador
                //escalar
                controlCoffin.setCanTake(false);
                Escalar();
            }
        }
        else if (!playerGround && !coffinGround)
        {
            rb2dc.mass = 1;
            controlCoffin.ataudColgando = false;
            rb2dc.constraints = RigidbodyConstraints2D.None;
            rb2d.mass = 1;
            swing = false;
            climb = false;
            moving = true;
        }
    }

    private void Escalar()
    {
        swing = true;
        if (InputManager.LeftTrigger())
        {
            climb = true;
            resize = true;
            joint.distance -= speedTakeCoffin * Time.deltaTime;
            click = true;
        }
        else if (InputManager.LeftTriggerUp() && click)
        {
            climb = false;
            resize = false;
            click = false;
        }
        else
        {
            climb = true;
            resize = true;
            joint.distance += speedTakeCoffin * Time.deltaTime;
            joint.distance = Mathf.Clamp(joint.distance, 0, 15);
            click = true;
        }
    }

    public void Reset()
    {
        canMoveRight = true;
        canMoveLeft = true;
        moveSpeed = auxMoveSpeed;
        jumpHeight = auxJumpHeight;
        timeJump = auxTimeJump;
        if (coffinTaken)
        {
            moving = true;
            moveSpeed = moveSpeedWithCoffin;
            jumpHeight = jumpHeightWithCoffin;
            timeJump = timeJumpWithCoffin;
        }
    }

    public void setMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public void setCoffinTaken(bool aux)
    {
        coffinTaken = aux;
    }

    public void setCoffinGround(bool aux)
    {
        coffinGround = aux;
    }
}
