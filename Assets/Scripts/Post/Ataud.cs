using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataud : MonoBehaviour {
    //Control coffin
    [SerializeField]
    private Transform playerTr;
    [SerializeField]
    private float speedOut;
    [SerializeField]
    private Jugador player;
    [SerializeField]
    private float distanceToTakeCoffin = .75f;
    [SerializeField]
    private float speedTakeCoffin = 10;
    private bool coffinTaken;
    Rigidbody2D rb2d;
    [Header("THROW")]
    public float power = 1;
    private bool recogerAtaud;
    private bool onSombra;
    float deadZone = 0.2f;
    Vector3 dir;
    private bool canTake;
    public AnimationCurve curvaRecogerCadena;
    float tiempoRecogerCadena;    
    [SerializeField]
    Animator anim;
    [SerializeField]
    float upGrav;
    [SerializeField]
    float downGrav;   
    private List<GameObject> trajectoryPoints;
    public int numTrajectoryPoints = 100;
    public GameObject trajectoryPrefeb;
    public bool ataudColgando;
    [SerializeField]
    AnimationCurve throwCurve;
    [SerializeField]
    float throwCoffinTimeMax = 2;
    [SerializeField]
    float throwMaxStrength = 10;
    float throwForce;
    [SerializeField]
    Collider2D coffinCollider;
    //end

    //Control Chain speed 
    public float distanceNorm;
    public float distSlow;
    //end
    
    void Start () {
        canTake = true;
        rb2d = GetComponent<Rigidbody2D>();

        trajectoryPoints = new List<GameObject>();
        for (int i = 0; i < numTrajectoryPoints; i++)
        {
            GameObject dot = Instantiate(trajectoryPrefeb);
            dot.GetComponent<Renderer>().enabled = false;
            trajectoryPoints.Insert(i, dot);
        }
    }

    [SerializeField]
    Transform coffinVisual;

	void Update () {
        ImprimirCadena();
        TakeCoffin();
        if (coffinTaken)
            coffinCollider.enabled = false;      
        else
            coffinCollider.enabled = true;
        
        if (Vector2.Distance(transform.position, playerTr.position) <= distanceNorm)
        {
            Reset();
        }
        else if (Vector2.Distance(transform.position, playerTr.position) <= distSlow)
        {
            player.setMoveSpeed(speedOut);
        }
        if (!coffinTaken)
        {
            if (player.coffinGround)
            {
                coffinVisual.localRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            }
            else if (rb2d.velocity.magnitude > 0)
            {
                coffinVisual.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(rb2d.velocity, Vector3.forward));

            }
            else if (ataudColgando)
            {
                if (transform.position.x > playerTr.position.x)
                    coffinVisual.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
                else
                    coffinVisual.rotation = Quaternion.LookRotation(Vector3.forward, -Vector3.right);
            }
        }
        else {

            Vector2 apuntar = InputManager.MainHorizontal() * Vector2.right + InputManager.MainVertical() * Vector2.up;
            if (apuntar.magnitude < 0.2f)
            {
                if (player.LookingRight)
                    apuntar = playerTr.right;
                else
                    apuntar = -playerTr.right;
            }
            if (Mathf.Abs(InputManager.MainVertical()) < 0.2f)
                apuntar += Vector2.up * verticalCoffinThrowOffset;
            coffinVisual.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, apuntar.normalized));
        }
        
    }

    void CheckKinematics() {
        if (player.playerGround && player.coffinGround)
        {
            if (taking)
            {
                firstJoint.connectedBody = rb2d;
                lastJoint.connectedBody = kinematicBodyPlayer;
            }
            else
            {
                firstJoint.connectedBody = rb2d;
                lastJoint.connectedBody = kinematicBodyPlayer;
            }
            coffinThrown = false;/*
            if (player.jumped && player.rb2d.velocity.y <= 0)
                player.jumped = false;*/
        }
        else if (!player.playerGround && player.coffinGround)
        {
            if (player.jumped)
            {
                firstJoint.connectedBody = rb2d;
                lastJoint.connectedBody = kinematicBodyPlayer;
            }
            else
            {
                firstJoint.connectedBody = kinematicBody;
                lastJoint.connectedBody = player.rb2d;
            }

            if (Vector2.Distance(rb2d.position, player.rb2d.position) > 10)
            {
                player.jumped = false;
            }
            coffinThrown = false;
        }
        else if (player.playerGround && !player.coffinGround)
        {
            if (!coffinThrown)
            {
                firstJoint.connectedBody = rb2d;
                lastJoint.connectedBody = kinematicBodyPlayer;
            }
            if (rb2d.velocity.y <= 0 || Vector2.Distance(rb2d.position,player.rb2d.position) > 10)
            {
                firstJoint.connectedBody = rb2d;
                coffinThrown = false;
            }
            if (player.jumped && player.rb2d.velocity.y <= 0)
                player.jumped = false;
        }
        else
        {
            firstJoint.connectedBody = rb2d;
            lastJoint.connectedBody = player.rb2d;
        }

    }

    void FixedUpdate()
    {
        CheckKinematics();
        ThrowCoffin();
        if (!coffinTaken)
        {
            float g = upGrav;
            if (rb2d.velocity.y < 0.1)
                g = downGrav;
            if (ataudColgando)
            {
                g = 0;
            }
            //Debug.Log("G:: " + g+ " vel:"+ rb2d.velocity.y);
            rb2d.velocity = rb2d.velocity.x * Vector2.right + rb2d.velocity.y * Vector2.up - Vector2.up * g * Time.deltaTime;
        }
    }

    [SerializeField]
    DistanceJoint2D firstJoint;
    [SerializeField]
    DistanceJoint2D lastJoint;
    [SerializeField]
    List<DistanceJoint2D> joints;
    [SerializeField]
    float duracionRecogerCadena =5;
    [SerializeField]
    float achorDistance;
    [SerializeField]
    float jointDistance;
    [SerializeField]
    List<GameObject> links;
    [SerializeField]
    LineRenderer lineRenderer;

    bool taking = false;

    private void ImprimirCadena()
    {
        lineRenderer.numPositions=links.Count;
        for (int i = 0; i < links.Count; i++)
        {
            Vector3 pos;
            if (i == links.Count - 1) pos= playerTr.position;
            else if (i == 0)pos=transform.position;
            else pos=links[i].transform.position;
            pos.z += 10;
            lineRenderer.SetPosition(i, pos);
        }
    }

    private void TakeCoffin()
    {
        if (InputManager.MainHorizontal() == 0 /*&& canTake*/)
        {
            if (InputManager.LeftTrigger())
            {
                if (Vector3.Distance(playerTr.position, transform.position) > distanceToTakeCoffin)
                {

                    tiempoRecogerCadena += Time.deltaTime / duracionRecogerCadena;
                    tiempoRecogerCadena = Mathf.Clamp01(tiempoRecogerCadena);
                    foreach (DistanceJoint2D j in joints)
                    {
                        j.anchor = new Vector2(Mathf.Lerp(-achorDistance, achorDistance, tiempoRecogerCadena), 0);
                        j.distance = 0.001f;
                    }                   
                    /*
                    Vector3 dir = playerTr.transform.position - transform.position;
                    //dir = new Vector3(dir.x, 0, 0);
                    rb2d.velocity = dir.normalized * speedTakeCoffin * curvaRecogerCadena.Evaluate(tiempoRecogerCadena);
                    //Debug.Log("VEL::" + rb2d.velocity+" DIR: "+dir.normalized);*/
                }
                taking = true;
            }
            else
            {
                taking = false;
                tiempoRecogerCadena -= Time.deltaTime / duracionRecogerCadena;
                tiempoRecogerCadena = Mathf.Clamp01(tiempoRecogerCadena);
                foreach (DistanceJoint2D j in joints)
                {
                    j.anchor = new Vector2(Mathf.Lerp(-achorDistance, achorDistance, tiempoRecogerCadena), 0);
                    j.distance = jointDistance;
                }
            }
            if (InputManager.LeftTriggerDown())
            {
                if (Vector3.Distance(playerTr.position, transform.position) <= distanceToTakeCoffin)
                {
                    coffinTaken = !coffinTaken;
                    player.setCoffinTaken(coffinTaken);
                    if (coffinTaken == false)
                    {
                        transform.position = new Vector3(playerTr.position.x + 0.75f, playerTr.position.y, 0);
                    }
                }
            }
            if (InputManager.LeftTriggerUp())
            {
                recogerAtaud = false;
            }
        }
        else
        {
            tiempoRecogerCadena -= Time.deltaTime / duracionRecogerCadena;
            tiempoRecogerCadena = Mathf.Clamp01(tiempoRecogerCadena);
            foreach (DistanceJoint2D j in joints)
            {
                j.anchor = new Vector2(Mathf.Lerp(-0.5f, 0.5f, tiempoRecogerCadena), 0);

            }
            taking = false;
        }
        anim.SetFloat("Vel", Mathf.Abs(rb2d.velocity.x));
    }

    [SerializeField]
    float verticalCoffinThrowOffset = 0;

    private void ThrowCoffin()
    {
        if (coffinTaken)
        {
            if (InputManager.RightTrigger())
            {
                //rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                Vector2 apuntar = InputManager.MainHorizontal() * Vector2.right + InputManager.MainVertical() * Vector2.up;
                if (apuntar.magnitude < 0.2f)
                {
                    if(player.LookingRight)
                        apuntar = playerTr.right;
                    else
                        apuntar = -playerTr.right;
                }
                if (Mathf.Abs(InputManager.MainVertical()) < 0.2f)
                    apuntar += Vector2.up * verticalCoffinThrowOffset;



                throwForce += Time.deltaTime;
                
                Vector3 currenpoint = transform.position;
                Vector3 currentVel = apuntar.normalized * throwCurve.Evaluate(Mathf.Clamp01(throwForce / throwCoffinTimeMax)) * throwMaxStrength;
                for (int i = 0; i < numTrajectoryPoints; i++)
                {
                    trajectoryPoints[i].transform.position = currenpoint;
                    currenpoint += currentVel * Time.fixedDeltaTime;
                    float g = upGrav;
                    if (currentVel.y < 0)
                        g = downGrav;
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
                    //Debug.Log("aaa:: " + Vector3.Distance(trajectoryPoints[i].transform.position, playerTr.position));
                    
                       
                }
            }
            else if (InputManager.RightTriggerUp())
            {
                //rb2d.constraints = RigidbodyConstraints2D.None;
                ThrowAux();
                coffinTaken = false;
                player.setCoffinTaken(coffinTaken);
                throwForce = 0; 
                for (int i = 0; i < numTrajectoryPoints; i++)
                    trajectoryPoints[i].GetComponent<Renderer>().enabled = false;
            }
            float jHoriz = InputManager.AuxHorizontal();
            float jVert = InputManager.AuxVertical();
            Vector3 tmp = new Vector3(jHoriz, jVert, 0);
            if (tmp.sqrMagnitude > deadZone)
                dir = tmp;
        }
    }

    [SerializeField]
    Rigidbody2D kinematicBody;
    [SerializeField]
    Rigidbody2D kinematicBodyPlayer;

    bool coffinThrown = false;

    private void ThrowAux()
    {
        Vector2 apuntar = InputManager.MainHorizontal() * Vector2.right + InputManager.MainVertical() * Vector2.up;
        if (apuntar.magnitude < 0.2f)
        {
            if (player.LookingRight)
                apuntar = playerTr.right;
            else
                apuntar = -playerTr.right;
        }
        if (Mathf.Abs(InputManager.MainVertical()) < 0.2f)
            apuntar += Vector2.up * verticalCoffinThrowOffset;
        rb2d.velocity = apuntar.normalized * throwCurve.Evaluate(Mathf.Clamp01(throwForce / throwCoffinTimeMax)) * throwMaxStrength;
        firstJoint.connectedBody = kinematicBody;
        dir = new Vector3(0, 0, 0);
        coffinThrown = true;
    }

    private void Reset()
    {
        player.Reset();
        //rb2d.constraints = RigidbodyConstraints2D.None;
        if (coffinTaken)
        {
            transform.position = new Vector3(playerTr.position.x, playerTr.position.y + 0.75f, 0);
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public bool leftOrRight()
    {
        bool aux = true;
        if (transform.position.x < playerTr.position.x)
        {
            aux = false;

        }
        if (transform.position.x > playerTr.position.x)
        {
            aux = true;
        }
        rb2d.constraints = RigidbodyConstraints2D.FreezePosition;

        return aux;
    }

    public void setCanTake(bool aux)
    {
        canTake = aux;
    }
    public void setRecogerAtaud(bool aux)
    {
        recogerAtaud = aux;
    }
}
