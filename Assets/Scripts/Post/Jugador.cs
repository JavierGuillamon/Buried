using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller2D>();
        moving = true;
        trajectoryPoints = new List<GameObject>();
        for (int i = 0; i < numTrajectoryPoints; i++)
            trajectoryPoints.Add(Instantiate(trajectoryPrefeb));
        DisableTrajectory();
    }

    //checkKinematics
    [SerializeField]
    float maxMass;
    [SerializeField]
    float massChangeDistance = 10;
    [SerializeField]
    float maxDistancePull = 10;
    [SerializeField]
    DistanceJoint2D jointPlayer;
    [SerializeField]
    DistanceJoint2D jointCoffin;
    [SerializeField]
    Rigidbody2D kinematicBody;
    [SerializeField]
    Rigidbody2D kinematicBodyPlayer;
    [SerializeField]
    float massInterpolationSpeed;

    void CheckKinematics()
    {

        rb2dc.mass = Mathf.Lerp(rb2dc.mass, 1, Time.deltaTime * massInterpolationSpeed);
        this.rb2d.mass = Mathf.Lerp(rb2d.mass, 1, Time.deltaTime * massInterpolationSpeed);

        if (coffinTaken)
        {
            jointCoffin.connectedBody = kinematicBody;
            jointPlayer.connectedBody = kinematicBodyPlayer;

        } else if (this.playerGround && this.coffinGround)
        {
            if (taking)
            {
                jointCoffin.connectedBody = rb2dc;
                jointPlayer.connectedBody = kinematicBodyPlayer;
            }
            else
            {
                jointCoffin.connectedBody = rb2dc;
                jointPlayer.connectedBody = kinematicBodyPlayer;
            }
            coffinThrown = false;
        }
        else if (!this.playerGround && this.coffinGround)
        {
            jointCoffin.connectedBody = kinematicBody;
            jointPlayer.connectedBody = this.rb2d;
            if (Vector2.Distance(rb2dc.position, this.rb2d.position) > massChangeDistance)
            {
                this.jumped = false;
                this.rb2d.mass = maxMass;
            }
            coffinThrown = false;
        }
        else if (this.playerGround && !this.coffinGround)
        {
            jointCoffin.connectedBody = rb2dc;
            jointPlayer.connectedBody = kinematicBodyPlayer;
            if (this.jumped && this.rb2d.velocity.y <= 0)
                this.jumped = false;
        }
        else
        {
            jointCoffin.connectedBody = rb2dc;
            jointPlayer.connectedBody = kinematicBodyPlayer;
        }
    }

    //Coffin
    [SerializeField]
    Ataud coffinController;
    [SerializeField]
    Collider2D coffinCollider;
    [SerializeField]
    Transform coffingTakenPos;

    //Chain
    public AnimationCurve curvaRecogerCadena;
    float tiempoRecogerCadena;
    float maxDistanceCadena = 15;
    float maxDistanceCadenaAux;
    [SerializeField]
    float duracionRecogerCadena = 5;
    [SerializeField]
    List<HingeJoint2D> joints;
    [SerializeField]
    float achorDistance;
    [SerializeField]
    List<GameObject> links;
    [SerializeField]
    LineRenderer lineRenderer;
    [SerializeField]
    float posCadenaZ = 1;

    //Coffin actions
    bool taking = false;
    bool coffinThrown = false;

    //ThrowingCoffin
    [SerializeField]
    AnimationCurve throwCurve;
    [SerializeField]
    float throwCoffinTimeMax = 2;
    [SerializeField]
    float throwMaxStrength = 10;
    float throwForce;
    public float verticalCoffinThrowOffset = 0;
    private List<GameObject> trajectoryPoints;
    public int numTrajectoryPoints = 100;
    public GameObject trajectoryPrefeb;



    //Control Chain speed 
    public float distanceNorm;
    public float distSlow;
    [SerializeField]
    private float speedOut;

    private bool throwing;

    void FixedUpdate ()
    {
        playerGround = groundTrigger.IsTouchingLayers(groundMask);

        distanciaJugadorAtaud = transform.position - coffin.position;
        distanciaJugadorCoffin = Vector2.Distance(transform.position, coffin.position);

        CheckKinematics();

        if (InputManager.MainHorizontal() > 0)
            lookingRight = true;
        else if (InputManager.MainHorizontal() < 0)
            lookingRight = false;

        if (!InputManager.RightTrigger())
        {
            if (moving && !throwing) Move();
            if (swing) MoveSwing();
        }

        if (coffinTaken)
        {
            coffinController.SetVelocity(Vector2.zero);
            coffin.position = coffingTakenPos.position;
            tiempoRecogerCadena = 0;
            Reset();
            coffinCollider.enabled = false;
            if (playerGround)
                CheckThrowCoffin();
            else
                UnCheckThrowCoffin();
        }
        else {
            CheckDistanceForSpeed();
            coffinCollider.enabled = true;
            coffinController.SetFreeVelocity();
            TakeCoffin();
        }

        React();

        
    }

    bool left = false;
    bool leftPrevious = false;
    float distanciaMinima;
    [SerializeField]
    float distanciaMinimaNormal=2;
    [SerializeField]
    float distanciaMinimaEscalando=0;
    private void TakeCoffin()
    {
        leftPrevious = left;
        left = InputManager.LeftTrigger();
        bool leftTick = !leftPrevious && left;

        if (leftTick)
        {
            if (distanciaJugadorCoffin > distanceToTakeCoffin)
            {
                tiempoRecogerCadena = duracionRecogerCadena * (1 - distanciaJugadorCoffin / maxDistanceCadena);
            }
            else if(playerGround)
            {
                tiempoRecogerCadena = 0;
                if (coffinTaken)
                {
                    coffinTaken = false;
                }
                else
                {
                    coffinTaken = true;
                }
            }
        }

        if (left)
        {
            if (distanciaJugadorCoffin > distanceToTakeCoffin || !playerGround)
            {
                tiempoRecogerCadena += speedTakeCoffin * Time.deltaTime;
                tiempoRecogerCadena = Mathf.Clamp(tiempoRecogerCadena, 0, duracionRecogerCadena);
                foreach (HingeJoint2D j in joints)
                {
                    j.anchor = new Vector2(Mathf.Lerp(-achorDistance, achorDistance, tiempoRecogerCadena), 0);
                }
            }
            taking = true;
        }else
        {
            taking = false;
            tiempoRecogerCadena -= speedTakeCoffin * Time.deltaTime;
            tiempoRecogerCadena = Mathf.Clamp(tiempoRecogerCadena, 0, duracionRecogerCadena);
            foreach (HingeJoint2D j in joints)
            {
                j.anchor = new Vector2(Mathf.Lerp(-achorDistance, achorDistance, tiempoRecogerCadena), 0);
            }

        }
        tiempoRecogerCadena = Mathf.Clamp(tiempoRecogerCadena , 0, duracionRecogerCadena * (1 - (distanciaJugadorCoffin - 0.5f) / maxDistanceCadena));
       // Debug.Log("EV::"+ curvaRecogerCadena.Evaluate(tiempoRecogerCadena / duracionRecogerCadena));
        //2 distancias minimas, una cuando sube y otra para cuando esta en el suelo, hacer clamp sobre clamp, si el jugador esta en el aire  el jugador en el suelo es 0, 2
        //Debug.Log("CLAMP::" + Mathf.Clamp(Mathf.Clamp(maxDistanceCadena * (1 - curvaRecogerCadena.Evaluate(tiempoRecogerCadena / duracionRecogerCadena)), distanciaJugadorCoffin - 0.05f, maxDistanceCadena), distanciaMinima, maxDistanceCadena)+" Aaa::"+(1 - curvaRecogerCadena.Evaluate(tiempoRecogerCadena / duracionRecogerCadena)));
        jointPlayer.distance = Mathf.Clamp(Mathf.Clamp(maxDistanceCadena * (1 - curvaRecogerCadena.Evaluate(tiempoRecogerCadena / duracionRecogerCadena)), distanciaJugadorCoffin - 0.05f, maxDistanceCadena) ,distanciaMinima, maxDistanceCadena) ;
        jointCoffin.distance = Mathf.Clamp(Mathf.Clamp(maxDistanceCadena * (1 - curvaRecogerCadena.Evaluate(tiempoRecogerCadena / duracionRecogerCadena)), distanciaJugadorCoffin - 0.05f, maxDistanceCadena), distanciaMinima, maxDistanceCadena);
    }

    void Update()
    {
        input = new Vector2(InputManager.MainHorizontal(), System.Convert.ToInt32(InputManager.AButton()));
        ImprimirCadena();
        if (throwing)
        {
            input = new Vector2(0, 0);
            if (InputManager.MainHorizontal() <= 0.1 && InputManager.MainVertical() <= 0.1)
                throwing = false;
        }
        if (taking && playerGround && !coffinTaken)
            input = new Vector2(0, 0);
    }

    void CheckDistanceForSpeed()
    {
        if (distanciaJugadorCoffin <= distanceNorm)
        {
            Reset();
        }
        else if (distanciaJugadorCoffin <= distSlow)
        {
            this.setMoveSpeed(speedOut);
        }
    }

    private Vector2 Apuntar() {

        Vector2 apuntar = InputManager.MainHorizontal() * Vector2.right + InputManager.MainVertical() * Vector2.up;
        if (apuntar.magnitude < 0.2f)
        {
            if (this.LookingRight)
                apuntar = transform.right;
            else
                apuntar = -transform.right;
        }
        if (Mathf.Abs(InputManager.MainVertical()) < 0.2f)
            apuntar += Vector2.up * verticalCoffinThrowOffset;

        return apuntar;
    }


    bool right = false;
    bool rightPrevious = false;

    private void CheckThrowCoffin()
    {
        rightPrevious = right;
        right = InputManager.RightTrigger();
        bool rightDown = right && !rightPrevious;

        if (coffinTaken)
        {
            if (right)
            {
                throwing = true;
                Vector2 apuntar = Apuntar();
                throwForce += Time.deltaTime;
                Vector3 currenpoint = transform.position;
                Vector3 currentVel = apuntar.normalized * throwCurve.Evaluate(Mathf.Clamp01(throwForce / throwCoffinTimeMax)) * throwMaxStrength;
                for (int i = 0; i < numTrajectoryPoints; i++)
                {
                    trajectoryPoints[i].transform.position = currenpoint;
                    currenpoint += currentVel * Time.fixedDeltaTime;
                    float g = coffinController.UpGrav;
                    if (currentVel.y < 0)
                        g = coffinController.DownGrav;
                    currentVel = currentVel.x * Vector2.right + currentVel.y * Vector2.up - Vector2.up * g * Time.fixedDeltaTime;
                }
                for (int i = 1; i < trajectoryPoints.Count; i++)
                {
                    trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
                    RaycastHit2D hitInformation = Physics2D.Raycast(trajectoryPoints[i].transform.position, Camera.main.transform.forward, 1);
                    if (hitInformation.collider != null)
                    {
                        GameObject touchedObject = hitInformation.transform.gameObject;
                        if (touchedObject.layer == 8)
                            break;
                    }
                }
            }else if(rightPrevious)
            {
                coffinTaken = false;
                DoThrow();
                throwForce = 0;
                DisableTrajectory();
            }
        }
    }

    private void DoThrow()
    {
        Vector2 apuntar = Apuntar();
        rb2dc.velocity = apuntar.normalized * throwCurve.Evaluate(Mathf.Clamp01(throwForce / throwCoffinTimeMax)) * throwMaxStrength;
        rb2dc.mass = maxMass;
        coffinThrown = true;
    }

    private void DisableTrajectory()
    {
        for (int i = 0; i < numTrajectoryPoints; i++)
            trajectoryPoints[i].GetComponent<Renderer>().enabled = false;
    }

    private void UnCheckThrowCoffin()
    {
        throwForce = 0;
        DisableTrajectory();
        throwing = false;
    }

    private void Move()
    {
        Vector2 vely = rb2d.velocity.y * Vector2.up;
        if (rb2d.velocity.y >= 0)
            vely -= gravityUp * Time.deltaTime * Vector2.up;
        else
            vely -= gravityDown * Time.deltaTime * Vector2.up;
        if (input.y > 0 && playerGround && !coffinTaken)
        {
            vely = jumpVelocity * Vector2.up;
            jumped = true;
        }

       // Debug.Log("distancia::"+ distanciaJugadorCoffin+ "maxDistanceAtaudCofin:"+ maxDistanceAtaudCofin);
        if (coffinTaken)
        {
            rb2d.velocity = new Vector2(input.x * Time.deltaTime * accelerationXSlow, vely.y);
        }
        else if (distanciaJugadorCoffin >= maxDistanceAtaudCofin)
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

        distanciaMinima = distanciaMinimaNormal;
        if (playerGround && coffinGround)
        {
            rb2dc.constraints = RigidbodyConstraints2D.FreezeRotation;
            controlCoffin.ataudColgando = false;
        }
        else if (playerGround && !coffinGround)
        {
            controlCoffin.ataudColgando = true;
        }
        else if (!playerGround && coffinGround && this.transform.position.y < coffin.position.y)
        {
            rb2dc.constraints = RigidbodyConstraints2D.FreezeRotation;
            controlCoffin.ataudColgando = false;
            //distancias minimas  
            distanciaMinima = distanciaMinimaEscalando;
        }
        else if (!playerGround && !coffinGround)
        {
            controlCoffin.ataudColgando = false;
            swing = false;
            moving = true;
        }
    }

    private void ImprimirCadena()
    {
        lineRenderer.positionCount=links.Count;
        for (int i = 0; i < links.Count; i++)
        {
            Vector3 pos;
            if (i == links.Count - 1) pos= transform.position;
            else if (i == 0)pos=coffin.position;
            else pos=links[i].transform.position;
            pos.z -= posCadenaZ;
            lineRenderer.SetPosition(i, pos);
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
}
