using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NewScript

public class InputPlayer
{
    public static float GetAxisChangeWeapon(bool useKeyboard)
    {
        if (useKeyboard) { return Input.GetAxisRaw("Mouse Scrollwheel") + (Input.GetKeyDown(KeyCode.LeftControl)? 2f: 0f); }
        else { return Input.GetAxisRaw("Joystick Scrollwheel"); }
    }

    public static float GetAxisHorizontal(bool useKeyboard)
    {
        if (useKeyboard) { return Input.GetAxisRaw("Horizontal"); }
        else { return Input.GetAxisRaw("HorizontalJoystick"); }
    }

    public static float GetAxisVertical(bool useKeyboard)
    {
        if (useKeyboard) { return Input.GetAxisRaw("Vertical"); }
        else { return Input.GetAxisRaw("VerticalJoystick"); }
    }

    public static bool GetButtonConfirmDown(bool useKeyboard)
    {
        if (useKeyboard) { return Input.GetKeyDown(KeyCode.Return); }
        else { return Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Keypad0); }

    }

    public static bool GetButtonConfirmUp(bool useKeyboard)
    {
        if (useKeyboard) { return Input.GetKeyUp(KeyCode.Return); }
        else { return Input.GetKeyUp(KeyCode.JoystickButton0); }

    }

    public static bool GetButtonShoot(bool useKeyboard)
    {
        if (useKeyboard) { return Input.GetKey(KeyCode.Mouse0); }
        else { return Input.GetKey(KeyCode.JoystickButton5); }
        
    }

    public static bool GetButtonSpecial(bool useKeyboard)
    {
        if (useKeyboard) { return Input.GetKey(KeyCode.LeftShift); }
        else { return Input.GetKey(KeyCode.JoystickButton4); }

    }
    public static bool GetButtonSpecialDown(bool useKeyboard)
    {
        if (useKeyboard) { return Input.GetKeyDown(KeyCode.LeftShift); }
        else { return Input.GetKeyDown(KeyCode.JoystickButton4); }
    }

    public static bool GetButtonPauseDown()
    {
        return Input.GetKeyDown(KeyCode.Escape) 
            || Input.GetKeyDown(KeyCode.JoystickButton7); 
    }

}
