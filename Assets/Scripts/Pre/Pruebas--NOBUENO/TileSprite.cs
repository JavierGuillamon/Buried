using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSprite : MonoBehaviour {
    SpriteRenderer sprite;


    // Use this for initialization
    void Change() {
        
        int a = transform.childCount;
        for (int i = 0; i< transform.childCount; i++)
        {
            GameObject.Destroy(gameObject.transform.GetChild(i).gameObject);
        }
      // prueba1

        //coge el sprite actual con un tamaño sin escalar
        sprite = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);
        //Vector2 spriteSize = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
        //genera un hijo prefab del sprite renderer
        GameObject childPrefab = new GameObject();
        SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer>();
        childPrefab.transform.position = transform.position;
        childSprite.sprite = sprite.sprite;

        GameObject child;
      /*  for(int i =1, l=(int)Mathf.Round(sprite.bounds.size.y);i< l; i++)
        {
            child = Instantiate(childPrefab) as GameObject;
            child.transform.position = transform.position - (new Vector3(0, spriteSize.y, 0)*i);
            child.transform.parent = transform;
        }

        childPrefab.transform.parent = transform;
        */

        for(int i = 0, h=(int)Mathf.Round(sprite.bounds.size.y);i*spriteSize.y< h; i++)
        {
            for(int j =0, w = (int)Mathf.Round(sprite.bounds.size.x);j+spriteSize.x< w; j++)
            {
                child = Instantiate(childPrefab) as GameObject;
                child.transform.position = transform.position - (new Vector3(spriteSize.x * j, spriteSize.y * i, 0));
                child.transform.parent = transform;
            }
        }
        Destroy(childPrefab);
        sprite.enabled = false;

        //prueba 2


	}
	
	// Update is called once per frame
	void Update() {
      //  if (Input.GetKeyDown(KeyCode.T))
       // {
         //   Debug.Log("DNENTRO");
            Change();
       // }
	}
    
}
