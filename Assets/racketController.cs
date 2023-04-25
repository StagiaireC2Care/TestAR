using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class racketController : MonoBehaviour
{
    [SerializeField]
    Vector3 rot1 = new Vector3(57.37f, 0, -3.2f);

    [SerializeField]
    Vector3 rot2;

    private ControllerSampleInputActions inputActions;
    private ParentConstraint parentConstraint;
    bool currentRot = false;
    private void Awake()
    {
        inputActions = new ControllerSampleInputActions();
        parentConstraint = GetComponent<ParentConstraint>();
    }

    // private void OnEnable()
    // {
    //     inputActions.RightController.AX.Enable();
    //     inputActions.RightController.AX.performed += RightAXButtonClickHandler;
    // }

    // private void OnDisable()
    // {
    //     inputActions.RightController.AX.Disable();
    //     inputActions.RightController.AX.performed -= RightAXButtonClickHandler;
    // }

    private void RightAXButtonClickHandler(InputAction.CallbackContext callbackContext)
    {
        parentConstraint.SetRotationOffset(0, currentRot ? rot2 : rot1);
        currentRot = !currentRot;
    }
}
