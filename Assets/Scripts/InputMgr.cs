using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputMgr : MonoBehaviour
{
    public static InputMgr instance { get; private set; }

    public enum DeviceModel { DEFAULT, OPENXR, PICO }

    private InputDevice leftHandDevice, rightHandDevice, headDevice;
    private float backBtnTimer, debugBtnTimer, debugSuccessBtnTimer, debugFailBtnTimer;
    private string deviceName;
    private DeviceModel deviceModel;
    private string devicePlatform;

    private enum ButtonStatus { Inactive, Down, Active, Up };

    private ButtonStatus leftPrimaryButtonStatus;
    private ButtonStatus rightPrimaryButtonStatus;
    private ButtonStatus leftSecondaryButtonStatus;
    private ButtonStatus rightSecondaryButtonStatus;
    private ButtonStatus leftTriggerButtonStatus;
    private ButtonStatus rightTriggerButtonStatus;
    private ButtonStatus leftGripButtonStatus;
    private ButtonStatus rightGripButtonStatus;
    private ButtonStatus leftMenuButtonStatus;
    private ButtonStatus rightMenuButtonStatus;
    private ButtonStatus leftPrimary2DAxisStatus;
    private ButtonStatus rightPrimary2DAxisStatus;
    private ButtonStatus leftSecondary2DAxisStatus;
    private ButtonStatus rightSecondary2DAxisStatus;

    public static InputMgr GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        StartCoroutine(this.SlowUpdate());

#if UNITY_STANDALONE_WIN
        devicePlatform = "windows";
#elif UNITY_ANDROID
		devicePlatform = "android";
#endif
    }
    private void Update()
    {
        UpdateButtonStatus(leftHandDevice, CommonUsages.primaryButton, ref leftPrimaryButtonStatus);
        UpdateButtonStatus(rightHandDevice, CommonUsages.primaryButton, ref rightPrimaryButtonStatus);
        UpdateButtonStatus(leftHandDevice, CommonUsages.secondaryButton, ref leftSecondaryButtonStatus);
        UpdateButtonStatus(rightHandDevice, CommonUsages.secondaryButton, ref rightSecondaryButtonStatus);
        UpdateButtonStatus(leftHandDevice, CommonUsages.triggerButton, ref leftTriggerButtonStatus);
        UpdateButtonStatus(rightHandDevice, CommonUsages.triggerButton, ref rightTriggerButtonStatus);
        UpdateButtonStatus(leftHandDevice, CommonUsages.gripButton, ref leftGripButtonStatus);
        UpdateButtonStatus(rightHandDevice, CommonUsages.gripButton, ref rightGripButtonStatus);
        UpdateButtonStatus(leftHandDevice, CommonUsages.menuButton, ref leftMenuButtonStatus);
        UpdateButtonStatus(rightHandDevice, CommonUsages.menuButton, ref rightMenuButtonStatus);
        UpdateButtonStatus(leftHandDevice, CommonUsages.primary2DAxisClick, ref leftPrimary2DAxisStatus);
        UpdateButtonStatus(rightHandDevice, CommonUsages.primary2DAxisClick, ref rightPrimary2DAxisStatus);
        UpdateButtonStatus(leftHandDevice, CommonUsages.secondary2DAxisClick, ref leftSecondary2DAxisStatus);
        UpdateButtonStatus(rightHandDevice, CommonUsages.secondary2DAxisClick, ref rightSecondary2DAxisStatus);

        if (Input.GetKeyDown(KeyCode.Insert))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private IEnumerator SlowUpdate()
    {
        while (true)
        {
            this.UpdateDevices(headDevice);

            yield return new WaitForSecondsRealtime(1.0f);
            yield return new WaitForEndOfFrame();
        }
    }

    private void UpdateDevices(InputDevice device)
    {
        var devices = new List<UnityEngine.XR.InputDevice>();

        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, devices);
        if (devices.Count > 0)
            leftHandDevice = devices[0];

        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, devices);
        if (devices.Count > 0)
            rightHandDevice = devices[0];

        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.Head, devices);
        if (devices.Count > 0)
            headDevice = devices[0];

        deviceName = devices.Count > 0 ? headDevice.name : "Unknown";

        if (XRSettings.loadedDeviceName.ToLower().Contains("pico"))
        {
            deviceModel = DeviceModel.PICO;
        }
        else if (XRSettings.loadedDeviceName.ToLower().Contains("openxr"))
        {
            deviceModel = DeviceModel.OPENXR;
        }
        else
        {
            deviceModel = DeviceModel.DEFAULT;
        }
    }

    #region PRIMARY BUTTON
    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur la touche principale.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool PrimaryButtonDown()
    {
        return LeftPrimaryButtonDown() || RightPrimaryButtonDown();
    }

    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur la touche principale manette droite.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool RightPrimaryButtonDown()
    {
        return rightPrimaryButtonStatus == ButtonStatus.Down || Input.GetMouseButtonDown(0);
    }

    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur la touche principale manette droite.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool LeftPrimaryButtonDown()
    {
        return leftPrimaryButtonStatus == ButtonStatus.Down || Input.GetMouseButtonDown(0);
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la touche principale.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool PrimaryButton()
    {
        return LeftPrimaryButton() || RightPrimaryButton();
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la touche principale manette droite.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool RightPrimaryButton()
    {
        return rightPrimaryButtonStatus == ButtonStatus.Active || Input.GetMouseButton(0);
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la touche principale manette gauche.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool LeftPrimaryButton()
    {
        return leftPrimaryButtonStatus == ButtonStatus.Active || Input.GetMouseButton(0);
    }

    /// <summary>
    /// Renvoie true lors de la frame où l'on relache la touche principale.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonUp" d'unity).
    /// </summary>
    public bool PrimaryButtonUp()
    {
        return LeftPrimaryButtonUp() || RightPrimaryButtonUp();
    }

    /// <summary>
    /// Renvoie true lors de la frame où l'on relache la touche principale manette droite.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool RightPrimaryButtonUp()
    {
        return rightPrimaryButtonStatus == ButtonStatus.Up || Input.GetMouseButtonUp(0);
    }

    /// <summary>
    /// Renvoie true lors de la frame où l'on relache la touche principale manette droite.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool LeftPrimaryButtonUp()
    {
        return leftPrimaryButtonStatus == ButtonStatus.Up || Input.GetMouseButtonUp(0);
    }
    #endregion
    #region SECONDARY BUTTON
    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur la touche secondaire.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool SecondaryButtonDown()
    {
        return LeftSecondaryButtonDown() || RightSecondaryButtonDown();
    }

    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur la touche secondaire manette droite.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool RightSecondaryButtonDown()
    {
        return rightSecondaryButtonStatus == ButtonStatus.Down || Input.GetMouseButtonDown(1);
    }

    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur la touche secondaire manette droite.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool LeftSecondaryButtonDown()
    {
        return leftSecondaryButtonStatus == ButtonStatus.Down || Input.GetMouseButtonDown(1);
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la touche secondaire.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool SecondaryButton()
    {
        return LeftSecondaryButton() || RightSecondaryButton();
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la touche secondaire manette droite.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool RightSecondaryButton()
    {
        return rightSecondaryButtonStatus == ButtonStatus.Active || Input.GetMouseButton(1);
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la touche secondaire manette gauche.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool LeftSecondaryButton()
    {
        return leftSecondaryButtonStatus == ButtonStatus.Active || Input.GetMouseButton(1);
    }

    /// <summary>
    /// Renvoie true lors de la frame où l'on relache la touche principale.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonUp" d'unity).
    /// </summary>
    public bool SecondaryButtonUp()
    {
        return LeftSecondaryButtonUp() || RightSecondaryButtonUp();
    }

    /// <summary>
    /// Renvoie true lors de la frame où l'on relache la touche principale manette droite.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool RightSecondaryButtonUp()
    {
        return rightSecondaryButtonStatus == ButtonStatus.Up || Input.GetMouseButtonUp(1);
    }

    /// <summary>
    /// Renvoie true lors de la frame où l'on relache la touche principale manette droite.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool LeftSecondaryButtonUp()
    {
        return leftSecondaryButtonStatus == ButtonStatus.Up || Input.GetMouseButtonUp(1);
    }
    #endregion
    #region MENU BUTTON
    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur une touche de menu.
    /// Si on n'a pas appuyé sur une touche ou on est en train de maintenir, retourne false.
    /// </summary>
    public bool MenuButtonDown()
    {
        //On considère toujours les boutons Y et B comme étant les boutons de menu
        return SecondaryButtonDown();

        // switch (deviceModel)
        // {
        //     case DeviceModel.OPENXR:
        //         return SecondaryButtonDown();
        //     default:
        //         return leftMenuButtonStatus == ButtonStatus.Down || rightMenuButtonStatus == ButtonStatus.Down || Input.GetKeyDown(KeyCode.Escape);
        // }
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur une touche de menu.
    /// Si on n'a pas appuyé sur une touche, retourne false.
    /// </summary>
    public bool MenuButton()
    {
        //On considère toujours les boutons Y et B comme étant les boutons de menu
        return SecondaryButton();

        // switch (deviceModel)
        // {
        //     case DeviceModel.OPENXR:
        //         return SecondaryButton();
        //     default:
        //         return leftMenuButtonStatus == ButtonStatus.Active || rightMenuButtonStatus == ButtonStatus.Active || Input.GetKey(KeyCode.Escape);
        // }
    }

    /// <summary>
    /// Renvoie true lors de la frame où l'on relache une touche de menu.
    /// Si on n'a pas appuyé sur une touche, retourne false.
    /// </summary>
    public bool MenuButtonUp()
    {
        //On considère toujours les boutons Y et B comme étant les boutons de menu
        return SecondaryButtonUp();

        // switch (deviceModel)
        // {
        //     case DeviceModel.OPENXR:
        //         return SecondaryButtonUp();
        //     default:
        //         return leftMenuButtonStatus == ButtonStatus.Up || rightMenuButtonStatus == ButtonStatus.Up || Input.GetKeyUp(KeyCode.Escape);
        // }
    }
    #endregion
    #region ACTION BUTTON
    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur une touche d'action.
    /// Si on n'a pas appuyé sur une touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool ActionButtonDown()
    {
        return LeftActionButtonDown() || RightActionButtonDown();
    }

    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur une touche d'action manette droite.
    /// Si on n'a pas appuyé sur une touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool RightActionButtonDown()
    {
        //On considère toujours les boutons X ou A comme étant les boutons d'action
        return RightPrimaryButtonDown();

        //return RightPrimaryButtonDown() || RightSecondaryButtonDown();
    }

    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur une touche d'action manette droite.
    /// Si on n'a pas appuyé sur une touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool LeftActionButtonDown()
    {
        //On considère toujours les boutons X ou A comme étant les boutons d'action
        return LeftPrimaryButtonDown();

        //return LeftPrimaryButtonDown() || LeftSecondaryButtonDown();
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la touche d'action.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool ActionButton()
    {
        return LeftActionButton() || RightActionButton();
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la touche d'action manette droite.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool RightActionButton()
    {
        //On considère toujours les boutons X ou A comme étant les boutons d'action
        return RightPrimaryButton();

        //return RightPrimaryButton() || RightSecondaryButton();
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la touche d'action manette gauche.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool LeftActionButton()
    {
        //On considère toujours les boutons X ou A comme étant les boutons d'action
        return LeftPrimaryButton();

        //return LeftPrimaryButton() || LeftSecondaryButton();
    }

    /// <summary>
    /// Renvoie true lors de la frame où l'on relache la touche d'action.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool ActionButtonUp()
    {
        return LeftActionButtonUp() || RightActionButtonUp();
    }

    /// <summary>
    /// Renvoie true lors de la frame où l'on relache la touche d'action manette droite.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool RightActionButtonUp()
    {
        //On considère toujours les boutons X ou A comme étant les boutons d'action
        return RightPrimaryButtonUp();

        //return RightPrimaryButtonUp() || RightSecondaryButtonUp();
    }

    /// <summary>
    /// Renvoie true lors de la frame où l'on relache la touche d'action manette gauche.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool LeftActionButtonUp()
    {
        //On considère toujours les boutons X ou A comme étant les boutons d'action
        return LeftPrimaryButtonUp();

        //return LeftPrimaryButtonUp() || LeftSecondaryButtonUp();
    }
    #endregion
    #region BACK BUTTON
    /// <summary>
    /// Renvoie true si on appuie sur la touche retour pendant 3 secondes.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// </summary>
    public bool BackButton()
    {
        bool isPressed = MenuButton();

        if (isPressed)
        {
            backBtnTimer += Time.unscaledDeltaTime;
            if (backBtnTimer > 3)
            {
                backBtnTimer = 0;
                return true;
            }
            return false;
        }
        else
        {
            backBtnTimer = 0;
            return false;
        }
    }
    #endregion
    #region TRIGGER
    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur une gachette.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool TriggerButtonDown()
    {
        return RightTriggerButtonDown() || LeftTriggerButtonDown();
    }

    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur la gachette manette droite.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool RightTriggerButtonDown()
    {
        return rightTriggerButtonStatus == ButtonStatus.Down || Input.GetMouseButtonDown(2);
    }

    /// <summary>
    /// Renvoie true lors de la frame pendant laquelle on a débuté l'appui sur la gachette manette gauche.
    /// Si on n'a pas appuyé sur la touche ou on est en train de maintenir, retourne false.
    /// (Comparable à un "GetButtonDown" d'unity).
    /// </summary>
    public bool LeftTriggerButtonDown()
    {
        return leftTriggerButtonStatus == ButtonStatus.Down || Input.GetMouseButtonDown(2);
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur une gachette.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool TriggerButton()
    {
        return RightTriggerButton() || LeftTriggerButton();
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la gachette manette droite.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool RightTriggerButton()
    {
        return rightTriggerButtonStatus == ButtonStatus.Active || Input.GetMouseButton(2);
    }

    /// <summary>
    /// Renvoie true à chaque frame où on appuie sur la gachette manette gauche.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// (Comparable à un "GetButton" d'unity).
    /// </summary>
    public bool LeftTriggerButton()
    {
        return leftTriggerButtonStatus == ButtonStatus.Active || Input.GetMouseButton(2);
    }

    /// <summary>
    /// Returns true on the first frame where the trigger button (right or left) is released.
    /// </summary>
    /// <returns></returns>
    public bool TriggerButtonUp()
    {
        return RightTriggerButtonUp() || LeftTriggerButtonUp();
    }

    /// <summary>
    /// Returns true on the first frame where the right trigger button is released.
    /// </summary>
    /// <returns></returns>
    public bool RightTriggerButtonUp()
    {
        return rightTriggerButtonStatus == ButtonStatus.Up || Input.GetMouseButtonUp(2);
    }

    /// <summary>
    /// Returns true on the first frame where the left trigger button is released.
    /// </summary>
    /// <returns></returns>
    public bool LeftTriggerButtonUp()
    {
        return leftTriggerButtonStatus == ButtonStatus.Up || Input.GetMouseButtonUp(2);
    }

    /// <summary>
    /// Renvoie la valeur entre 0 et 1 de la pression sur une gachette à chaque frame.
    /// (Priorité à la manette gauche).
    /// Si on n'a pas appuyé sur la touche, retourne 0.
    /// </summary>
    public float TriggerForce()
    {
        float leftTriggerForce = LeftTriggerForce();
        float rightTriggerForce = RightTriggerForce();
        if (leftTriggerForce >= rightTriggerForce)
        {
            return LeftTriggerForce();
        }
        else
        {
            return RightTriggerForce();
        }
    }

    /// <summary>
    /// Renvoie la valeur entre 0 et 1 de la pression sur une gachette gauche à chaque frame.
    /// Si on n'a pas appuyé sur la touche, retourne 0.
    /// </summary>
    public float LeftTriggerForce()
    {
        if (!UnityEngine.XR.XRSettings.enabled)
        {
            return Input.GetMouseButton(2) ? 1f : 0f;
        }
        else
        {
            return GetButtonForce(leftHandDevice, CommonUsages.trigger, 0.01f);
        }
    }

    /// <summary>
    /// Renvoie la valeur entre 0 et 1 de la pression sur une gachette droite à chaque frame.
    /// Si on n'a pas appuyé sur la touche, retourne 0.
    /// </summary>
    public float RightTriggerForce()
    {
        if (!UnityEngine.XR.XRSettings.enabled)
        {
            return Input.GetMouseButton(2) ? 1f : 0f;
        }
        else
        {
            return GetButtonForce(rightHandDevice, CommonUsages.trigger, 0.01f);
        }
    }
    #endregion
    #region GRIP
    /// <summary>
    /// Returns true on the first frame where the grip button (left or right) is pressed.
    /// </summary>
    public bool GripButtonDown()
    {
        return LeftGripButtonDown() || RightGripButtonDown();
    }

    /// <summary>
    /// Returns true on the first frame where the right grip button is pressed.
    /// </summary>
    public bool RightGripButtonDown()
    {
        return rightGripButtonStatus == ButtonStatus.Down || Input.GetKeyDown(KeyCode.G);
    }

    /// <summary>
    /// Returns true on the first frame where the left grip button is pressed.
    /// </summary>
    public bool LeftGripButtonDown()
    {
        return leftGripButtonStatus == ButtonStatus.Down || Input.GetKeyDown(KeyCode.G);
    }

    /// <summary>
    /// Returns true if the grip button (left or right) is pressed.
    /// </summary>
    public bool GripButton()
    {
        return LeftGripButton() || RightGripButton();
    }

    /// <summary>
    /// Returns true if the right grip button is pressed.
    /// </summary>
    public bool RightGripButton()
    {
        return rightGripButtonStatus == ButtonStatus.Active || Input.GetKey(KeyCode.G);
    }

    /// <summary>
    /// Returns true if the left grip button is pressed.
    /// </summary>
    public bool LeftGripButton()
    {
        return leftGripButtonStatus == ButtonStatus.Active || Input.GetKey(KeyCode.G);
    }

    /// <summary>
    /// Returns true on the first frame where the grip button (left or right) is released.
    /// </summary>
    public bool GripButtonUp()
    {
        return LeftGripButtonUp() || RightGripButtonUp();
    }

    /// <summary>
    /// Returns true on the first frame where the right grip button is released.
    /// </summary>
    public bool RightGripButtonUp()
    {
        return rightGripButtonStatus == ButtonStatus.Up || Input.GetKeyUp(KeyCode.G);
    }

    /// <summary>
    /// Returns true on the first frame where the left grip button is released.
    /// </summary>
    public bool LeftGripButtonUp()
    {
        return leftGripButtonStatus == ButtonStatus.Up || Input.GetKeyUp(KeyCode.G);
    }

    /// <summary>
    /// Returns a value between 0 and 1, depending on how much the grip button (left or right) is pressed.
    /// </summary>
    public float GripForce()
    {
        float leftGripForce = LeftGripForce();
        float rightGripForce = RightGripForce();
        if (leftGripForce >= rightGripForce)
        {
            return LeftGripForce();
        }
        else
        {
            return RightGripForce();
        }
    }

    /// <summary>
    /// Returns a value between 0 and 1, depending on how much the left grip button is pressed.
    /// </summary>
    public float LeftGripForce()
    {
        if (!UnityEngine.XR.XRSettings.enabled)
        {
            return Input.GetKey(KeyCode.G) ? 1f : 0f;
        }
        else
        {
            return GetButtonForce(leftHandDevice, CommonUsages.grip, 0.01f);
        }
    }

    /// <summary>
    /// Returns a value between 0 and 1, depending on how much the right grip button is pressed.
    /// </summary>
    public float RightGripForce()
    {
        if (!UnityEngine.XR.XRSettings.enabled)
        {
            return Input.GetKey(KeyCode.G) ? 1f : 0f;
        }
        else
        {
            return GetButtonForce(rightHandDevice, CommonUsages.grip, 0.01f);
        }
    }
    #endregion
    #region GRAB BUTTON
    /// <summary>
    /// Returns true on the first frame where the action grab button (left or right) is pressed.
    /// </summary>
    /// <returns></returns>
    public bool ActionGrabButtonDown()
    {
        return LeftActionGrabButtonDown() || RightActionGrabButtonDown();
    }

    /// <summary>
    /// Returns true on the first frame where the right action grab button is pressed.
    /// </summary>
    /// <returns></returns>
    public bool RightActionGrabButtonDown()
    {
        return RightPrimaryButtonDown() || RightSecondaryButtonDown() || RightTriggerButtonDown() || RightGripButtonDown();
    }

    /// <summary>
    /// Returns true on the first frame where the left action grab button is pressed.
    /// </summary>
    /// <returns></returns>
    public bool LeftActionGrabButtonDown()
    {
        return LeftPrimaryButtonDown() || LeftSecondaryButtonDown() || LeftTriggerButtonDown() || LeftGripButtonDown();
    }

    /// <summary>
    /// Returns true when the action grab button (left or right) is pressed.
    /// </summary>
    public bool ActionGrabButton()
    {
        return LeftActionGrabButton() || RightActionGrabButton();
    }

    /// <summary>
    /// Returns true when the right action grab button is pressed.
    /// </summary>
    public bool RightActionGrabButton()
    {
        return RightPrimaryButton() || RightSecondaryButton() || RightTriggerButton() || RightGripButton();
    }

    /// <summary>
    /// Returns true when the left action grab button is pressed.
    /// </summary>
    public bool LeftActionGrabButton()
    {
        return LeftPrimaryButton() || LeftSecondaryButton() || LeftTriggerButton() || LeftGripButton();
    }

    /// <summary>
    /// Returns true on the first frame where the action grab button (left or right) is released.
    /// </summary>
    public bool ActionGrabButtonUp()
    {
        return LeftActionGrabButtonUp() || RightActionGrabButtonUp();
    }

    /// <summary>
    /// Returns true on the first frame where the right action grab button is released.
    /// </summary>
    public bool RightActionGrabButtonUp()
    {
        return RightPrimaryButtonUp() || RightSecondaryButtonUp() || RightTriggerButtonUp() || RightGripButtonUp();
    }

    /// <summary>
    /// Returns true on the first frame where the left action grab button is released.
    /// </summary>
    public bool LeftActionGrabButtonUp()
    {
        return LeftPrimaryButtonUp() || LeftSecondaryButtonUp() || LeftTriggerButtonUp() || LeftGripButtonUp();
    }
    #endregion
    #region JOYSTICK
    /// <summary>
    /// Retourne les valeurs X et Y des joysticks (priorité au gauche).
    /// </summary>
    /// <param name="dead_zone">Valeur minimale à atteindre pour retourner une valeur supérieur à zéro</param>
    public Vector2 Joystick(float dead_zone = 0.2f)
    {
        if (LeftJoystick(dead_zone) != Vector2.zero)
            return LeftJoystick(dead_zone);

        if (RightJoystick(dead_zone) != Vector2.zero)
            return RightJoystick(dead_zone);

        return Vector2.zero;
    }

    /// <summary>
    /// Retourne les valeurs X et Y du joystick gauche.
    /// </summary>
    /// <param name="dead_zone">Valeur minimale à atteindre pour retourner une valeur supérieur à zéro</param>
    public Vector2 LeftJoystick(float dead_zone = 0.2f)
    {
        if (!UnityEngine.XR.XRSettings.enabled)
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        else
        {
            return GetAxis(leftHandDevice, CommonUsages.primary2DAxis, dead_zone);
        }
    }

    /// <summary>
    /// Retourne les valeurs X et Y du joystick droit.
    /// </summary>
    /// <param name="dead_zone">Valeur minimale à atteindre pour retourner une valeur supérieur à zéro</param>
    public Vector2 RightJoystick(float dead_zone = 0.2f)
    {
        if (!UnityEngine.XR.XRSettings.enabled)
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        else
        {
            return GetAxis(rightHandDevice, CommonUsages.primary2DAxis, dead_zone);
        }
    }

    /// <summary>
    /// Appui sur un des joysticks des manettes.
    /// </summary>
    /// <returns></returns>
    public bool JoystickButton()
    {
        return LeftJoystickButton() || RightJoystickButton();
    }

    /// <summary>
    /// Returns true on the first frame where one of the joystick buttons is released.
    /// </summary>
    /// <returns></returns>
    public bool JoystickButtonUp()
    {
        return LeftJoystickButtonUp() || RightJoystickButtonUp();
    }

    /// <summary>
    /// Returns true on the first frame where one of the joystick buttons is pressed.
    /// </summary>
    /// <returns></returns>
    public bool JoystickButtonDown()
    {
        return LeftJoystickButtonDown() || RightJoystickButtonDown();
    }

    /// <summary>
    /// Appui sur le joystick de la manette gauche.
    /// </summary>
    /// <returns></returns>
    public bool LeftJoystickButton()
    {
        return leftPrimary2DAxisStatus == ButtonStatus.Active;
    }

    /// <summary>
    /// Returns true on the first frame where the left joystick button is released.
    /// </summary>
    /// <returns></returns>
    public bool LeftJoystickButtonUp()
    {
        return leftPrimary2DAxisStatus == ButtonStatus.Up;
    }

    /// <summary>
    /// Returns true on the first frame where the left joystick button is pressed.
    /// </summary>
    /// <returns></returns>
    public bool LeftJoystickButtonDown()
    {
        return leftPrimary2DAxisStatus == ButtonStatus.Down;
    }

    /// <summary>
    /// Appui sur le joystick de la manette droite.
    /// </summary>
    /// <returns></returns>
    public bool RightJoystickButton()
    {
        return rightPrimary2DAxisStatus == ButtonStatus.Active;
    }

    /// <summary>
    /// Returns true on the first frame where the right joystick button is released.
    /// </summary>
    /// <returns></returns>
    public bool RightJoystickButtonUp()
    {
        return rightPrimary2DAxisStatus == ButtonStatus.Up;
    }

    /// <summary>
    /// Returns true on the first frame where the right joystick button is pressed.
    /// </summary>
    /// <returns></returns>
    public bool RightJoystickButtonDown()
    {
        return rightPrimary2DAxisStatus == ButtonStatus.Down;
    }
    #endregion
    #region POSITION / ROTATION
    /// <summary>
    /// Get Right controller postion depending on device
    /// </summary>
    /// <returns>return Vector3 -> Right controller position </returns>
    public Vector3 GetRightControllerPosition()
    {
        Vector3 devicePosition = new Vector3();
        rightHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out devicePosition);
        return devicePosition;
    }

    /// <summary>
    /// Get Left controller postion depending on device
    /// </summary>
    /// <returns>return Vector3 -> Left controller position </returns>
    public Vector3 GetLeftControllerPosition()
    {
        Vector3 devicePosition = new Vector3();
        leftHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out devicePosition);
        return devicePosition;
    }

    /// <summary>
    /// Get controller rotation depending on device
    /// </summary>
    /// <returns>return Quaternion -> controller rotation </returns>
    public Quaternion GetRightControllerRotation()
    {
        Quaternion deviceRotation = new Quaternion();
        rightHandDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation);

        if (deviceModel == DeviceModel.OPENXR)
        {
            return deviceRotation * Quaternion.Euler(50, 0, 0);
        }

        return deviceRotation;
    }

    /// <summary>
    /// Get controller rotation depending on device
    /// </summary>
    /// <returns>return Quaternion -> controller rotation </returns>
    public Quaternion GetLeftControllerRotation()
    {
        Quaternion deviceRotation = new Quaternion();
        leftHandDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation);

        if (deviceModel == DeviceModel.OPENXR)
        {
            return deviceRotation * Quaternion.Euler(50, 0, 0);
        }

        return deviceRotation;
    }

    /// <summary>
    /// Get the right controller velocity.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRightControllerVelocity()
    {
        Vector3 devicePosition = new Vector3();
        rightHandDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out devicePosition);
        return devicePosition;
    }

    /// <summary>
    /// Get the left controller velocity.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLeftControllerVelocity()
    {
        Vector3 devicePosition = new Vector3();
        leftHandDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out devicePosition);
        return devicePosition;
    }

    /// <summary>
    /// Get the right controller velocity.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRightControllerAngularVelocity()
    {
        Vector3 devicePosition = new Vector3();
        rightHandDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out devicePosition);
        return devicePosition;
    }

    /// <summary>
    /// Get the left hand velocity.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLeftControllerAngularVelocity()
    {
        Vector3 devicePosition = new Vector3();
        leftHandDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out devicePosition);
        return devicePosition;
    }

    /// <summary>
    /// Get head (HMD) position depending on device
    /// </summary>
    /// <returns>return Vector3 -> Left controller position </returns>
    public Vector3 GetHeadPosition()
    {
        Vector3 devicePosition = new Vector3();
        headDevice.TryGetFeatureValue(CommonUsages.devicePosition, out devicePosition);
        return devicePosition;
    }

    /// <summary>
    /// Get head (HMD) rotation depending on device
    /// </summary>
    /// <returns>return Quaternion -> controller rotation </returns>
    public Quaternion GetHeadRotation()
    {
        Quaternion deviceRotation = new Quaternion();
        headDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation);
        return deviceRotation;
    }
    #endregion
    #region OTHERS
    /// <summary>
    /// Recentre la vue
    /// </summary>
    public void Recenter()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);
        foreach (var device in devices)
        {
            device.subsystem.TryRecenter();
        }
    }
    /// <summary>
    /// Retourne true si on appuie sur un des boutons permettant d'accélérer.
    /// </summary>
    public bool SpeedMove()
    {
        return (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift));
    }

    /// <summary>
    /// Renvoie true si on appuie sur la combinaison de touche de debug pendant "holdDuration" secondes.
    /// Si on n'a pas appuyé sur la touche, retourne false.
    /// </summary>
    public bool DebugButton(float holdDuration)
    {
        bool isPressed = false;

        isPressed = (leftPrimary2DAxisStatus == ButtonStatus.Active && rightPrimary2DAxisStatus == ButtonStatus.Active)
                  || (Input.GetKey(KeyCode.Escape) && Input.GetKey(KeyCode.F1));

        if (isPressed)
        {
            debugBtnTimer += Time.unscaledDeltaTime;
            if (debugBtnTimer > holdDuration)
            {
                debugBtnTimer = 0;
                return true;
            }
            return false;
        }
        else
        {
            debugBtnTimer = 0;
            return false;
        }
    }

    /// <summary>
    /// Bouton de debug pour un cas de "succès".
    /// </summary>
    /// <returns></returns>
    public bool DebugSuccessButton(float holdDuration)
    {
        bool isPressed = RightJoystickButton() || Input.GetKey(KeyCode.F1);
        if (isPressed)
        {
            debugSuccessBtnTimer += Time.unscaledDeltaTime;
            if (debugSuccessBtnTimer > holdDuration)
            {
                debugSuccessBtnTimer = 0;
                return true;
            }
            return false;
        }
        else
        {
            debugSuccessBtnTimer = 0;
            return false;
        }
    }

    /// <summary>
    /// Bouton de debug pour un cas d'échec.
    /// </summary>
    /// <returns></returns>
    public bool DebugFailButton(float holdDuration)
    {
        bool isPressed = LeftJoystickButton() || Input.GetKey(KeyCode.F2);
        if (isPressed)
        {
            debugFailBtnTimer += Time.unscaledDeltaTime;
            if (debugFailBtnTimer > holdDuration)
            {
                debugFailBtnTimer = 0;
                return true;
            }
            return false;
        }
        else
        {
            debugFailBtnTimer = 0;
            return false;
        }
    }

    /// <summary>
    /// Retourne le nom du casque utilisé.
    /// </summary>
    public string GetDeviceName()
    {
        return deviceName;
    }

    /// <summary>
    /// Retourne le model du casque utilisé.
    /// </summary>
    public DeviceModel GetDeviceModel()
    {
        return deviceModel;
    }

    /// <summary>
    /// Retourne la plateforme sur laquelle le casque est lancé (windows, android).
    /// </summary>
    public string GetDevicePlatform()
    {
        return devicePlatform;
    }
    #endregion
    #region VIBRATION
    /// <summary>
    /// Applique une vibration aux deux manettes.
    /// </summary>
    /// <param name="force">force de la vibration entre 0 et 1</param>
    public void SetVibration(float force, float duration = 1f)
    {
        SetLeftVibration(force, duration);
        SetRightVibration(force, duration);
    }

    /// <summary>
    /// Applique une vibration à la manette gauche.
    /// </summary>
    /// <param name="force">force de la vibration entre 0 et 1</param>
    public void SetLeftVibration(float force, float duration = 1f)
    {
        SetVibration(leftHandDevice, force, duration);
    }

    /// <summary>
    /// Applique une vibration à la manette droite.
    /// </summary>
    /// <param name="force">force de la vibration entre 0 et 1</param>
    public void SetRightVibration(float force, float duration = 1f)
    {
        SetVibration(rightHandDevice, force, duration);
    }

    /// <summary>
    /// Stop les vibrations sur les deux manettes.
    /// </summary>
    public void StopVibration()
    {
        StopLeftVibration();
        StopRightVibration();
    }

    /// <summary>
    /// Stop les vibrations sur la manette gauche.
    /// </summary>
    public void StopLeftVibration()
    {
        SetVibration(leftHandDevice, 0f, 1f);
    }

    /// <summary>
    /// Stop les vibrations sur la manette droite.
    /// </summary>
    public void StopRightVibration()
    {
        SetVibration(rightHandDevice, 0f, 1f);
    }
    #endregion
    #region STATUS
    /// <summary>
    /// Permet de savoir si au moins une des manettes est connectée
    /// </summary>
    public bool IsControllerConnected()
    {
        return IsLeftControllerConnected() || IsRightControllerConnected();
    }

    /// <summary>
    /// Permet de savoir si la manette gauche est connectée
    /// </summary>
    public bool IsLeftControllerConnected()
    {
        return leftHandDevice.isValid;
    }

    /// <summary>
    /// Permet de savoir si la manette droite est connectée
    /// </summary>
    public bool IsRightControllerConnected()
    {
        return rightHandDevice.isValid;
    }

    /// <summary>
    /// Renvoie True si la tête est traqué.
    /// </summary>
    public bool IsHeadTracked()
    {
        bool isTracked;
        headDevice.TryGetFeatureValue(CommonUsages.isTracked, out isTracked);
        return isTracked;
    }
    #endregion
    #region PRIVATE FUNCTIONS
    private void UpdateButtonStatus(InputDevice hand, InputFeatureUsage<bool> button, ref ButtonStatus buttonStatus)
    {
        bool isPressed = false;
        hand.TryGetFeatureValue(button, out isPressed);

        switch (buttonStatus)
        {
            case ButtonStatus.Inactive:
                if (isPressed)
                {
                    buttonStatus = ButtonStatus.Down;
                }
                break;
            case ButtonStatus.Down:
            case ButtonStatus.Active:
                if (isPressed)
                {
                    buttonStatus = ButtonStatus.Active;
                }
                else
                {
                    buttonStatus = ButtonStatus.Up;
                }
                break;
            case ButtonStatus.Up:
                if (isPressed)
                {
                    buttonStatus = ButtonStatus.Down;
                }
                else
                {
                    buttonStatus = ButtonStatus.Inactive;
                }
                break;
        }
    }

    private float GetButtonForce(InputDevice hand, InputFeatureUsage<float> button, float dead_zone = 0.01f)
    {
        float force;
        hand.TryGetFeatureValue(button, out force);
        if (force > dead_zone)
        {
            return force;
        }
        return 0f;
    }

    private Vector2 GetAxis(InputDevice hand, InputFeatureUsage<Vector2> axis, float dead_zone = 0.1f)
    {
        Vector2 value;
        hand.TryGetFeatureValue(axis, out value);
        if (value != Vector2.zero
            && (Mathf.Abs(value.x) >= dead_zone) || (Mathf.Abs(value.y) >= dead_zone))
        {
            return value;
        }
        return Vector2.zero;
    }

    private void SetVibration(InputDevice hand, float force, float duration)
    {
        if (hand.TryGetHapticCapabilities(out UnityEngine.XR.HapticCapabilities capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                hand.SendHapticImpulse(0u, force, duration);
            }
        }
    }
    #endregion
}
