using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {

    public static float MainHorizontal()
    {
        float r = 0.0f;
        r += Input.GetAxis("J_MainHorizontal");
        r += Input.GetAxis("K_MainHorizontal");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }
    public static float MainVertical()
    {
        float r = 0.0f;
        r += Input.GetAxis("J_MainVertical");
        r += Input.GetAxis("K_MainVertical");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }
    public static float AuxHorizontal()
    {
        float r = 0.0f;
        r += Input.GetAxis("J_AuxHorizontal");
        r += Input.GetAxis("Mouse X");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }
    public static float AuxVertical()
    {
        float r = 0.0f;
        r += Input.GetAxis("J_AuxVertical");
        r += Input.GetAxis("Mouse Y");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }
    
    public static Vector2 MainJoystic()
    {
        return new Vector2(MainHorizontal(), MainVertical());
    }
    public static Vector2 AuxJoystic()
    {
        return new Vector2(AuxHorizontal(), AuxVertical());
    }

    public static bool LeftTrigger()
    {
        int r = 0;
        r += (int)Input.GetAxis("J_lftTrigger");
        r += System.Convert.ToInt32(Input.GetMouseButton(0));
        if(r>0)
            return true;
        return false;
    }
    public static bool LeftTriggerDown()
    {
        int r = 0;
        r += (int)Input.GetAxis("J_lftTrigger");
        r += System.Convert.ToInt32(Input.GetMouseButtonDown(0));
        if (r > 0)
            return true;
        return false;
    }
    public static bool LeftTriggerUp()
    {
        int r = 0;
        r += (int) Input.GetAxis("J_lftTrigger");
        r += System.Convert.ToInt32(!Input.GetMouseButtonUp(0));
        if (r == 0) return true;
        return false;
    }
    public static bool RightTrigger()
    {
        int r = 0;
        r += (int)Input.GetAxis("J_rgtTrigger");
        r += System.Convert.ToInt32(Input.GetMouseButton(1));
        if (r > 0)
            return true;
        return false;
    }
    public static bool RightTriggerDown()
    {
        int r = 0;
        r += (int)Input.GetAxis("J_rgtTrigger");
        r += System.Convert.ToInt32(Input.GetMouseButtonDown(1));
        if (r > 0)
            return true;
        return false;
    }
    public static bool RightTriggerUp()
    {
        int r = 0;
        r += (int)Input.GetAxis("J_rgtTrigger");
        r += System.Convert.ToInt32(!Input.GetMouseButtonUp(1));
        if (r == 0)
            return true;
        return false;
    }
    public static bool AButton()
    {
        return Input.GetButton("A_Button");
    }
    public static bool BButton()
    {
        return Input.GetButtonDown("B_Button");
    }
    public static bool XButton()
    {
        return Input.GetButtonDown("X_Button");
    }
    public static bool YButton()
    {
        return Input.GetButtonDown("Y_Button");
    }

}
