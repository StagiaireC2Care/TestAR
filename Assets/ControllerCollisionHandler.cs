using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.PXR;

public class ControllerCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        string colTag = collision.gameObject.tag;
        Debug.Log("[_DEBUG_] Collision with " + colTag);
        if (colTag == "Ball")
        {
            PXR_Input.SetControllerVibration(1, 150, PXR_Input.Controller.RightController);
        }
    }
}
