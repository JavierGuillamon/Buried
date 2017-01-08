using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class PrintChain : MonoBehaviour {

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform source;
    //[SerializeField]
    // private Transform source;

    //private Transform transform;
    [SerializeField]
    private GameObject chainLink;
    [SerializeField]
    private GameObject chain;

    private RectTransform rctTrans;

    private Vector3 fromSourceToTarget;
    // Use this for initialization
    void Start () {
        // transform = GetComponent<Transform>();
        rctTrans = chainLink.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
       
        float rot_z = Mathf.Atan2(target.position.y, target.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot_z );

        float dist = Vector3.Distance(transform.position, target.position);
        print("Distance to other: " + dist);
       
         /***
        Basicamente lo que queria hacer es coger la distancia entre traget y source, la dividimos por el tamaño de cada link
        desde ahi imprimimos esa cantidad de links, facil no? pues no me esta saliendo xD tengo que coger bien el tamaño del link
        la distancia esta bien pero esta en unidades de unity, el link esta en pixeles.
        100 pixeles son 1 unidad de unity, pero el rectransform al cambiar la escala no modifica los valores de los pixeles, puta mierda

 height = rctTrans.rect.height;
       
    */
        //float size = mesh.bounds.size.x * transform.localScale.x;
        fromSourceToTarget = target.position - source.position;
        transform.localScale = fromSourceToTarget;
       // float a = NumberOfLinks(dist, height);

        //Debug.Log("HH: " + height);
        //GameObject newLink = Instantiate(chainLink) as GameObject;
        //newLink.transform.parent = chain.transform;


    }

    private float NumberOfLinks(float distance, float sizeLink)
    {


        float nmbr= distance/ sizeLink;
        Debug.Log("chain: " + distance + " Link: " + sizeLink + " result: " + nmbr);
        return nmbr;
    }

    private void ChangeParentScale(Transform parent, Vector3 scale)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            child.parent = null;
            children.Add(child);
        }
        parent.localScale = scale;
        foreach (Transform child in children) child.parent = parent;
    }
}
