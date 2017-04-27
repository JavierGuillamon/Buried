using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoffinPositionManager : MonoBehaviour {
    public Transform player;
    public Transform coffin;
    public bool coffinUp;
    public bool playerGround;
    public bool coffinGround;
    public Vector2 tr;
    public float deadZone;
    [SerializeField]
    private float speedTakeCoffin = 10;
   // private Vector3 velocity;
    private Controller2D controller;
    private Player pj;

    public ControlManager control;
    public Chain chainScript;
    public Rigidbody2D rb2d;
    public Rigidbody2D rb2dc;

    [SerializeField]
    private float distanceToTakeCoffin = .75f;
    private bool coffinTaken;

    private bool click;

    public ControllCoffin controlCoffin;

    // Use this for initialization
    void Start () {
        controller = GetComponent<Controller2D>();
        pj = GetComponent<Player>();
        click = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        tr = player.position - coffin.position;
        if (tr.y < deadZone) coffinUp = true;
        else coffinUp = false;
        playerGround = player.GetComponent<Controller2D>().collisions.below;
        react();
    }

    void react()
    {       
        if (playerGround && coffinGround)
        {
            rb2dc.mass = 1;
            rb2dc.constraints = RigidbodyConstraints2D.FreezeRotation;

            controlCoffin.ataudColgando = false;
            rb2d.mass = 1;
            control.setSwing(false);
            control.setClimb(false);
            //atraer
            controlCoffin.setCanTake(true);
            if (InputManager.LeftTrigger())
            {
                controlCoffin.setRecogerAtaud(true);
            }
        }
        else if(playerGround && !coffinGround)
        {
            rb2dc.mass = 1;
            rb2dc.constraints = RigidbodyConstraints2D.None;
            controlCoffin.ataudColgando = true;
            rb2d.mass = 100;
            control.setSwing(false);
            control.setClimb(false);
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
        else if(!playerGround && coffinGround)
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
                escalar();
            }
        }else if(!playerGround && !coffinGround)
        {
            rb2dc.mass = 1;

            controlCoffin.ataudColgando = false;
            rb2dc.constraints = RigidbodyConstraints2D.None;
            rb2d.mass = 1;
            control.setClimb(false);
            control.setSwing(false);
            control.setMoving(true);
        }

    }

    [SerializeField]
    DistanceJoint2D joint;

    void escalar()
    {
        control.setSwing(true);
        if (InputManager.LeftTrigger())
        {
           // rb2d.gravityScale = 0;
            control.setClimb(true);
            control.setResize(true);
            joint.distance -= speedTakeCoffin * Time.deltaTime;/*
            Vector3 dir = (coffin.position - player.position).normalized * speedTakeCoffin;
            Vector2 aux = new Vector2((dir.x + Vector3.up.x)/2,(dir.y+Vector3.up.y)/2);
            rb2d.velocity = aux;*/
            click = true;
        }
        else if (InputManager.LeftTriggerUp() && click)
        {
            //rb2d.gravityScale = 1;
            //rb2d.velocity = Vector2.zero;
            control.setClimb(false);
            control.setResize(false);
            click = false;
        }else 
        {
            //soltamos cuerda
           //rb2d.gravityScale = 0;
            control.setClimb(true);
            control.setResize(true);
            joint.distance += speedTakeCoffin * Time.deltaTime;
            joint.distance = Mathf.Clamp(joint.distance,0, 15);/*
            Vector3 dir = (coffin.position - player.position).normalized * speedTakeCoffin;
            Vector2 aux = new Vector2((dir.x+Vector3.down.x)/2,(-dir.y+Vector3.down.y)/2);
            rb2d.velocity = aux;
            */
            click = true;
        }/*
        else if(InputManager.RightTriggerUp() && click)
        {
            //dejamos de soltar cuerda
            rb2d.gravityScale = 1;
            rb2d.velocity = Vector2.zero;
            control.setClimb(false);
            control.setResize(false);
            click = false;
        }*/
    }

    

    public void setCoffinGround(bool aux)
    {
        coffinGround = aux;
    }
}
