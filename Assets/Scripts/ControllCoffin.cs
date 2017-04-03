using UnityEngine;
using System.Collections;

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
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        TakeCoffin();
        ThrowCoffin();
        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseDistance = Vector2.Distance(mousePos, transform.position);
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

    public void setRecogerAtaud(bool aux)
    {
        recogerAtaud = aux;
    }
     public void setOnSombra(bool aux)
    {
        onSombra = aux;
    }
    public void TakeCoffin()
    {
        if (transform.position.y - target.position.y < 0 && recogerAtaud) GetComponent<Rigidbody2D>().gravityScale = 0;
        else if(!onSombra) GetComponent<Rigidbody2D>().gravityScale = 1;
        if (Input.GetKey(takeCoffin))
        {
            if (Vector3.Distance(target.position, transform.position) <= distanceToTakeCoffin)
            {
            }
            else
            {
               transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speedTakeCoffin * Time.deltaTime);
            }       
        }

        if (Input.GetKeyDown(takeCoffin))
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
    }

    public void ThrowCoffin()
    {
        if (coffinTaken)
        {
            if (Input.GetKeyDown(throwCoffin))
            {
                coffinTaken = false;
                ThrowAux();
            }
            Vector3 vel = GetForceFrom(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
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

    
    //-------------Pruebas Tiro parabolico
   

    private void ThrowAux()
    {
        rb.AddForce(GetForceFrom(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)), ForceMode2D.Impulse);
    }
    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * power;
    }
    

}
