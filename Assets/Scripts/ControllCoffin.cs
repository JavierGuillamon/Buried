﻿using UnityEngine;
using System.Collections;

public class ControllCoffin : MonoBehaviour {

    [SerializeField]
    private float distance;
    [SerializeField]
    public float MaxDistance;
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
    float power;
    float angle;

    [Header("Chain distance detectors")]
    public CircleCollider2D areaMovNormal;
    public CircleCollider2D areaMovSlow;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        areaMovNormal.radius = distance;
        areaMovSlow.radius = MaxDistance;
    }
	
	// Update is called once per frame
	void Update () {
        TakeCoffin();
        ThrowCoffin();

        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseDistance = Vector2.Distance(mousePos, transform.position);
       // detectAndMove();
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
            manager.setResize(true);
            transform.position = new Vector3(target.position.x, target.position.y + 0.75f, 0);
            control.setMoveSpeed(moveSpeedWithCoffin);
            control.setJumpHeight(jumpHeightWithCoffin);
            control.setTimeJump(timeJumpWithCoffin);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void TakeCoffin()
    {
        if (Input.GetKey(takeCoffin))
        {
            if (Vector3.Distance(target.position, transform.position) <= distanceToTakeCoffin)
            {
               /* coffinTaken = !coffinTaken;
                if (coffinTaken == false)
                {
                    transform.position = new Vector3(target.position.x + 0.75f, target.position.y, 0);
                }*/
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
                Throw();
                
            }
                
        }
    }

    void calculateAngle()
    {
        Vector2 v2 = mousePos - (Vector2)transform.position;
        angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
        /*
        if (angle >= 0 && angle < 45)
        {
            if (Mathf.Abs(0 - angle) < Mathf.Abs(45 - angle))
            {
                angle = 0;
               // targetThrow.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y);
            }
            else
            {
                angle = 45;
               // targetThrow.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y + (Mathf.Tan(45) * maxDistance * power) / 4);

            }
        }
        else if (angle >= 45 && angle < 90)
        {
            if (Mathf.Abs(45 - angle) < Mathf.Abs(90 - angle))
            {
                angle = 45;
               // targetThrow.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y + (Mathf.Tan(45) * maxDistance * power) / 4);
            }
            else
            {
                angle = 90;
                //targetThrow.position = new Vector2(transform.position.x, transform.position.y + power * maxH);
            }
        }
        else if (angle >= 90 && angle < 135)
        {
            if (Mathf.Abs(90 - angle) < Mathf.Abs(135 - angle))
            {
                angle = 90;
                //targetThrow.position = new Vector2(transform.position.x, transform.position.y + power * maxH);
            }
            else
            {
                angle = 135;
                //targetThrow.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y + Mathf.Abs((Mathf.Tan(135) * maxDistance * power) / 4));
            }
        }
        else if (angle >= 135 && angle < 180)
        {
            if (Mathf.Abs(135 - angle) < Mathf.Abs(180 - angle))
            {
                angle = 135;
                //targetThrow.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y + Mathf.Abs((Mathf.Tan(135) * maxDistance * power) / 4));
            }
            else
            {
                angle = 180;
                //targetThrow.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y);
            }
        }
        else if (angle < 0 && angle > -45)
        {
            if (Mathf.Abs(0 - angle) < Mathf.Abs(-45 - angle))
            {
                angle = 0;
                //targetThrow.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y);
            }
            else
            {
                angle = -45;
                //targetThrow.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y + (Mathf.Tan(135) * maxDistance * power) / 4);
            }
        }
        else if (angle < -45 && angle > -135)
        {
            if (Mathf.Abs(-45 - angle) < Mathf.Abs(-135 - angle))
            {
                angle = -45;
                //targetThrow.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y + (Mathf.Tan(-45) * maxDistance * power) / 4);
            }
            else
            {
                angle = -135;
                //targetThrow.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y - (Mathf.Tan(-135) * maxDistance * power) / 4);
            }
        }
        else if (angle < -135 && angle > -180)
        {
            if (Mathf.Abs(-135 - angle) < Mathf.Abs(-180 - angle))
            {
                angle = -135;
                //targetThrow.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y - (Mathf.Tan(-135) * maxDistance * power) / 4);
            }
            else
            {
                angle = -180;
                //targetThrow.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y);
            }
        }*/
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
        rb.AddForce(dir * (force*power));
    }

}
