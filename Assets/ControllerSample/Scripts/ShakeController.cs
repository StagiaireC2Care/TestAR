using System.Collections;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Responsible for vibration control of controller
/// 1. Click the AX and BY button to produce light and heavy shocks
/// 2. Press the trigger button to produce continuous heavy shocks
/// 3. Press the grip button to produce continuous light shocks
/// </summary>
public class ShakeController : MonoBehaviour
{
    //Low vibration strength value (0-1)
    public float lowStrength = 0.3f;
    //High vibration strength value
    public float highStrength = 1f;
    //Time of single vibration , unit of milliseconds
    public int shakeTime = 1000;

    //Unity New Input System input actions
    private ControllerSampleInputActions inputActions;
    
    private ControllerUITest uiTest;
    private void Awake()
    {
        uiTest = FindObjectOfType<ControllerUITest>();
        inputActions = new ControllerSampleInputActions();
    }

    //Register controller input actions to produce vibration and display log message
    private void OnEnable()
    {
        inputActions.RightController.AX.Enable();
        inputActions.RightController.AX.performed += RightAXButtonClickHandler;
        
        inputActions.RightController.BY.Enable();
        inputActions.RightController.BY.performed += RightBYButtonClickHandler;
        
        inputActions.RightController.Trigger.Enable();
        inputActions.RightController.Trigger.performed += RightTriggerDownHandler;
        inputActions.RightController.Trigger.canceled += RightTriggerUpHandler;
        
        inputActions.RightController.Grip.Enable();
        inputActions.RightController.Grip.performed += RightGripDownHandler;
        inputActions.RightController.Grip.canceled += RightGripUpHandler;
        
        inputActions.LeftController.AX.Enable();
        inputActions.LeftController.AX.performed += LeftAXButtonClickHandler;
        
        inputActions.LeftController.BY.Enable();
        inputActions.LeftController.BY.performed += LeftBYButtonClickHandler;
        
        inputActions.LeftController.Grip.Enable();
        inputActions.LeftController.Grip.performed += LeftGripDownHandler;
        inputActions.LeftController.Grip.canceled += LeftGripUpHandler;
        
        inputActions.LeftController.Trigger.Enable();
        inputActions.LeftController.Trigger.performed += LeftTriggerDownHandler;
        inputActions.LeftController.Trigger.canceled += LeftTriggerUpHandler;
    }
    

    //Deactivates all controller events
    private void OnDisable()
    {
        inputActions.RightController.AX.Disable();
        inputActions.RightController.AX.performed -= RightAXButtonClickHandler;
        
        inputActions.RightController.BY.Disable();
        inputActions.RightController.BY.performed -= RightBYButtonClickHandler;
        
        inputActions.RightController.Trigger.Disable();
        inputActions.RightController.Trigger.performed -= RightTriggerDownHandler;
        inputActions.RightController.Trigger.canceled -= RightTriggerUpHandler;
        
        inputActions.RightController.Grip.Disable();
        inputActions.RightController.Grip.performed -= RightGripDownHandler;
        inputActions.RightController.Grip.canceled -= RightGripUpHandler;
        
        inputActions.LeftController.AX.Disable();
        inputActions.LeftController.AX.performed -= LeftAXButtonClickHandler;
        
        inputActions.LeftController.BY.Disable();
        inputActions.LeftController.BY.performed -= LeftBYButtonClickHandler;
        
        inputActions.LeftController.Grip.Disable();
        inputActions.LeftController.Grip.performed -= LeftGripDownHandler;
        inputActions.LeftController.Grip.canceled -= LeftGripUpHandler;
        
        inputActions.LeftController.Trigger.Disable();
        inputActions.LeftController.Trigger.performed -= LeftTriggerDownHandler;
        inputActions.LeftController.Trigger.canceled -= LeftTriggerUpHandler;
    }

    /// <summary>
    /// Left controller button down event callback function
    /// produce vibration and append log message
    /// </summary>
    private void LeftTriggerDownHandler(InputAction.CallbackContext callbackContext)
    {
        StartCoroutine("LeftControllerShake", "Trigger");
    }
    /// <summary>
    /// Left controller button up event callback function
    /// stop vibration
    /// </summary>
    private void LeftTriggerUpHandler(InputAction.CallbackContext callbackContext)
    {
        uiTest.AppendLogInfo("Left Trigger has been Up", true);
        StopCoroutine("LeftControllerShake");
        PXR_Input.SetControllerVibration(0f, 0, PXR_Input.Controller.LeftController);
        
    }

    private void LeftGripUpHandler(InputAction.CallbackContext callbackContext)
    {
        uiTest.AppendLogInfo("Left Grip has been Up", true);
        PXR_Input.SetControllerVibration(0f, 0, PXR_Input.Controller.LeftController);
        StopCoroutine("LeftControllerShake");
    }
    
    private void LeftGripDownHandler(InputAction.CallbackContext callbackContext)
    {
        StartCoroutine("LeftControllerShake", "Grip");
    }

    private void LeftBYButtonClickHandler(InputAction.CallbackContext callbackContext)
    {
        PXR_Input.SetControllerVibration(highStrength, shakeTime, PXR_Input.Controller.LeftController);
    }

    private void LeftAXButtonClickHandler(InputAction.CallbackContext callbackContext)
    {
        PXR_Input.SetControllerVibration(lowStrength, shakeTime, PXR_Input.Controller.LeftController);
    }

    private void RightTriggerUpHandler(InputAction.CallbackContext callbackContext)
    {
        uiTest.AppendLogInfo("Right Trigger has been Up", false);
        StopCoroutine("RightControllerShake");
        PXR_Input.SetControllerVibration(0f, 0, PXR_Input.Controller.RightController);
    }

    private void RightTriggerDownHandler(InputAction.CallbackContext callbackContext)
    {
        StartCoroutine("RightControllerShake","Trigger");
    }

    private IEnumerator RightControllerShake(string str)
    {
        while (true)
        {
            float strength = str == "Trigger" ? highStrength : lowStrength;
            uiTest.AppendLogInfo($"Right {str} has been Down", false);
            PXR_Input.SetControllerVibration(strength, shakeTime, PXR_Input.Controller.RightController);
            yield return new WaitForSeconds(shakeTime * 0.001f);
        }
    }

    private IEnumerator LeftControllerShake(string str)
    {
        while (true)
        {
            float strength = str == "Trigger" ? highStrength : lowStrength;
            uiTest.AppendLogInfo($"Left {str} has been Down", true);
            PXR_Input.SetControllerVibration(strength, shakeTime, PXR_Input.Controller.LeftController);
            yield return new WaitForSeconds(shakeTime * 0.001f);
        }
    }

    private void RightGripUpHandler(InputAction.CallbackContext callbackContext)
    {
        uiTest.AppendLogInfo("Right Grip has been Up", false);
        StopCoroutine("RightControllerShake");
        PXR_Input.SetControllerVibration(0f, 0, PXR_Input.Controller.RightController);
    }
    
    private void RightGripDownHandler(InputAction.CallbackContext callbackContext)
    {
        StartCoroutine("RightControllerShake","Grip");
    }

    private void RightBYButtonClickHandler(InputAction.CallbackContext callbackContext)
    {
        PXR_Input.SetControllerVibration(highStrength, shakeTime, PXR_Input.Controller.RightController);
    }

    private void RightAXButtonClickHandler(InputAction.CallbackContext callbackContext)
    {
        PXR_Input.SetControllerVibration(lowStrength, shakeTime, PXR_Input.Controller.RightController);
    }
}

