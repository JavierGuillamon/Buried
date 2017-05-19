using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataud : MonoBehaviour {
   
    [SerializeField]
    bool coffinThrown = false;
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    float upGrav;
    public float UpGrav { get { return upGrav; } }
    [SerializeField]
    float downGrav;
    public float DownGrav { get { return downGrav; } }
    public bool ataudColgando;
    public bool coffinTaken;
    [SerializeField]
    private Jugador player;
    [SerializeField]
    Transform coffinVisual;
    [SerializeField]
    private Transform playerTr;
    [SerializeField]
    float massInterpolationSpeed;

    void Update () {/*
        Vector2 direction = Vector2.up;
       if (!coffinTaken)
        {
            if (!player.coffinGround && rb2d.velocity.magnitude > 0 && coffinThrown)
            {
                direction = Vector3.Cross(rb2d.velocity, Vector3.forward);
                transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(rb2d.velocity, Vector3.forward));
            }
            else if (ataudColgando)
            {
                if (transform.position.x > playerTr.position.x)
                    direction = Vector3.forward;
                else
                    direction = -Vector3.forward;
            }
        }
        else
        {
            Vector2 apuntar = InputManager.MainHorizontal() * Vector2.right + InputManager.MainVertical() * Vector2.up;
            if (apuntar.magnitude < 0.2f)
            {
                if (player.LookingRight)
                    apuntar = playerTr.right;
                else
                    apuntar = -playerTr.right;
            }
            if (InputManager.MainVertical() > -0.2f )
                apuntar += Vector2.up * player.verticalCoffinThrowOffset;
            direction = Vector3.Cross(Vector3.forward, apuntar.normalized);
        }
       */
    }

    public void SetVelocity( Vector2 vel) {
        rb2d.velocity = vel;
    }

    public void SetFreeVelocity() {
        float g = upGrav;
        if (rb2d.velocity.y < 0)
            g = downGrav;
        if (ataudColgando && !coffinThrown)
        {
            g = Mathf.Lerp(g, 1, Time.deltaTime * massInterpolationSpeed);
        }
        rb2d.velocity = rb2d.velocity.x * Vector2.right + rb2d.velocity.y * Vector2.up - Vector2.up * g * Time.deltaTime;
    }

    public void CheckThrown() {
        coffinThrown = true;
    }

    public void CheckUnThrown()
    {
        coffinThrown = false;
    }

    [SerializeField]
    float maxAllowedSpeed;

    void OnCollisionStay2D(Collision2D coll) {
        rb2d.velocity = rb2d.velocity.normalized * Mathf.Clamp(rb2d.velocity.magnitude,0, maxAllowedSpeed);
    }
}
