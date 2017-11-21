using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
