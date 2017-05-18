using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour {
    [SerializeField]
    private GameObject destiny, origin;
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float distance;

    [SerializeField]
    private GameObject linkPrefab, firstLink;
    private GameObject lastLink;
    [SerializeField]
    private List<GameObject> Links = new List<GameObject>();
    [SerializeField]
    private LineRenderer lr;
    [SerializeField]
    private ControllCoffin controlCoffin;

    public bool resize=false;

    private float maxDistance;
    private int numPositions=2;
    private bool done = false;
    private float maxLinksNumber;
     // Use this for initialization
    void Start () {
        lr = GetComponent<LineRenderer>();
        lastLink = transform.gameObject;
        Links.Add(transform.gameObject);
        //maxDistance = controlCoffin.MaxDistance;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = Vector2.MoveTowards(transform.position, destiny.transform.position,speed);

        //if (transform.position != destiny.transform.position)
        float dist = Vector2.Distance(origin.transform.position, destiny.transform.position);
        maxLinksNumber = dist / distance;

        //  Debug.Log("Dist: " + dist + " distance: " + distance + " MaxLinkNumbers: " + maxLinksNumber + " numPositios: " + numPositions);
         if ((Vector2)transform.position!= (Vector2) destiny.transform.position)
         {
            //Debug.Log("TR pos:: " + (Vector2)transform.position + " destiny:: " + (Vector2)destiny.transform.position);
             //done = false;
             if (Vector2.Distance(origin.transform.position, lastLink.transform.position) > distance)
             {
                 createLink();     
             }
         }
         else if(done == false)
         {
             done = true;
             createLink();

            //Debug.Log("DISTANCE:: " + Vector2.Distance(origin.transform.position, lastLink.transform.position) + " distance:: " + distance);
             while (Vector2.Distance(origin.transform.position, lastLink.transform.position) > distance)
             {
                 createLink();
             }
             lastLink.GetComponent<DistanceJoint2D>().connectedBody = origin.GetComponent<Rigidbody2D>();

         }
        //instantiateLink();
        if (resize)
            destroyLinks();
       // RenderLine();
    }

    void destroyLinks()
    {
        //Debug.Log("ROMPEMOS");
        Links.Clear();
        numPositions=2;
        //lastLink = Links[Links.Count];

        foreach(Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        lastLink = transform.gameObject;
        instantiateLink();
        //RenderLine();
        // Destroy(this.gameObject);
    }

    void RenderLine()
    {
        lr.positionCount= numPositions;
        int i;
        for (i =0; i < Links.Count; i++)
        {
            lr.SetPosition(i, Links[i].transform.position);
        }
        lr.SetPosition(i, origin.transform.position);
    }

    void instantiateLink()
    {
        while (Vector2.Distance(origin.transform.position, lastLink.transform.position) > distance)
        {
            createLink();
        }
        lastLink.GetComponent<DistanceJoint2D>().connectedBody = origin.GetComponent<Rigidbody2D>();
    }
    void createLink()
    {
        Vector2 positionLink = origin.transform.position - lastLink.transform.position;
        positionLink.Normalize();
        positionLink *= distance;
        positionLink += (Vector2)lastLink.transform.position;
        GameObject go = Instantiate(linkPrefab, positionLink, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        lastLink.GetComponent<DistanceJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();
        lastLink = go;
        Links.Add(lastLink);

        numPositions++;
    }


    public void setResize(bool resize)
    {
        this.resize = resize;
    }
}