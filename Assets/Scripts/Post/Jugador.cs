using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


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
    [SerializeField]
    float accelerationXNormal;
    [SerializeField]
    float maxVelXSlow;
    [SerializeField]
    float maxVelX;
    [SerializeField]
    float dragX;
    //end

    //PlayerController
    [SerializeField]
    float speedSwing;
    //end

    //PlayerCoffinPositionManager
    [Header("PlayerCoffinPositionManager")]
    public Transform coffin;
    [SerializeField]
    List<GroundDetector> coffinDetectors;
    [SerializeField]
    List<GroundDetector> playerDetectors;
    bool playerGround
    {
        get
        {
            foreach (GroundDetector gd in playerDetectors)
            {
                if (gd.DetectGround())
                {
                    return true;
                }

            }
            return false;
        }
    }
    bool coffinGround
    {
        get
        {
            foreach (GroundDetector gd in coffinDetectors)
            {
                if (gd.DetectGround())
                    return true;
            }
            return false;
        }
    }
   
    [SerializeField]
    private float speedTakeCoffin = 10;
    public Rigidbody2D rb2dc;
    public Ataud controlCoffin;
    //end
    private float distanciaJugadorCoffin;

    bool lookingRight = true;
    public bool LookingRight { get { return lookingRight; } }

    [SerializeField]
    Collider2D groundTrigger;
    [SerializeField]
    LayerMask groundMask;
    Vector2 input;
    
    [SerializeField]
    float gravityUp;
    [SerializeField]
    float gravityDown;
    [SerializeField]
    float maxDistanceAtaudCofin;

    //checkKinematics
    [Header("Kinematics")]
    [SerializeField]
    float maxMass;
    [SerializeField]
    float massChangeDistance = 10;
    [SerializeField]
    Rigidbody2D kinematicBody;
    [SerializeField]
    Rigidbody2D kinematicBodyPlayer;
    [SerializeField]
    float massInterpolationSpeed;
    //Coffin
    [SerializeField]
    Ataud coffinController;
    [SerializeField]
    Collider2D coffinCollider;
    [SerializeField]
    Collider2D coffinCollider1;
    [SerializeField]
    Collider2D coffinCollider2;
    [SerializeField]
    Transform coffingTakenPos;

    //Chain
    [Header("Cadena")]
    public AnimationCurve curvaRecogerCadena;
    float tiempoRecogerCadena;
    float maxDistanceCadena = 15;
    [SerializeField]
    float duracionRecogerCadena = 5;

    //Coffin actions
    bool taking = false;

    //ThrowingCoffin
    [Header("Lanzamiento")]
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
    [SerializeField]
    private float distanceToTakeCoffin = .75f;
    private bool coffinTaken;

    private bool throwing;
    bool left = false;
    bool leftPrevious = false;
    bool right = false;
    bool rightPrevious = false;
    float distanciaMinima;
    [SerializeField]
    float distanciaMinimaNormal = 2;
    [SerializeField]
    float distanciaMinimaEscalando = 2;

    [SerializeField]
    bool tirandoDeMas = false;
    [SerializeField]
    ParticleSystem sweatParticles;
    [SerializeField]
    ParticleSystem launchParticles;
    [SerializeField]
    ParticleSystem jumpParticles;
    [SerializeField]
    ParticleSystem runParticles;

    public DistanceJoint2D firsjoint;
    public DistanceJoint2D firsjointCoffin;
    public Animator animator;
    public GameObject VisualGO;

    [Space(20)]
    [SerializeField] LayerMask throwLayerCollision;
    [SerializeField] Transform camPos;
    [SerializeField] float speedTakingCoffin;
    [SerializeField] AnimationCurve takeCoffinCurve;
    [SerializeField] GameObject throwParticles;
    private Vector3 oldCoffinPos;
    private float takeCoffinTimer;
    private LineRenderer throwLine;
    

    void Start ()
    {
        throwLine = GetComponent<LineRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        moving = true;
        trajectoryPoints = new List<GameObject>();
        throwLine.positionCount = numTrajectoryPoints;
        //for (int i = 0; i < numTrajectoryPoints; i++)
        //trajectoryPoints.Add(Instantiate(trajectoryPrefeb));
        DisableTrajectory();
    }

    void CheckKinematics()
    {
        rb2dc.mass = Mathf.Lerp(rb2dc.mass, 1, Time.deltaTime * massInterpolationSpeed);
        this.rb2d.mass = Mathf.Lerp(rb2d.mass, 1, Time.deltaTime * massInterpolationSpeed);

        if (coffinTaken)
        {
            chain.pullFromCoffin = false;
            chain.pullFromPlayer = false;

        } else if (this.playerGround && this.coffinGround)
        {
            if (taking)
            {
                chain.pullFromCoffin = true;
                chain.pullFromPlayer = false;
            }
            else
            {
                chain.pullFromCoffin = true;
                chain.pullFromPlayer = false;
            }
        }
        else if (!this.playerGround && this.coffinGround)
        {
            chain.pullFromCoffin = false;
            chain.pullFromPlayer = true;
            
        }
        else if (this.playerGround && !this.coffinGround)
        {
            chain.pullFromCoffin = true;
            chain.pullFromPlayer = false;
        }
        else
        {
            chain.pullFromCoffin = true;
            chain.pullFromPlayer = false;
        }
    }

    [Space(20)]
    public bool movSlow;
    public float lastY;
    public float fallHeight;
    public float deathHeight;
    bool tensandoCadena;
    bool contadorDeGiro=false;
    bool dividirVelocidadAtaud = false;
    public float divisionVelocidadDelAtaudAlAterrizar=1.5f;
    void FixedUpdate ()
    {
        
        distanciaJugadorCoffin = Vector2.Distance(transform.position, coffin.position);

        if (distanciaJugadorCoffin > maxDistanceAtaudCofin || coffinGround) {
            coffinController.CheckUnThrown();
        }

        CheckKinematics();

        if (InputManager.MainHorizontal() > 0)
        {
            if (!lookingRight) contadorDeGiro = true;
            lookingRight = true;
        }
        else if (InputManager.MainHorizontal() < 0)
        {
            if (lookingRight) contadorDeGiro = true;
            lookingRight = false;
        }

        if (!InputManager.RightTrigger())
        {
            if (moving && !throwing && !swing) Move();
            if (swing) MoveSwing();
        }else
        {
            MoveHolding();
        }

        if (coffinTaken)
        {
            coffinController.SetVelocity(Vector2.zero);
            //coffin.position = coffingTakenPos.position;
            takeCoffinTimer += Time.deltaTime * speedTakingCoffin;
            coffin.position = Vector3.Lerp(oldCoffinPos, coffingTakenPos.position, takeCoffinCurve.Evaluate(takeCoffinTimer));
            desiredDistance = maxDistanceCadena;
            coffinCollider.enabled = false;
            coffinCollider1.enabled = false;
            coffinCollider2.enabled = false;
            if (playerGround)
            {
                CheckThrowCoffin();
            }
            else
            {
                UnCheckThrowCoffin();
            }
        }
        else {
            coffinCollider.enabled = true;
            coffinCollider1.enabled = true;
            coffinCollider2.enabled = true;
            coffinController.SetFreeVelocity();
            TakeCoffin();
        }

        React();

        if ((tirandoDeMas || movSlow )&& !sweatParticles.isPlaying)
        {
            sweatParticles.Play();
        }
        else if (!tirandoDeMas && !movSlow && sweatParticles.isPlaying) {
            sweatParticles.Stop();
        }

        //death fall
        if (!playerGround)
        {
            float Ydistance = transform.position.y - lastY;
            if (Ydistance < 0)
                fallHeight += Ydistance;
            else
                fallHeight = 0;
        }
        else
        {
            if (-fallHeight >= deathHeight)
            {
                fallHeight = 0;
                Debug.Log("HAS MUERTO");
                //TODO cambiar este numero por el de la escena real, que se pueda cambiar por parametro
                SceneManager.LoadScene(3);
            }
            fallHeight = 0;
        }
        lastY = transform.position.y;
        if (coffinGround && dividirVelocidadAtaud)
        {
            rb2dc.velocity = new Vector2(rb2dc.velocity.x/ divisionVelocidadDelAtaudAlAterrizar, rb2dc.velocity.y);
            dividirVelocidadAtaud = false;
        }
        else if (!coffinGround && !dividirVelocidadAtaud)
        {
            dividirVelocidadAtaud = true;
        }
        playAnimations();
    }

    float maxDistanceCadenaAux;
    bool canResetDistance = true;
    [SerializeField]
    Chain chainScript;

    private void agarrarAtaud()
    {
        if (distanciaJugadorCoffin <= distanceToTakeCoffin + 0.66f && playerGround)
        {
            if (coffinTaken)
                coffinTaken = false;
            else
            {
                oldCoffinPos = coffin.position;
                takeCoffinTimer = 0;
                coffinTaken = true;
                chainScript.ResetCorners();
            }
        }
    }

    private void TakeCoffin()
    {
        leftPrevious = left;
        left = InputManager.LeftTrigger();
        bool leftTick = !leftPrevious && left;

        if (leftTick)
        {
            desiredDistance = chain.ChainLength;
            tiempoRecogerCadena = 0;
            agarrarAtaud();   
        }

        if (left)
        {
            if (distanciaJugadorCoffin >= distanceToTakeCoffin || !playerGround)
            {
                //Debug.Log("dentro");
                tiempoRecogerCadena += Time.deltaTime;
                desiredDistance = desiredDistance - speedTakeCoffin * Time.deltaTime * curvaRecogerCadena.Evaluate(tiempoRecogerCadena);
                taking = true;
                agarrarAtaud();
            }
            else
                agarrarAtaud();
        }else 
        {
                taking = false;
            if (this.playerGround && this.coffinGround)
            {
                desiredDistance = maxDistanceCadena;
            }
            else
            {
                tiempoRecogerCadena -=  Time.deltaTime;
                desiredDistance = desiredDistance + speedTakeCoffin * Time.deltaTime * curvaRecogerCadena.Evaluate(tiempoRecogerCadena);
            }
        }

        tirandoDeMas = chain.ChainLength >= maxDistanceCadena;
        desiredDistance= Mathf.Clamp(desiredDistance , distanciaMinima, maxDistanceCadena);
        chain.currDistance = desiredDistance;

    }

    float desiredDistance;

    [SerializeField]
    Chain chain;

    bool aux = false;

    [SerializeField]
    Stack<GameObject> linksBloqueados = new Stack<GameObject>();
    [SerializeField]
    Stack<GameObject> linksBloqueadosCoffin = new Stack<GameObject>();
    bool tick=false;
    public float forceToTakeLink = 15;
    [SerializeField]
    Transform ChainParent;
    ArrayList list = new ArrayList();
    
    void Update()
    {
        // Particulas
        if (InputManager.AButton() && playerGround && coffinTaken == false)
        {
            jumpParticles.Play();
            animator.SetBool("Jump", true);
        }
        if (rb2d.velocity.x != 0 && playerGround)
        {
            runParticles.Play();
        }
        else
        {
            runParticles.Stop();
        }
        //end
        input = new Vector2(InputManager.MainHorizontal(), System.Convert.ToInt32(InputManager.AButton()));
        if (throwing)
        {
            input = new Vector2(0, 0);
            if (InputManager.MainHorizontal() == 0 && InputManager.MainVertical() == 0)
                throwing = false;
           
        }
        if (taking && playerGround && !coffinTaken)
            input = new Vector2(0, 0);
    }

    public float verticalOffset;
    private Vector2 Apuntar() {
        Vector2 apuntar = InputManager.MainHorizontal() * Vector2.right + InputManager.MainVertical() * verticalOffset * Vector2.up;
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

    private void CheckThrowCoffin()
    {
        rightPrevious = right;
        right = InputManager.RightTrigger();
        bool rightDown = right && !rightPrevious;

        if (coffinTaken)
        {
            if (right)
            {
                throwParticles.SetActive(true);
                throwing = true;
                Vector2 apuntar = Apuntar();
                throwForce += Time.deltaTime;
                Vector3 currenpoint = transform.position;
                //Vector3 currentVel = apuntar.normalized * throwCurve.Evaluate(Mathf.Clamp01(throwForce / throwCoffinTimeMax)) * throwMaxStrength;
                Vector3 currentVel = apuntar.normalized * throwCurve.Evaluate(throwForce / throwCoffinTimeMax) * throwMaxStrength;
                throwLine.enabled = true;
                for (int i = 0; i < numTrajectoryPoints; i++)
                {
                    //trajectoryPoints[i].transform.position = currenpoint;
                    throwLine.SetPosition(i, currenpoint);
                    currenpoint += currentVel * Time.fixedDeltaTime;
                    float g = coffinController.UpGrav;
                    if (currentVel.y < 0)
                        g = coffinController.DownGrav;
                    currentVel = currentVel.x * Vector2.right + currentVel.y * Vector2.up - Vector2.up * g * Time.fixedDeltaTime;
                }

                for (int i = 0; i < numTrajectoryPoints -1; i++)
                {
                    RaycastHit2D hit = Physics2D.Raycast(throwLine.GetPosition(i), throwLine.GetPosition(i + 1) - throwLine.GetPosition(i), Vector3.Distance(throwLine.GetPosition(i + 1), throwLine.GetPosition(i)), throwLayerCollision);
                    Debug.DrawRay(throwLine.GetPosition(i), throwLine.GetPosition(i + 1) - throwLine.GetPosition(i), Color.yellow);
                    if (hit.collider != null)
                    {
                        throwParticles.transform.position = hit.point;
                        Debug.DrawRay(hit.point, hit.normal, Color.cyan);
                        if (hit.normal.y > 0)
                            throwParticles.transform.eulerAngles = new Vector3(0, 0 , 0);
                        else if (hit.normal.y < 0)
                            throwParticles.transform.eulerAngles = new Vector3(-180, 0, 0);
                        else if (hit.normal.x < 0)
                            throwParticles.transform.eulerAngles = new Vector3(0, 0, 90);
                        else if (hit.normal.x > 0)
                            throwParticles.transform.eulerAngles = new Vector3(0, 0, -90);
                        break;
                    }
                }
                rb2dc.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(apuntar.normalized, Vector3.forward));
            }
            else if (rightPrevious)
            {
                coffinTaken = false;
                DoThrow();
                throwForce = 0;
                DisableTrajectory();
                throwParticles.SetActive(false);
            }
        }
    }

    private void DoThrow()
    {
        Vector2 apuntar = Apuntar();
        rb2dc.velocity = apuntar.normalized * throwCurve.Evaluate(Mathf.Clamp01(throwForce / throwCoffinTimeMax)) * throwMaxStrength;
        rb2dc.mass = maxMass;
        launchParticles.Play();
        coffinController.CheckThrown();
    }

    private void DisableTrajectory()
    {
        /*for (int i = 0; i < numTrajectoryPoints; i++)
            trajectoryPoints[i].GetComponent<Renderer>().enabled = false;*/
        throwLine.enabled = false;
    }

    private void UnCheckThrowCoffin()
    {
        throwForce = 0;
        DisableTrajectory();
        throwing = false;
    }

    float vely;
    [SerializeField]
    float JumpSpeed = 5;
    [SerializeField]
    float JumpCutOff = 0.5f;
    bool jumped = false;

    private void Move()
    {
        if (!InputManager.LeftTrigger() || (!playerGround&&InputManager.LeftTrigger()))
        {
            vely = rb2d.velocity.y;
            if (InputManager.AButton() && playerGround && !coffinTaken && !jumped)
            {
                vely = JumpSpeed;
                desiredDistance = chain.ChainLength;
                jumped = true;
            }
            if (!InputManager.AButton() && vely > JumpCutOff)
            {
                Debug.Log("cutting");
                vely = JumpCutOff;
                jumped = false;
            }
            if (!InputManager.AButton())
                jumped = false;
            if (vely >= 0)
                vely -= gravityUp * Time.deltaTime;
            else
                vely -= gravityDown * Time.deltaTime;


            if (coffinTaken)
            {
                rb2d.velocity += Vector2.right * Time.deltaTime * accelerationXNormal * InputManager.MainHorizontal();
                movSlow = true;
            }
            else if (distanciaJugadorCoffin >= chain.currDistance - 2)
            {
                if (transform.position.x > coffin.position.x && InputManager.MainHorizontal() > 0)
                {
                    rb2d.velocity += Vector2.right * Time.deltaTime * accelerationXNormal * InputManager.MainHorizontal();
                    movSlow = true;
                }
                else if (transform.position.x < coffin.position.x && InputManager.MainHorizontal() < 0)
                {
                    rb2d.velocity += Vector2.right * Time.deltaTime * accelerationXNormal * InputManager.MainHorizontal();
                    movSlow = true;
                }
                else
                {
                    rb2d.velocity += Vector2.right * Time.deltaTime * accelerationXNormal * InputManager.MainHorizontal();
                    movSlow = false;
                }
            }
            else
            {
                rb2d.velocity += Vector2.right * Time.deltaTime * accelerationXNormal * InputManager.MainHorizontal();
                movSlow = false;
            }
            if (movSlow)
                rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -maxVelXSlow, maxVelXSlow), rb2d.velocity.y);
            else
                rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -maxVelX, maxVelX), rb2d.velocity.y);
            rb2d.velocity = new Vector2(rb2d.velocity.x * dragX, vely);
        }
        else
        {
               rb2d.velocity = new Vector2(0,0);
        }
    }

    void MoveHolding()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x * dragX, vely) - gravityDown * Time.deltaTime * Vector2.up;
    }

    private void MoveSwing()
    {
        Vector3 n = chain.chainNormal;
        Vector3 chainRight = Vector3.Cross(n,Vector3.forward);
        rb2d.velocity += (Vector2)chainRight * InputManager.MainHorizontal() * speedSwing * Time.deltaTime - gravityDown * Time.deltaTime * Vector2.up;
        
    }
    
    private void React()
    {
        distanciaMinima = distanciaMinimaNormal;
        if (playerGround && coffinGround)
        {
            controlCoffin.ataudColgando = false;
            swing = false;
        }
        else if (playerGround && !coffinGround)
        {
            controlCoffin.ataudColgando = true;
            swing = false;
        }
        else if (!playerGround && coffinGround && this.transform.position.y < coffin.position.y && rb2d.velocity.y <= 0)
        {
            controlCoffin.ataudColgando = false;
            distanciaMinima = distanciaMinimaEscalando;
            swing = true;
            if (((this.transform.position.y - coffin.position.y) > -2)&& InputManager.LeftTrigger())
            {
                //chapuzilla para solucionar el fallo de terminar de trepar en una plataforma. TODO: mejorar 
                this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y+.5f,this.transform.position.z);
            }
            
        }
        else if (!playerGround && !coffinGround)
        {
            controlCoffin.ataudColgando = false;
            swing = false;
            moving = true;
        }
    }
    
    private void playAnimations()
    {
        animator.SetFloat("InputX",input.x);
        animator.SetFloat("InputY", vely);
        if (contadorDeGiro)
        {
            VisualGO.transform.Rotate(0, 180, 0);
            contadorDeGiro = false;
        }
        animator.SetBool("Ataud", coffinTaken);
        if (!playerGround && coffinGround &&(coffin.position.y>transform.position.y))
        {
            animator.SetBool("Climbing", true);
            animator.SetBool("TakingAtaud", taking);
        }
        else
        {
            animator.SetBool("Climbing", false);
            animator.SetBool("TakingAtaud", taking);
            if (taking)
            {
                if (lookingRight && coffin.position.x < transform.position.x)
                {
                    lookingRight = false;
                    VisualGO.transform.Rotate(0, 180, 0);
                }
                else if (!lookingRight && coffin.position.x > transform.position.x)
                {
                    lookingRight = true;
                    VisualGO.transform.Rotate(0, 180, 0);
                }
            }
        }
        if (playerGround)
            animator.SetBool("Jump", false);
    }
}
