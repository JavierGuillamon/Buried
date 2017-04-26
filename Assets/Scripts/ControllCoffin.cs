using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllCoffin : MonoBehaviour {

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float speedOut;
    [SerializeField]
    private Player control;
    [SerializeField]
    private float speedMovCoffin;
    [Tooltip("Tecla con la que se recoge el ataud")]
    [SerializeField]
    private KeyCode takeCoffin;
    [SerializeField]
    private float distanceToTakeCoffin=.75f;
    [SerializeField]
    private float speedTakeCoffin = 10;
    [Tooltip("Velocidad de movimiento cogiendo el ataud")]
    [SerializeField]
    private float moveSpeedWithCoffin;
    [SerializeField]
    private float jumpHeightWithCoffin;
    [SerializeField]
    private float timeJumpWithCoffin;
    [SerializeField]
    private DistanceJoint2D joint;

    [Header("Referencias Scripts")]
    [SerializeField]
    private ControlManager manager;

    private float dist;
    private bool coffinTaken;
    private bool distAlterada;
    private Vector3 targetPosition;
    Rigidbody2D rb;

    [Header("THROW")]
    [SerializeField]
    private KeyCode throwCoffin;
    [SerializeField]
    private float force;
    Vector2 mousePos;
    float mouseDistance;
    public float power = 1;
    float angle;
    public float Vo, g;
    private bool recogerAtaud;
    private bool onSombra;

    float deadZone = 0.2f;
    Vector3 dir;

    public float takeIntervalSec;
    private IEnumerator coroutine;
    private bool playingCoroutine;

    private bool canTake;

    public AnimationCurve curvaRecogerCadena;
    float tiempoRecogerCadena;

    [SerializeField]
    Rigidbody2D body;
    [SerializeField]
    Animator anim;


    [SerializeField]
    float upGrav;

    [SerializeField]
    float downGrav;

    //List<Vector2> trajectoryPoints;
    private List<GameObject> trajectoryPoints;
    public int numTrajectoryPoints=100;
    public GameObject trajectoryPrefeb;

    public bool ataudColgando;

    [SerializeField]
    AnimationCurve throwCurve;
    [SerializeField]
    float throwCoffinTimeMax = 2;
    [SerializeField]
    float throwMaxStrength = 10;
    float throwForce;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        playingCoroutine = false;

        trajectoryPoints = new List<GameObject>();
        for(int i = 0; i < numTrajectoryPoints; i++)
        {
            GameObject dot = Instantiate(trajectoryPrefeb);
            dot.GetComponent<Renderer>().enabled = false;
            trajectoryPoints.Insert(i, dot);
        }
    }

    [SerializeField]
    Collider2D coffinCollider;
	
	void Update () {
        TakeCoffin();
        ThrowCoffin();
        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseDistance = Vector2.Distance(mousePos, transform.position);
        if (coffinTaken)
        {
            coffinCollider.enabled = false;
        }
        else
        {
            coffinCollider.enabled = true;
        }
       
    }

  /*  void detectAndMove()
    {
        targetPosition = new Vector3(target.position.x,target.position.y-0.25f,0);
        dist = Vector3.Distance(target.position, transform.position);
        if (dist > distance)
        {
            control.setMoveSpeed(speedOut);
           
        }
        else
        {
            reset();
            if (coffinTaken)
            {
                manager.setResize(true);
                transform.position = new Vector3(target.position.x, target.position.y + 0.75f, 0);             
                control.setMoveSpeed(moveSpeedWithCoffin);
                control.setJumpHeight(jumpHeightWithCoffin);
                control.setTimeJump(timeJumpWithCoffin);
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }*/
    //true = left, false = right
    public bool leftOrRight()
    {
        bool aux=true;
        if (transform.position.x < target.position.x)
        {
            aux = false;    
            
        }
        if (transform.position.x > target.position.x)
        {
            aux = true;
        }
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        
        return aux;
    }

    public float getSpeedOut()
    {
        return speedOut;
    }

    public void reset()
    {
        control.resetFreeze();
        control.resetSpeed();
        control.resetJumpHeight();
        control.resetTimeJump();
        rb.constraints = RigidbodyConstraints2D.None;
        if (coffinTaken)
        {
            manager.setMoving(true);
            transform.position = new Vector3(target.position.x, target.position.y + 0.75f, 0);
            control.setMoveSpeed(moveSpeedWithCoffin);
            control.setJumpHeight(jumpHeightWithCoffin);
            control.setTimeJump(timeJumpWithCoffin);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void FixedUpdate()
    {
        if (!coffinTaken)
        {
            float g = upGrav;
            if (body.velocity.y < 0)
                g = downGrav;
            if (ataudColgando)
            {
                //Debug.Log("aaaaa");
                g = 0;
            }
            body.velocity = body.velocity.x * Vector2.right + body.velocity.y * Vector2.up - Vector2.up * g * Time.deltaTime;
            //Debug.Log(body.velocity);
        }
    }

    public void TakeCoffin()
    {
        if (InputManager.MainHorizontal() == 0 && canTake)
        {
            if (InputManager.LeftTrigger1())
            {
                if (Vector3.Distance(target.position, transform.position) > distanceToTakeCoffin)
                {

                    tiempoRecogerCadena += Time.deltaTime;
                    Vector3 dir = target.transform.position - transform.position;
                    dir = new Vector3(dir.x,0,0);
                    body.velocity = dir.normalized * speedTakeCoffin * curvaRecogerCadena.Evaluate(tiempoRecogerCadena);                 
                }
            }
            else
            {
                tiempoRecogerCadena = 0;
            }
            if (InputManager.LeftTrigger())
            {
                if (Vector3.Distance(target.position, transform.position) <= distanceToTakeCoffin)
                {
                    coffinTaken = !coffinTaken;
                    if (coffinTaken == false)
                    {
                        transform.position = new Vector3(target.position.x + 0.75f, target.position.y, 0);
                    }
                }
            }
            if (InputManager.LeftTriggerUp())
            {
                recogerAtaud = false;
            }
        }
        anim.SetFloat("Vel",Mathf.Abs(body.velocity.x));
    }
      
    public void ThrowCoffin()
    {
        if (coffinTaken)
        {
            if (InputManager.RightTrigger1())
            {

                Vector2 apuntar = InputManager.MainHorizontal() * Vector2.right + InputManager.MainVertical() * Vector2.up;
                throwForce += Time.deltaTime;

                // trajectoryPoints = new List<Vector2>();
               
              
                Vector3 currenpoint = transform.position;
                Vector3 currentVel = apuntar.normalized * throwCurve.Evaluate(Mathf.Clamp01(throwForce / throwCoffinTimeMax)) * throwMaxStrength;
                for (int i = 0; i < 100; i++)
                {
                    // trajectoryPoints.Add(currenpoint);
                    trajectoryPoints[i].transform.position = currenpoint;

                    currenpoint += currentVel * Time.fixedDeltaTime;
                    float g = upGrav;
                    if (currentVel.y < 0)
                        g = downGrav;
                    currentVel = currentVel.x * Vector2.right + currentVel.y * Vector2.up  - Vector2.up * g * Time.fixedDeltaTime;
                }
                for (int i = 1; i < trajectoryPoints.Count; i++)
                {
                    // Debug.DrawLine(trajectoryPoints[i-1], trajectoryPoints[i]);
                    trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
                    RaycastHit2D hitInformation = Physics2D.Raycast(trajectoryPoints[i].transform.position, Camera.main.transform.forward);
                    if (hitInformation.collider != null)
                    {
                        GameObject touchedObject = hitInformation.transform.gameObject;
                        if (touchedObject.layer == 8)
                            break;
                    }

                }


                transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, apuntar.normalized));              
            }
            else if (InputManager.RightTriggerUp())
            {
                ThrowAux();
                coffinTaken = false;
                throwForce = 0;
                for (int i = 0; i < numTrajectoryPoints; i++)
                    trajectoryPoints[i].GetComponent<Renderer>().enabled = false;
            }
            //Vector3 vel = GetForceFrom(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            
            float jHoriz = InputManager.AuxHorizontal();
            float jVert = InputManager.AuxVertical();
            Vector3 tmp = new Vector3(jHoriz, jVert, 0);
            if (tmp.sqrMagnitude > deadZone)
                dir = tmp;

            //Debug.Log("DIR: " + dir+" MOUSE: "+ Input.mousePosition);          
            //Vector3 vel = GetForceFrom(transform.position, transform.position+dir);
            //float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
            //transform.eulerAngles = new Vector3(0, 0, angle);

            
        }
    }
    /*
        void calculateAngle()
        { 
            Vector2 v2 = mousePos - (Vector2)transform.position;
            v2 = transform.InverseTransformDirection(v2);
            angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;

            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        void calculatePower()
        {
            if (mouseDistance >= MaxDistance)
            {
                power = 1;
            }
            else
            {
                power = (mouseDistance / MaxDistance);
            }
        }
        

    public void Throw()
    {
        calculatePower(); calculateAngle();
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        // Debug.Log("Power: " +power+" Dir: "+dir);
        //Vector3 dir = Quaternion.AngleAxis(Quaternion.Angle(transform.rotation, Quaternion.identity), Vector3.forward)*Vector3.right;
        rb.AddForce(dir * (force * power));
        //rb.AddForce(dir * (force*power));
    }*/

    private void ThrowAux()
    {
        Vector2 apuntar = InputManager.MainHorizontal() * Vector2.right + InputManager.MainVertical() * Vector2.up;
        body.velocity = apuntar.normalized * throwCurve.Evaluate(Mathf.Clamp01(throwForce / throwCoffinTimeMax)) * throwMaxStrength;
       // Debug.Log(body.velocity);
        dir = new Vector3(0,0,0);
    }
    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * power;
    }


    public void setRecogerAtaud(bool aux)
    {
        recogerAtaud = aux;
    }
    public void setOnSombra(bool aux)
    {
        onSombra = aux;
    }

    public void setCanTake(bool aux)
    {
        canTake = aux;
    }

}
