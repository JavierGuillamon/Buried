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
    [SerializeField]
    private Jugador controlPlayer;
   
    public float distanceNorm;
    public float distSlow;
    //end

    // Use this for initialization
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
	
	// Update is called once per frame
	void Update () {
        TakeCoffin();
        if (coffinTaken)
            coffinCollider.enabled = false;      
        else
            coffinCollider.enabled = true;
        
        if (Vector2.Distance(transform.position, playerTr.position) <= distanceNorm)
        {
            Reset();
        }
        else 
        {
            controlPlayer.setMoveSpeed(speedOut);
        }
    }

    void FixedUpdate()
    {
        ThrowCoffin();
        if (!coffinTaken)
        {
            float g = upGrav;
            if (rb2d.velocity.y < 0)
                g = downGrav;
            if (ataudColgando)
            {
                g = 0;
            }
            rb2d.velocity = rb2d.velocity.x * Vector2.right + rb2d.velocity.y * Vector2.up - Vector2.up * g * Time.deltaTime;
        }
    }

    private void TakeCoffin()
    {
        if (InputManager.MainHorizontal() == 0 && canTake)
        {
            if (InputManager.LeftTrigger())
            {
                if (Vector3.Distance(playerTr.position, transform.position) > distanceToTakeCoffin)
                {

                    tiempoRecogerCadena += Time.deltaTime;
                    Vector3 dir = playerTr.transform.position - transform.position;
                    dir = new Vector3(dir.x, 0, 0);
                    rb2d.velocity = dir.normalized * speedTakeCoffin * curvaRecogerCadena.Evaluate(tiempoRecogerCadena);
                }
            }
            else
            {
                tiempoRecogerCadena = 0;
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
        anim.SetFloat("Vel", Mathf.Abs(rb2d.velocity.x));
    }

    private void ThrowCoffin()
    {
        if (coffinTaken)
        {
            if (InputManager.RightTrigger())
            {
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                Vector2 apuntar = InputManager.MainHorizontal() * Vector2.right + InputManager.MainVertical() * Vector2.up;
                throwForce += Time.deltaTime;

                Debug.Log(transform.position);
                Vector3 currenpoint = transform.position;
                Vector3 currentVel = apuntar.normalized * throwCurve.Evaluate(Mathf.Clamp01(throwForce / throwCoffinTimeMax)) * throwMaxStrength;
                for (int i = 0; i < 100; i++)
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
                }
                transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, apuntar.normalized));
            }
            else if (InputManager.RightTriggerUp())
            {
                rb2d.constraints = RigidbodyConstraints2D.None;
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

    private void ThrowAux()
    {
        Vector2 apuntar = InputManager.MainHorizontal() * Vector2.right + InputManager.MainVertical() * Vector2.up;
        rb2d.velocity = apuntar.normalized * throwCurve.Evaluate(Mathf.Clamp01(throwForce / throwCoffinTimeMax)) * throwMaxStrength;
        dir = new Vector3(0, 0, 0);
    }

    private void Reset()
    {
        player.Reset();
        rb2d.constraints = RigidbodyConstraints2D.None;
        if (coffinTaken)
        {
            transform.position = new Vector3(playerTr.position.x, playerTr.position.y + 0.75f, 0);
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
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
