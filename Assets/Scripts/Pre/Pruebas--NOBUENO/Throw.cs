using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField]
    private KeyCode throwCoffin;
    public bool coffinTaken = true;


    public Transform target;
    public float maxH = 5;
    float h;
    public float gravity = -9.8f;
    public float maxDistance;
    Rigidbody2D rb;
    Vector2 mousePos;
    float mouseDistance;
    public float maxPower;
    float power;
    float angle;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseDistance = Vector2.Distance(mousePos, transform.position);

        if (coffinTaken)
        {
            if (Input.GetKeyDown(throwCoffin))
            {
                ThrowCoffin();
            }
        }
        DrawPath();
    }

    void calculateAngle()
    {
        Vector2 v2= mousePos-(Vector2)transform.position;
        angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;

        if(angle >=0 &&angle < 45)
        {
            if (Mathf.Abs(0 - angle) < Mathf.Abs(45 - angle))
            {
                angle = 0;
                target.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y);
            }
            else
            {
                angle = 45;
                target.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y+ (Mathf.Tan(45)*maxDistance*power)/4);
               
            }
        }else if (angle >= 45 && angle < 90)
        {
            if (Mathf.Abs(45 - angle) < Mathf.Abs(90 - angle))
            {
                angle = 45;
                target.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y + (Mathf.Tan(45) * maxDistance * power) / 4);
            }
            else
            {
                angle = 90;
                target.position = new Vector2(transform.position.x, transform.position.y+ power * maxH);
            }
        }
        else if (angle >= 90 && angle < 135)
        {
            if (Mathf.Abs(90 - angle) < Mathf.Abs(135 - angle))
            {
                angle = 90;
                target.position = new Vector2(transform.position.x, transform.position.y + power * maxH);
            }
            else
            {
                angle = 135;
                target.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y + Mathf.Abs((Mathf.Tan(135) * maxDistance * power) / 4));
            }
        }
        else if (angle >= 135 && angle < 180)
        {
            if (Mathf.Abs(135 - angle) < Mathf.Abs(180 - angle))
            {
                angle = 135;
                target.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y + Mathf.Abs((Mathf.Tan(135) * maxDistance * power) / 4));
            }
            else
            {
                angle = 180;
                target.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y);
            }
            }
        else if (angle < 0 && angle > -45)
        {
            if (Mathf.Abs(0 - angle) < Mathf.Abs(-45 - angle))
            {
                angle = 0;
                target.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y);
            }
            else
            {
                angle = -45;
                target.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y + (Mathf.Tan(135) * maxDistance * power) / 4);
            }
        }
        else if (angle < -45 && angle > -135)
        {
            if (Mathf.Abs(-45 - angle) < Mathf.Abs(-135 - angle))
            {
                angle = -45;
                target.position = new Vector2(transform.position.x + power * maxDistance, transform.position.y +(Mathf.Tan(-45) * maxDistance * power) / 4);
            }
            else
            {
                angle = -135;
                target.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y - (Mathf.Tan(-135) * maxDistance * power) / 4);
            }
        }
        else if (angle < -135 && angle > -180)
        {
            if (Mathf.Abs(-135 - angle) < Mathf.Abs(-180 - angle))
            {
                angle = -135;
                target.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y - (Mathf.Tan(-135) * maxDistance * power) / 4);
            }
            else
            {
                angle = -180;
                target.position = new Vector2(transform.position.x - power * maxDistance, transform.position.y);
            }
        }
        Debug.Log("Angle: " + angle);
    }

    void calculatePower()
    {
        if(mouseDistance >= maxDistance)
        {
            power = 1;
        }
        else{
            power = (mouseDistance / maxDistance);
        }
        h = maxH * power;
        Debug.Log("Power: " + power+" X: "+power*maxDistance+" H: "+h);
    }

    public void ThrowCoffin()
    {
        calculatePower(); calculateAngle();
        Physics.gravity = Vector3.up * gravity;
        rb.velocity = CalculateLaunchData().initialVelocity;
    }


    LaunchData CalculateLaunchData()
    {
        float displacementY = target.position.y - rb.position.y;
        Vector3 displacementX = new Vector2(target.position.x - rb.position.x, 0);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2*(displacementY- h)/gravity);
        Vector2 velocityY = Vector2.up * Mathf.Sqrt(-2 * gravity * h);
        Vector2 velocityX = displacementX / time;

        return new LaunchData(velocityX + velocityY * -Mathf.Sign(gravity), time);
    }

    void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector2 previousDrawPoint = rb.position;
        int resolution = 30;
        for(int i = 1; i <= resolution; i++)
        {
            float simulationTime = i/(float)resolution * launchData.timeToTarget;
            Vector2 displacement = launchData.initialVelocity * simulationTime + Vector2.up * gravity * simulationTime * simulationTime / 2f;
            Vector2 drawPoint = rb.position + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;
        }

    }

    struct LaunchData
    {
        public readonly Vector2 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector2 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}
