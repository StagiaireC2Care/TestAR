using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller UI Function
/// 1. Add a log message to the left or right controller scrollRect content
/// 2. Refresh the scrollRect and canvas layout after append a log message
/// </summary>
public class ControllerUITest : MonoBehaviour
{
    #region UI Components
    //left and right Controller ScrollRect Object
    public ScrollRect leftLogScrollRect;
    public ScrollRect rightLogScrollRect;
    //left and right Controller ScrollRect Content Text Object
    private Text leftLogInfo;
    private Text rightLogInfo;
    #endregion

    private ControllerSampleInputActions inputActions;
    private void Awake()
    {
        leftLogInfo = leftLogScrollRect.content.GetComponent<Text>();
        rightLogInfo = rightLogScrollRect.content.GetComponent<Text>();
        inputActions = new ControllerSampleInputActions();
    }

    /// <summary>
    /// Add a log message to the left or right controller scrollRect content
    /// </summary>
    /// <param name="info">the Log Message</param>
    /// <param name="isLeft">Mark left or right</param>
    public void AppendLogInfo(string info , bool isLeft)
    {
        string itemInfo = info + "\n";
        if (isLeft)
        {
            leftLogInfo.text += itemInfo;
            //Refresh text layout after append message in text
            Canvas.ForceUpdateCanvases();
            //to display the latest messages
            leftLogScrollRect.verticalNormalizedPosition = 0f;
        }
        else
        {
            rightLogInfo.text += itemInfo;
            //Refresh text layout after append message in text
            Canvas.ForceUpdateCanvases();
            rightLogScrollRect.verticalNormalizedPosition = 0f;
        }
        //Refresh scrollRect layout to displays the latest messages
        Canvas.ForceUpdateCanvases();
        
        
    }
    
    /// <summary>
    /// Register controller input events to display log message
    /// </summary>
    private void OnEnable()
    {
        //=============Register right controller input action listener =============
        inputActions.RightController.AX.Enable();
        inputActions.RightController.AX.performed += context => AppendLogInfo("Right AX has been Click", false);
        
        inputActions.RightController.BY.Enable();
        inputActions.RightController.BY.performed += context => AppendLogInfo("Right BY has been Click", false);
        
        inputActions.RightController.App.Enable();
        inputActions.RightController.App.started += context => AppendLogInfo("Right App has been Click", false);

        inputActions.RightController.JoystickClick.Enable();
        inputActions.RightController.JoystickClick.started += context => AppendLogInfo("Right Joy has been Click", false);

        //=============Register left controller input action listener =============
        inputActions.LeftController.AX.Enable();
        inputActions.LeftController.AX.performed += context => AppendLogInfo("Left AX has been Click", true);

        inputActions.LeftController.BY.Enable();
        inputActions.LeftController.BY.performed += context => AppendLogInfo("Left BY has been Click", true);
        
        inputActions.LeftController.App.Enable();
        inputActions.LeftController.App.performed += context => AppendLogInfo("Left App has been Click", true);

        inputActions.LeftController.JoystickClick.Enable();
        inputActions.LeftController.JoystickClick.performed += context => AppendLogInfo("Left Joy has been Click", true);

    }
    
    
}
