using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlacaPresion : MonoBehaviour {

    [Tooltip("Introducir en minúsculas el eje debe moverse el objeto")]
    public string ejeMovimiento;
    public Transform movingObject;
    public AnimationCurve idaCurve;
    public AnimationCurve vueltaCurve;
    public float speedForward;
    public float speedBackwards;
    public float distance;
    public bool showGizmo;
    [Range(0f, 1f)]
    public float gizmoScale;

    public int objectsInTrigger;
    private bool open;
    private Vector3 objectOriginPos;
    private Vector3 objectFinalPos;
    private bool opening;
    private bool closing;
    private bool closeMotion;
    private float lastPercent;

    private void Start()
    {
        objectOriginPos = movingObject.position;
        objectsInTrigger = 0;
        open = false;
        opening = false;
        closeMotion = false;
        switch (ejeMovimiento)
        {
            case "x":
                objectFinalPos = new Vector3(movingObject.position.x + distance, movingObject.position.y, movingObject.position.z);
                break;
            case "y":
                objectFinalPos = new Vector3(movingObject.position.x, movingObject.position.y + distance, movingObject.position.z);
                break;
            case "z":
                objectFinalPos = new Vector3(movingObject.position.x, movingObject.position.y, movingObject.position.z + distance);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TriggerPlacaPresion")
        {
            objectsInTrigger += 1;
            if (objectsInTrigger == 2)
            {
                open = true;
                opening = true;
                StartCoroutine(_MoveForward());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TriggerPlacaPresion")
        {
            objectsInTrigger -= 1;
            if (open && !closeMotion)
            {
                closing = true;
                StartCoroutine(_MoveBackwards());
            }
        }
    }

    IEnumerator _MoveForward()
    {
        float newSpeed = (speedForward * lastPercent) + speedForward;
        closing = false;
        Vector3 oldPosition = movingObject.position;
        float curveTime = 0;
        float curveAmount = idaCurve.Evaluate(curveTime);
        while (curveAmount < 1.0f && opening)
        {
            curveTime += Time.deltaTime * speedForward;
            curveAmount = idaCurve.Evaluate(curveTime);
            movingObject.position = Vector3.Lerp(oldPosition, objectFinalPos, curveAmount);
            lastPercent = curveAmount;
            yield return null;
        }
    }

    IEnumerator _MoveBackwards()
    {
        float newSpeed = (speedBackwards * lastPercent) + speedBackwards;
        closeMotion = true;
        opening = false;
        Vector3 oldPosition = movingObject.position;
        float curveTime = 0;
        float curveAmount = vueltaCurve.Evaluate(curveTime);
        while (curveAmount < 1.0f && closing)
        {
            curveTime += Time.deltaTime * newSpeed;
            curveAmount = vueltaCurve.Evaluate(curveTime);
            movingObject.position = Vector3.Lerp(oldPosition, objectOriginPos, curveAmount);
            lastPercent = curveAmount;
            yield return null;
        }
        closeMotion = false;
    }

    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            switch (ejeMovimiento)
            {
                case "x":
                    objectFinalPos = new Vector3(movingObject.position.x + distance, movingObject.position.y, movingObject.position.z);
                    break;
                case "y":
                    objectFinalPos = new Vector3(movingObject.position.x, movingObject.position.y + distance, movingObject.position.z);
                    break;
                case "z":
                    objectFinalPos = new Vector3(movingObject.position.x, movingObject.position.y, movingObject.position.z + distance);
                    break;
            }
            Gizmos.color = new Color(0, 1, 0, .5f);
            Gizmos.DrawCube(objectFinalPos, new Vector3((movingObject.localScale.x + .1f) * gizmoScale, (movingObject.localScale.y + .1f) * gizmoScale, (movingObject.localScale.z + .1f) * gizmoScale));
        }
    }
}
