using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Jugador : MonoBehaviour {
    //controlMannager
    public bool moving;
    public bool sombra;
    public bool swing;
    public Rigidbody2D rb2d;
    //end

    //Player
    [Header("Valores de movimiento")]
    [Tooltip("Velocidad de movimiento")]
    [SerializeField]
    private float moveSpeed = 6;
    private float auxMoveSpeed;
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
    public Ataud controlCoffin;
    //end
    [Tooltip("Velocidad de movimiento cogiendo el ataud")]
    [SerializeField]
    private float moveSpeedWithCoffin;
    [SerializeField]
    private float jumpHeightWithCoffin;
    [SerializeField]
    private float timeJumpWithCoffin;
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

    private bool inPlatform;

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller2D>();
        moving = true;
    }
	
	void FixedUpdate ()
    {
        distanciaJugadorAtaud = transform.position - coffin.position;
        distanciaJugadorCoffin = Vector3.Distance(transform.position, coffin.position);
        React();
        if (inPlatform)
            playerGround = true;
        else
            playerGround = groundTrigger.IsTouchingLayers(groundMask);

        if (InputManager.MainHorizontal() > 0)
            lookingRight = true;
        else if(InputManager.MainHorizontal() < 0)
            lookingRight = false;

        if (!InputManager.RightTrigger())
        {
            if (moving) Move();            
            if (swing) MoveSwing(); 
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
        
        if(coffinTaken)
            rb2d.velocity = new Vector2(input.x * Time.deltaTime * accelerationXSlow, 0);
        else if (distanciaJugadorCoffin > maxDistanceAtaudCofin)
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

    private void React()
    {
        if (playerGround && coffinGround)
        {
            rb2dc.constraints = RigidbodyConstraints2D.FreezeRotation;
            controlCoffin.ataudColgando = false;
            //atraer
            if (InputManager.LeftTrigger())
            {
                controlCoffin.setRecogerAtaud(true);
            }
        }
        else if (playerGround && !coffinGround)
        {
            controlCoffin.ataudColgando = true;
            if (!coffinUp)
            {
                //jugador en el suelo ataud por debajo
                //atraer
                if (InputManager.LeftTrigger())
                {
                    controlCoffin.setRecogerAtaud(true);
                }
            }
        }
        else if (!playerGround && coffinGround)
        {
            rb2dc.constraints = RigidbodyConstraints2D.FreezeRotation;
            controlCoffin.ataudColgando = false;        
        }
        else if (!playerGround && !coffinGround)
        {
            controlCoffin.ataudColgando = false;
            swing = false;
            moving = true;
        }
    }

    public void Reset()
    {
        canMoveRight = true;
        canMoveLeft = true;
        moveSpeed = auxMoveSpeed;
        if (coffinTaken)
        {
            moving = true;
            moveSpeed = moveSpeedWithCoffin;
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

    public void setSombra(bool aux)
    {
        sombra = aux;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "platform")
        {
            inPlatform = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "platform")
        {
            inPlatform = false;
        }
    }
}
