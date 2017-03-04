using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsController : MonoBehaviour {
    public GameObject Coffin;
    [SerializeField]
    private float mana;
    [SerializeField]
    private float manaRegenRate;
    [Header("Fuego")]
    public FuegoCoffin fuego;
    public KeyCode fuegoKeyCode;
    public float cooldownFuego;
    public float manaFuego;

    [Header("Hielo")]
    public KeyCode hieloKeyCode;
    public float cooldownHielo;
    public float manaHielo;
    [Header("Sombra")]
    public SombraCoffinDetect sombraDetect;
    public KeyCode sombraKeyCode;
    public float cooldownSombra;
    public float manaSombra;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(fuegoKeyCode))
        {
           fuego.doFuego(cooldownFuego);
            
        }
        if (Input.GetKeyDown(hieloKeyCode))
        {
            //HIELO
        }
        if (Input.GetKeyDown(sombraKeyCode))
        {
            //SOMBRA
            if (sombraDetect.canUseSombra())
            {
                Coffin.layer = LayerMask.NameToLayer("sombra");
                
            }
        }
	}
}
