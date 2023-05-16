using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.Events;

public class SeeThroughMgr : MonoBehaviour
{

    public UnityEvent OnSeeThroughEnabled;
    public UnityEvent OnSeeThroughDisabled;
    private Camera mainCamera;
    private CameraClearFlags defaultClearFlag;
    private bool seeThroughEnabled;

    void Awake() 
    {
        mainCamera = Camera.main;
        defaultClearFlag = mainCamera.clearFlags;
    }

    public void SetSeeThrough (bool enabled)
    {
        mainCamera.clearFlags = enabled ? CameraClearFlags.SolidColor : defaultClearFlag;
        mainCamera.backgroundColor = new Color(0,0,0,0);
        //mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing = !enabled;
        PXR_Boundary.EnableSeeThroughManual(enabled);
        seeThroughEnabled=enabled;
        (enabled ? OnSeeThroughEnabled : OnSeeThroughDisabled).Invoke();
    }

    private void OnApplicationPause(bool pause) 
    {
        if(!pause && seeThroughEnabled==true)
        {
            SetSeeThrough(true);
        }
    }

    /*void OnEnable ()
    {
        EventManager.StartListening ("augmented_reality", () => { SetSeeThrough(NetClientManager.boolMsg.value); });
	}

    void OnDisable ()
    {
        EventManager.StopListening ("augmented_reality", () => { SetSeeThrough(NetClientManager.boolMsg.value); });
    }*/

}
