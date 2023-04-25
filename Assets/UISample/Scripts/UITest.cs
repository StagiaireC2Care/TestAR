using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Test the use of common UI in VR
/// 1. Two buttons are provided to control the raising and lowering of sound
/// 2. Provides slider to adjust screen brightness
/// 3. Provides Toggle control for virtual keyboard display and hide
/// 4. Provides a virtual keyboard with input box, you can enter it in the input box
/// </summary>
public class UITest : MonoBehaviour
{
    //Android default input method
    private TouchScreenKeyboard board;
    //virtual keyboard controller
    private KeyboardController boardController;
    public Button volumeUPButton, volumeDownButton;
    private Slider brightnessSlider;
    private InputField testInputField;
    private Toggle toggle;

    //Find and init all Component
    private void Awake()
    {
        //Get the Default Android keyboard and disable it
        board = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
        board.active = false;
        boardController = GetComponentInChildren<KeyboardController>();
        toggle = GetComponentInChildren<Toggle>();
        testInputField = GetComponentInChildren<InputField>();
        brightnessSlider = GetComponentInChildren<Slider>();
        //Set the current active input field
        boardController.SetInteractiveInputField(testInputField);
        boardController.SetKeyboardState(false);
        //Init system service
        PXR_System.InitAudioDevice();
    }

    //Register call back function for each ui event
    private void OnEnable()
    {
        //Registers a method to listen for changes in a checkbox value
        toggle.onValueChanged.AddListener(ToggleValueChangedHandler);
        //Registers a slider value change listening method
        brightnessSlider.onValueChanged.AddListener(SliderValueChangedHandler);
        //Registers the button click listening method
        volumeUPButton.onClick.AddListener(VolumeUPButtonClickHandler);
        //Registers the button click listening method
        volumeDownButton.onClick.AddListener(VolumeDownButtonClickHandler);
    }
    
    //Canceled listener on disalbe
    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(ToggleValueChangedHandler);
        brightnessSlider.onValueChanged.RemoveListener(SliderValueChangedHandler);
        volumeUPButton.onClick.RemoveListener(VolumeUPButtonClickHandler);
        volumeDownButton.onClick.RemoveListener(VolumeDownButtonClickHandler);
    }

    private void VolumeDownButtonClickHandler()
    {
        bool isSuccess = PXR_System.VolumeDown();
        if(!isSuccess) Debug.Log("Error: VolumeDown Fail");
    }

    private void VolumeUPButtonClickHandler()
    {
        bool isSuccess = PXR_System.VolumeUp();
        if(!isSuccess) Debug.Log("Error: VolumeUP Fail");
    }
    
    private void SliderValueChangedHandler(float prop)
    {
        //The maximum brightness value is 255
        int curBrightness = (int)(prop * 255);
        PXR_System.SetCommonBrightness(curBrightness);
    }

    
    private void ToggleValueChangedHandler(bool isOpen)
    {
        boardController.SetKeyboardState(isOpen);
    }

    private void LateUpdate()
    {
        //Continue to disable the android default input method
        board.active = false;
    }
}
