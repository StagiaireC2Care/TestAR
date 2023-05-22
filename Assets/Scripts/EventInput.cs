using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ButtonAction
{
    public UnityEvent OnDown;
    public UnityEvent OnUp;
    public UnityEvent OnPress;
}

public class EventInput : MonoBehaviour
{

    [Header("Left controller")]
    public ButtonAction X;
    public ButtonAction Y;
    public ButtonAction LeftTrigger;
    public ButtonAction LeftJoystick;

    [Header("Right controller")]
    public ButtonAction A;
    public ButtonAction B;
    public ButtonAction RightTrigger;
    public ButtonAction RightJoystick;

    private InputMgr Input;
    void Awake()
    {
        Input = InputMgr.GetInstance();
    }

    void Update()
    {
        if(Input.LeftPrimaryButtonDown())
            X.OnDown.Invoke();
        else if(Input.LeftPrimaryButtonUp())
            X.OnUp.Invoke();
        else if(Input.LeftPrimaryButton())
            X.OnPress.Invoke();

        if(Input.LeftSecondaryButtonDown())
            Y.OnDown.Invoke();
        else if(Input.LeftSecondaryButtonUp())
            Y.OnUp.Invoke();
        else if(Input.LeftSecondaryButton())
            Y.OnPress.Invoke();

        if(Input.LeftTriggerButtonDown())
            LeftTrigger.OnDown.Invoke();
        else if(Input.LeftTriggerButtonUp())
            LeftTrigger.OnUp.Invoke();
        else if(Input.LeftTriggerButton())
            LeftTrigger.OnPress.Invoke();

        if(Input.RightPrimaryButtonDown())
            A.OnDown.Invoke();
        else if(Input.RightPrimaryButtonUp())
            A.OnUp.Invoke();
        else if(Input.RightPrimaryButton())
            A.OnPress.Invoke();

        if(Input.RightSecondaryButtonDown())
            B.OnDown.Invoke();
        else if(Input.RightSecondaryButtonUp())
            B.OnUp.Invoke();
        else if(Input.RightSecondaryButton())
            B.OnPress.Invoke();

        if(Input.RightTriggerButtonDown())
            RightTrigger.OnDown.Invoke();
        else if(Input.RightTriggerButtonUp())
            RightTrigger.OnUp.Invoke();
        else if(Input.RightTriggerButton())
            RightTrigger.OnPress.Invoke();

        if(Input.RightJoystickButtonDown())
            RightJoystick.OnDown.Invoke();
        else if(Input.RightJoystickButtonUp())
            RightJoystick.OnUp.Invoke();
        else if(Input.RightJoystickButton())
            RightJoystick.OnPress.Invoke();

        if(Input.LeftJoystickButtonDown())
            LeftJoystick.OnDown.Invoke();
        else if(Input.LeftJoystickButtonUp())
            LeftJoystick.OnUp.Invoke();
        else if(Input.LeftJoystickButton())
            LeftJoystick.OnPress.Invoke();
    }
}
