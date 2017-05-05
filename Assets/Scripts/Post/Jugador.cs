using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Jugador : MonoBehaviour {
    //controlMannager
    public bool moving;
    public bool sombra;
    public bool swing;
    public bool climb;
    public bool resize;
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
    [SerializeField]
    float jumpVelocity;
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
    //public Transform player;
    public Transform coffin;
    public bool coffinUp;
    public bool playerGround;
    public bool coffinGround;
    public bool CoffinGround {
        get { return coffinGround; }
    }
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
    //end
    [Tooltip("Velocidad de movimiento cogiendo el ataud")]
    [SerializeField]
    private float moveSpeedWithCoffin;
    [SerializeField]
    private float jumpHeightWithCoffin;
    [SerializeField]
    private float timeJumpWithCoffin;
    [SerializeField]
    private float distanciaMaximaJugadorCoffinAlEscalar;
    private float distanciaJugadorCoffin;
    public Collider2D AtaudCollider;

    bool lookingRight = true;
    public bool LookingRight { get { return lookingRight; } }

    [SerializeField]
    Collider2D groundTrigger;
    [SerializeField]
    LayerMask groundMask;
    Vector2 input;

    [SerializeField]
    float frictionX;
    [SerializeField]
    float accelerationXNormal;
    [SerializeField]
    float accelerationXSlow;
    [SerializeField]
    float gravityUp;
    [SerializeField]
    float gravityDown;
    public bool jumped = false;
    public float maxSpeed;
    [SerializeField]
    float maxDistanceAtaudCofin;

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeJump, 2);
        auxMoveSpeed = moveSpeed;
        auxJumpHeight = jumpHeight;
        auxTimeJump = timeJump;

        moving = true;
    }
	
	void FixedUpdate ()
    {
        Slope();
        distanciaJugadorAtaud = transform.position - coffin.position;
        distanciaJugadorCoffin = Vector3.Distance(transform.position, coffin.position);
        if (distanciaJugadorAtaud.y < deadZone) coffinUp = true;
        else coffinUp = false;
        //Debug.Log(groundTrigger.IsTouchingLayers(groundMask));
        React();

        playerGround = groundTrigger.IsTouchingLayers(groundMask); /* GetComponent<Controller2D>().collisions.below;*/

        if (InputManager.MainHorizontal() > 0)
            lookingRight = true;
        else if(InputManager.MainHorizontal() < 0)
            lookingRight = false;

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
                Move();
            }
             if (swing)
            {
                resize = false;
                MoveSwing();
            }

            if (sombra)
            {
                resize = false;
                //GetComponent<Player>().enabled = false;
                //GetComponent<PlayerController>().enabled = false;
            }

             if (climb)
            {
                moving = false;
                MoveSwing();
            }
        }
    }
   
    void Update()
    {
        input = new Vector2(InputManager.MainHorizontal(), System.Convert.ToInt32(InputManager.AButton()));
    }
    
    private void Move()
    {

        Vector2 vely = rb2d.velocity.y * Vector2.up;
        if (rb2d.velocity.y >= 0)
            vely -= gravityUp * Time.deltaTime * Vector2.up;
        else
            vely -= gravityDown * Time.deltaTime * Vector2.up;
        if (input.y > 0 && playerGround)
        {
            vely = jumpVelocity * Vector2.up;
            jumped = true;
        }
        /*
        Vector2 velx = rb2d.velocity.x * Vector2.right;
        velx += input.x * Time.deltaTime * accelerationX * Vector2.right;
        if (Mathf.Abs(input.x) < 0.2f)
            velx *= Mathf.Clamp01(1 - frictionX * Time.deltaTime);
        if (Mathf.Abs(velx.x) > maxSpeed)
            velx.x = maxSpeed*input.x;
       rb2d.velocity = velx + vely;*/

        //Debug.Log("distanciaJugadorCoffin:: " + distanciaJugadorCoffin);
        if (distanciaJugadorCoffin > maxDistanceAtaudCofin)
        {
             if (transform.position.x > coffin.position.x && InputManager.MainHorizontal() > 0)
                 rb2d.velocity = new Vector2(input.x * Time.deltaTime * accelerationXSlow, vely.y);
             else if (transform.position.x < coffin.position.x && InputManager.MainHorizontal() < 0)
                 rb2d.velocity = new Vector2(input.x * Time.deltaTime * accelerationXSlow, vely.y);
             else
                 rb2d.velocity = new Vector2(input.x * Time.deltaTime * accelerationXNormal, vely.y);
        }          
        else
            rb2d.velocity = new Vector2(input.x * Time.deltaTime * accelerationXNormal, vely.y);  
        
            
    }

    private void MoveSwing()
    {
        Vector2 moveVelocity = rb2d.velocity;
        moveVelocity = Vector2.right * InputManager.MainHorizontal() * speed;
        rb2d.AddForce(moveVelocity);
    }

    /*[SerializeField]
    float slopeFriction;*/
    private void Slope()
    {
        if (playerGround)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,Mathf.Infinity, groundMask);
            if(hit.collider!=null && Mathf.Abs(hit.normal.x) > 0.1f){
                /*Debug.Log("AAA hit noraml:: " + hit.normal.x);
                rb2d.velocity = new Vector2(rb2d.velocity.x- hit.normal.x * rb2d.velocity.x,rb2d.velocity.y);              
                transform.position = new Vector2(transform.position.x,transform.position.y+ -hit.normal.x*Mathf.Abs(rb2d.velocity.x)*Time.deltaTime*(rb2d.velocity.x-hit.normal.x>0?1:-1));
                */
                rb2d.gravityScale = 0;
            }
            
        }
        else rb2d.gravityScale = 10;
    }

    private void React()
    {
        if (playerGround && coffinGround)
        {
            rb2dc.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (!coffinUp)
                controlCoffin.ataudColgando = true;
            else controlCoffin.ataudColgando = false;
            
            setSwing(false);
            setClimb(false);
            //atraer
            controlCoffin.setCanTake(true);
            if (InputManager.LeftTrigger())
            {
                controlCoffin.setRecogerAtaud(true);
            }
        }
        else if (playerGround && !coffinGround)
        {
            //rb2dc.constraints = RigidbodyConstraints2D.None;
            controlCoffin.ataudColgando = true;
            setSwing(false);
            setClimb(false);
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
            rb2dc.constraints = RigidbodyConstraints2D.FreezeRotation;
            controlCoffin.ataudColgando = false;
            if (coffinUp)
            {
                //ataud en el suelo por encima del jugador
                //escalar
                controlCoffin.setCanTake(false);
               // Debug.Log("dist: " + distanciaJugadorCoffin + " dist max: "+distanciaMaximaJugadorCoffinAlEscalar);
                Escalar();
            }
        }
        else if (!playerGround && !coffinGround)
        {
            controlCoffin.ataudColgando = false;
            //rb2dc.constraints = RigidbodyConstraints2D.None;
            swing = false;
            climb = false;
            moving = true;
        }
    }

    private void Escalar()
    {
        if (Input.GetButton("Jump"))
        {
            AtaudCollider.enabled = false;
        }
        else
        {
            setSwing(true);
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

    public void freezeLeft()
    {
        canMoveLeft = false;
    }

    public void freezeRight()
    {
        canMoveRight = false;
    }

    public void setMoving(bool aux)
    {
        moving = aux;
    }

    public void setSwing(bool aux)
    {
        
        swing = aux;
        moving = !aux;
    }

    public void setSombra(bool aux)
    {
        sombra = aux;
    }

    public void setClimb(bool aux)
    {
        climb = aux;
        moving = !aux;
    }

}
