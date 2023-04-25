using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.PXR;

public class ActiveController : MonoBehaviour
{
    [SerializeField]
    GameObject leftHand = null;
    [SerializeField]
    GameObject rightHand = null;

    [SerializeField]
    GameObject leftController = null;
    [SerializeField]
    GameObject rightController = null;

    void Start()
    {
        UpdateActiveInputDevice();
    }

    void Update(){
        UpdateActiveInputDevice();
    }

    void UpdateActiveInputDevice()
    {
        ActiveInputDevice activeInput = PXR_HandTracking.GetActiveInputDevice();

        if(leftHand)
        leftHand?.SetActive(activeInput == ActiveInputDevice.HandTrackingActive);

        if(rightHand)
        rightHand?.SetActive(activeInput == ActiveInputDevice.HandTrackingActive);

        if(leftController)
        leftController?.SetActive(activeInput == ActiveInputDevice.ControllerActive);

        if(rightController)
        rightController?.SetActive(activeInput == ActiveInputDevice.ControllerActive);
    }
}