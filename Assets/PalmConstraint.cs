using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.PXR;

public class PalmConstraint : MonoBehaviour
{
    void Update()
    {
        HandJointLocations jointLocations = new HandJointLocations();
        PXR_HandTracking.GetJointLocations(0, ref jointLocations);
        Posef palmJoint = jointLocations.jointLocations[(int)HandJoint.JointPalm].pose;

        transform.SetPositionAndRotation(palmJoint.Position.ToVector3(), palmJoint.Orientation.ToQuat());
        Debug.Log("Pos: " + palmJoint.Position.ToVector3() + "Rot: " + palmJoint.Orientation.ToQuat());
    }
}
