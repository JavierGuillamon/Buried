using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Morir : MonoBehaviour {


    public Image fadeImg;
    public Text fadeText;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine( Fade());
        }
    }

    IEnumerator Fade()
    {
        while (fadeImg.color.a < 1)
        {
            Color auxI = fadeImg.color;
            Color auxT = fadeText.color;
            auxI.a = auxI.a + 0.01f;
            auxT.a = auxT.a + 0.01f;
            fadeImg.color = auxI;
            fadeText.color = auxT;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
