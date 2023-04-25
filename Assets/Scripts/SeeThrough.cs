using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.PXR;

public class SeeThrough : MonoBehaviour
{
    void Start()
    {
        PXR_Boundary.EnableSeeThroughManual(true);
        //PXR_Input.SetControllerVibration(1, 500, PXR_Input.Controller.RightController);
        //PXR_System.ScreenOff();

    }

}
