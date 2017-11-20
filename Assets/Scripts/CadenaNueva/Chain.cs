using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour {

    /*[SerializeField]
    List<Corner> corners;*/
    [SerializeField]
    float offSet = 0.1f;
    [SerializeField]
    float offSetHook = 0.1f;
    [SerializeField]
    public float maxDistance = 5f;
    [SerializeField]
    public float currDistance = 5f;
    [SerializeField]
    float distanceToStartPull = 5f;
    [SerializeField]
    DistanceJoint2D coffinJoint;
    [SerializeField]
    DistanceJoint2D playerJoint;

    public Vector3 chainNormal { get { return (currenCornerPoints[1].GetPosition() - currenCornerPoints[0].GetPosition()).normalized; } }

    [SerializeField]
    List<GroundDetector> coffinDetectors;
    [SerializeField]
    List<GroundDetector> playerDetectors;

    bool playerInGround { get
        {
            foreach (GroundDetector gd in playerDetectors)
            {
                if (gd.DetectGround())
                    return true;
            }
            return false;
        }
    }
    bool coffinInGround
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

    public bool pullFromCoffin = true;
    public bool pullFromPlayer = true;
    
    public float ChainLength { get
        {
            float l = 0;
            for (int i = 1; i < currenCornerPoints.Count; i++)
                l += Vector2.Distance(currenCornerPoints[i-1].GetPosition(), currenCornerPoints[i ].GetPosition());
            return l;
        }
    }

    [SerializeField]
    Transform playerHook;
    [SerializeField]
    Transform coffinHook;

    // Use this for initialization
    bool Initialized = false;
    void Initialize () {
        currenCornerPoints = new List<CornerPoint>();
        currenCornerPoints.Add(new CornerPoint(playerHook.transform.position, playerHook.transform.parent.gameObject, playerHook.transform.position - Vector3.right, playerHook.transform.position + Vector3.right));
        currenCornerPoints.Add(new CornerPoint(coffinHook.transform.position, coffinHook.transform.parent.gameObject, coffinHook.transform.position - Vector3.right, coffinHook.transform.position + Vector3.right));
        Initialized = true;
    }

    [SerializeField]
    LayerMask terrainLayers;
    [SerializeField]
    LineRenderer lineRenderer;

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (!Initialized)
            Initialize();
        UpdateCornerPositions();
        CleanCorners();
        /* if (playerInGround && (ChainLength < maxDistance - distanceToStartPull) || (!coffinInGround) )
         {
             pullFromPlayer = false;
             pullFromCoffin = true;
         }else
         {
             pullFromPlayer = true;
             pullFromCoffin = false;
         }*/

        UpdateChainCorners();
        CleanCorners();
        CoffinPull();
        PlayerPull();
        
    }

    void Update()
    {
    }

    void CoffinPull()
    {
        if (pullFromCoffin)
        {
            Vector3 coffinJointPos = currenCornerPoints[currenCornerPoints.Count - 2].GetPosition();
            Vector3 off = Vector3.up * 2;//currenCornerPoints[currenCornerPoints.Count - 2].GetTangent()*offSetHook;
            if (currenCornerPoints[currenCornerPoints.Count - 2].GetTangent().x > 0)
                off += Vector3.right * 0.2f;
            else
                off -= Vector3.right * 0.2f;

            coffinJointPos += off;
            coffinJoint.connectedAnchor = coffinJointPos;
            coffinJoint.distance = currDistance - ChainLength + Vector2.Distance(currenCornerPoints[currenCornerPoints.Count - 1].GetPosition(), currenCornerPoints[currenCornerPoints.Count - 2].GetPosition());
        }
    }

    void PlayerPull()
    {
        if (pullFromPlayer)
        {
            Vector3 jointPos = currenCornerPoints[1].GetPosition();
            Vector3 off = Vector3.up * 2;// currenCornerPoints[1].GetTangent() * offSetHook;
            if (currenCornerPoints[currenCornerPoints.Count - 2].GetTangent().x > 0)
                off += Vector3.right * 0.5f;
            else
                off -= Vector3.right * 0.5f;
            jointPos += off;
            playerJoint.connectedAnchor = jointPos;
            playerJoint.distance = currDistance - ChainLength + Vector2.Distance(currenCornerPoints[0].GetPosition(), currenCornerPoints[1].GetPosition());
        }
        else
        {
            Vector3 jointPos = currenCornerPoints[1].GetPosition();
            Vector3 off = Vector3.up;//currenCornerPoints[1].GetTangent() * offSetHook;
            jointPos += off;
            playerJoint.connectedAnchor = jointPos;
            playerJoint.distance = maxDistance*2;
        }
    }
   
   [System.Serializable]
    struct CornerPoint
    {
        Vector3 previousPosition;
        Vector3 currentPosition;
        GameObject tiedGameObject;
        Vector3 relativePosition;
        Vector3 relativetangentDirection;
        
        public CornerPoint(Vector3 pos, GameObject ob, Vector3 prev, Vector3 next)
        {
            currentPosition = pos;
            relativetangentDirection = ob.transform.InverseTransformDirection(((pos - prev).normalized + (pos - next).normalized).normalized);
            relativePosition = ob.transform.InverseTransformPoint(pos);
            previousPosition = pos;
            tiedGameObject = ob;
        }

        public Vector3 GetPreviousPosition()
        {
            return previousPosition;
        }

        public Vector3 GetPosition()
        {
            return tiedGameObject.transform.TransformPoint(relativePosition);
        }

        public Vector3 GetTangent()
        {
            return tiedGameObject.transform.TransformDirection(relativetangentDirection).normalized;
        }

        public void SetNewPosition()
        {
            previousPosition = currentPosition;
            currentPosition = GetPosition();
        } 
    }

    //Missing circleColliders
    List<CornerPoint> GetCornersFrom(Collider2D coll)
    {
        List<Vector3> points = new List<Vector3>();
        //First we get the points
        if((coll as BoxCollider2D) != null)
        {
            BoxCollider2D c = coll as BoxCollider2D;
            points.Add(c.transform.TransformPoint(Vector2.right*(c.offset.x + c.size.x/2) + Vector2.up*(c.offset.y + c.size.y / 2)));
            points.Add(c.transform.TransformPoint(Vector2.right * (c.offset.x + c.size.x / 2) + Vector2.up * (c.offset.y - c.size.y / 2)));
            points.Add(c.transform.TransformPoint(Vector2.right * (c.offset.x - c.size.x / 2) + Vector2.up * (c.offset.y - c.size.y / 2)));
            points.Add(c.transform.TransformPoint(Vector2.right * (c.offset.x - c.size.x / 2) + Vector2.up * (c.offset.y + c.size.y / 2)));
        }
        if ((coll as PolygonCollider2D) != null)
        {
            PolygonCollider2D p = coll as PolygonCollider2D;
            for(int i = 0; i < p.points.Length; i++)
            {
                points.Add(p.transform.TransformPoint(p.points[i]));
            }
        }
        if ((coll as EdgeCollider2D) != null)
        {
            EdgeCollider2D p = coll as EdgeCollider2D;
            for (int i = 0; i < p.points.Length; i++)
            {
                points.Add(p.transform.TransformPoint(p.points[i]));
            }
        }
        if ((coll as CircleCollider2D) != null)
        {

        }
        //Debug Draw
        for (int i = 0; i < points.Count; i++)
        {
            if (i < points.Count - 1)
                Debug.DrawLine(points[i], points[i + 1], Color.black, 0.1f);
            else
                Debug.DrawLine(points[i], points[0], Color.black, 0.1f);
            
        }
        //GetCorners
        List<CornerPoint> corners = new List<CornerPoint>();
        for (int i = 0; i < points.Count; i++)
        {
            if (i ==0)
                corners.Add(new CornerPoint( points[i],coll.gameObject, points[points.Count -1], points[i+1]));
            else if (i == points.Count - 1)
                corners.Add(new CornerPoint(points[i], coll.gameObject, points[i - 1], points[0]));
            else
                corners.Add(new CornerPoint(points[i], coll.gameObject, points[i - 1], points[i+1]));
        }
        return corners;
    }
    List<CornerPoint> currenCornerPoints;


    void CleanCorners()
    {
        List<CornerPoint> CleanedCorners = new List<CornerPoint>();
        //Clean cleared corners
        CleanedCorners.Add(currenCornerPoints[0]);
        for (int z = 0; z < currenCornerPoints.Count - 2; z++)
        {
            Vector3 origin = currenCornerPoints[z].GetPosition();
            Vector3 middle = currenCornerPoints[z + 1].GetPosition();
            Vector3 end = currenCornerPoints[z + 2].GetPosition();
            Vector3 offTangent = middle + currenCornerPoints[z + 1].GetTangent();

            Vector3 crossTangent = Vector3.Cross(offTangent - origin, middle - origin);
            Vector3 crossend = Vector3.Cross(end - origin, middle - origin);
            Vector3 i_crossTangent = Vector3.Cross(offTangent - end, middle - end);
            Vector3 i_crossend = Vector3.Cross(origin - end, middle - end);
            if ((crossTangent.z < 0 && crossend.z < 0 || crossTangent.z > 0 && crossend.z > 0) || (i_crossTangent.z < 0 && i_crossend.z < 0 || i_crossTangent.z > 0 && i_crossend.z > 0))
            {
                if(z+2 < currenCornerPoints.Count - 1)
                    CleanedCorners.Add(currenCornerPoints[z + 2]);
                z++;
            }else
            {
                CleanedCorners.Add(currenCornerPoints[ z + 1]);
            }
        }
        CleanedCorners.Add(currenCornerPoints[currenCornerPoints.Count - 1]);
        currenCornerPoints = CleanedCorners;
    }

    void UpdateCornerPositions()
    {
        foreach(CornerPoint cp in currenCornerPoints)
        {
            cp.SetNewPosition();
        }
    }

    void UpdateChainCorners()
    {
        List<CornerPoint> newCorners = new List<CornerPoint>();
        newCorners.Add(currenCornerPoints[0]);
        int i = 1;
        bool addedForThis = false;
        lineRenderer.positionCount = currenCornerPoints.Count;
        while (i < currenCornerPoints.Count)
        {
            Vector3 origin = newCorners[newCorners.Count - 1].GetPosition() + newCorners[newCorners.Count - 1].GetTangent()*offSet;
            Vector3 end = currenCornerPoints[i].GetPosition() + currenCornerPoints[i].GetTangent() * offSet;
            Vector3 p_origin = newCorners[newCorners.Count - 1].GetPreviousPosition() + newCorners[newCorners.Count - 1].GetTangent() * offSet;
            Vector3 p_end = currenCornerPoints[i].GetPreviousPosition() + currenCornerPoints[i].GetTangent() * offSet;
            //Debug.DrawLine(origin, end, Color.green, 0.2f);
            lineRenderer.SetPosition(i-1, origin);
            lineRenderer.SetPosition(i, end);
            RaycastHit2D result = Physics2D.Raycast(origin + (end - origin).normalized * offSet, end - origin, (end - origin).magnitude - offSet, terrainLayers);
            //If there's something in between, add a new corner to the list.
            if (result.collider != null)
            {
                //We get all the corners from the hit collider
                List<CornerPoint> points = GetCornersFrom(result.collider);
                float angle = 0;
                CornerPoint newCorner = points[0];
                bool found = false;
                foreach (CornerPoint c in points)
                {
                    //We see if the corner has crossed the line
                    Vector3 crossCenter = Vector3.Cross(end - origin, (c.GetPosition() - c.GetTangent()) - origin);
                    Vector3 crossCurrent = Vector3.Cross(end - origin, c.GetPosition() - origin);
                    if (crossCenter.z < 0 && crossCurrent.z >= 0 || crossCenter.z > 0 && crossCurrent.z <= 0)
                    {
                        float newAngle = Vector3.Angle(c.GetPosition() - origin, end - origin);
                        if (!found || Mathf.Abs(newAngle) > Mathf.Abs(angle))
                        {
                            angle = newAngle;
                            newCorner = c;
                            found = true;
                        }
                    }
                }
                if (found && newCorners [newCorners.Count-1].GetPosition() != newCorner.GetPosition() && ! addedForThis)
                {
                    newCorners.Add(newCorner);
                    addedForThis = true;
                }
                else
                {
                    newCorners.Add(currenCornerPoints[i]);
                    i++;
                    addedForThis = false;
                }
            }
            else
            {
                newCorners.Add(currenCornerPoints[i]);
                i++;
                addedForThis = false;
            }
        }
        currenCornerPoints = newCorners;

        for (int ii = 0; ii < currenCornerPoints.Count - 1; ii++)
        {
            Debug.DrawLine(currenCornerPoints[ii].GetPosition(), currenCornerPoints[ii + 1].GetPosition(), Color.red);
        }
    }

    public void ResetCorners()
    {
        currenCornerPoints.Clear();
        Initialize();
    }
}
