using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Provide the left controller teleport function
/// </summary>
public class TeleportController : MonoBehaviour
{
    private TeleportationProvider provider;
    [FormerlySerializedAs("teleportRay")] public XRRayInteractor teleportRayInteractor;
    private InteractionSampleInputActions inputActions;

    private void Awake()
    {
        provider = GetComponent<TeleportationProvider>();
        teleportRayInteractor.enabled = false;
        inputActions = new InteractionSampleInputActions();
        //The left controller is responsible for controlling the teleport.
        inputActions.Teleport.Trigger.Enable();
        inputActions.Teleport.Trigger.performed += LeftTriggerDownHandler;
        inputActions.Teleport.Trigger.canceled += LeftTriggerUpHandler;
    }

    private void LeftTriggerUpHandler(InputAction.CallbackContext callbackContext)
    {
        /*
         * teleport to pointer when lift  (need to make sure goals are valid) 
         * 1. pointer position must exists and have teleportation scripts
         * 2. if pointer is valid, we need to create teleport request and submit it
         */
        RaycastHit rayInfo;
        //Judge target position's validity
        bool isValid = teleportRayInteractor.TryGetCurrent3DRaycastHit(out rayInfo);
        isValid = isValid && rayInfo.collider != null &&
                  (rayInfo.collider.GetComponent<TeleportationArea>() != null ||
                   rayInfo.collider.GetComponent<TeleportationAnchor>() != null);
        if (isValid)
        {
            //Create teleport request and submit it to the queue
            TeleportRequest request = new TeleportRequest();
            request.destinationPosition = rayInfo.point;
            provider.QueueTeleportRequest(request);
        }
        
        //Disable ray and indicator after teleport
        teleportRayInteractor.enabled = false;
        teleportRayInteractor.GetComponent<XRInteractorLineVisual>().reticle?.SetActive(false);
    }

    private void LeftTriggerDownHandler(InputAction.CallbackContext callbackContext)
    {
        //enable the script to select position to teleport
        teleportRayInteractor.enabled = true;
    }
    
    
}

