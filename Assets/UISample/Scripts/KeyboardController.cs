using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Virtual keyboard controller
/// 1. Categorize and store all key objects
/// 2. Add listener event function for input keys, rollback keys....
/// 3. Provide a method to bind InputField
/// </summary>
public class KeyboardController : MonoBehaviour
{
    private List<ButtonItem> buttonList;
    //The input box component that is currently interacting
    private InputField inputField;
    
    private void Awake()
    {
        InitKeyboard();
    }

    /// <summary>
    /// Register call back function for each key click event
    /// </summary>
    private void OnEnable()
    {
        foreach (ButtonItem item in buttonList)
        {
            switch (item.buttonType)
            {
                //Letter Enter Blank Key is all input function
                case ButtonType.Letter:
                case ButtonType.Enter:
                case ButtonType.Blank:
                    item.button.onClick.AddListener(() =>
                    {
                        InputButtonClickHandler(item.buttonValue);
                    });
                    break;
                //Backspace key need to special handling
                case ButtonType.Backspace:
                    item.button.onClick.AddListener(BackspaceButtonClickHandler);
                    break;
            }
        }
    }
    
    /// <summary>
    /// Canceled all listener call back function on Disable
    /// </summary>
    private void OnDisable()
    {
        foreach (ButtonItem item in buttonList)
        {
            switch (item.buttonType)
            {
                case ButtonType.Letter:
                case ButtonType.Enter:
                case ButtonType.Blank:
                    item.button.onClick.RemoveAllListeners();
                    break;
                case ButtonType.Backspace:
                    item.button.onClick.RemoveAllListeners();
                    break;
            }
        }
    }
    
    /// <summary>
    /// Backspace : Delete trailing character
    /// </summary>
    private void BackspaceButtonClickHandler()
    {
        if (inputField == null || string.IsNullOrEmpty(inputField.text)) return;
        inputField.text = inputField.text.Remove(inputField.text.Length-1);
    }

    /// <summary>
    /// Input : Append the character to the end
    /// </summary>
    /// <param name="itemButtonValue">key item value</param>
    private void InputButtonClickHandler(string itemButtonValue)
    {
        if (inputField == null) return;
        inputField.text += itemButtonValue;
    }

    /// <summary>
    /// Init keyboard all key items
    /// </summary>
    private void InitKeyboard()
    {
        buttonList = new List<ButtonItem>();
        //Gets all keys and sorts them by name
        Button[] buttons = transform.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            ButtonItem item = new ButtonItem(button);
            buttonList.Add(item);
        }
    }

    #region Public Function

    /// <summary>
    /// Provides open and hidden functions
    /// </summary>
    /// <param name="state">open/close</param>
    public void SetKeyboardState(bool state)
    {
        this.gameObject.SetActive(state);
    }

    /// <summary>
    /// Bind inputField
    /// </summary>
    /// <param name="inputField">target inputField</param>
    public void SetInteractiveInputField(InputField inputField)
    {
        this.inputField = inputField;
    }
    #endregion

    
    public enum ButtonType
    {
        Letter,
        Backspace,
        Enter,
        Blank,
    }
    
    /// <summary>
    /// Key item, save the key's value,type,component.
    /// </summary>
    public class ButtonItem
    {
        public Button button { get; set; }
        public string buttonValue { get; set; }
        public ButtonType buttonType { get; set; }

        public ButtonItem(Button button)
        {
            this.button = button;
            buttonType = GetButtonType(button.gameObject.name);
            buttonValue = GetButtonValue(button.gameObject.name, buttonType);
        }
        
        /// <summary>
        /// Get key's type
        /// </summary>
        /// <param name="value">game object name</param>
        /// <returns>key's type</returns>
        /// <exception cref="Exception">Error：no such type</exception>
        private ButtonType GetButtonType(string value)
        {
            //judge letter keys
            if (value.Length == 1 && value[0] >= 'a' && value[0] <= 'z')
                return ButtonType.Letter;
            //judge special keys
            switch (value)
            {
                case "backspace" : return ButtonType.Backspace;
                case "enter" : return ButtonType.Enter;
                case "blank" : return ButtonType.Blank;
            }
            throw new Exception($"Error : There is no such type here value = {value}");
        }

        /// <summary>
        /// Get key's value
        /// </summary>
        private string GetButtonValue(string value,ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.Letter:
                    return value;
                case ButtonType.Blank:
                    return " ";
                case ButtonType.Enter:
                    return "\n";
                default:
                    return "";
            }
        }
    }
}
