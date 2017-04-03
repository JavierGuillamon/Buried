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
    public KeyCode hieloActivateKeyCode;
    public Hielo hielo;
    public float timeOnAir;
    public float cooldownHielo;
    public float manaHielo;
    [Header("Sombra")]
    public SombraCoffinDetect sombraDetect;
    public KeyCode sombraKeyCode;
    public float cooldownSombra;
    public float manaSombra;

    private bool sombraActiva = false;
    private bool hieloActivo = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (InputManager.AButton())
        {
            hieloActivo = false;
            fuego.doFuego(cooldownFuego);
            
        }
        if (InputManager.BButton())
        {
            if (sombraActiva)
            {
                sombraActiva = !sombraActiva;
                Coffin.GetComponent<SombraMov>().Reject();
            }
            else
            {
                //HIELO
                hieloActivo = true;
                Coffin.layer = LayerMask.NameToLayer("hielo");
                hielo.activateHielo();
            }
        }
        if (InputManager.XButton())
        {
            //SOMBRA
            if (!sombraActiva) {
                sombraActiva = true;
                hieloActivo = false;
                if (sombraDetect.canUseSombra()) Coffin.layer = LayerMask.NameToLayer("sombra");
            }
            else
            {
                sombraActiva = false;
                Coffin.GetComponent<SombraMov>().Reject();
            }
        }

        if (InputManager.BButton())
        {
            if (hieloActivo)
            {
                hielo.HieloExplode(timeOnAir);
            }
        }
	}
}
