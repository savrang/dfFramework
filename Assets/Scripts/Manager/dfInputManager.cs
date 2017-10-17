using UnityEngine;
using System.Collections.Generic;

public class dfInputManager : msSingleton<dfInputManager>
{
    public enum InputType
    {
        JOYSTICK = 0x1
    }

    private const float AlmostZero = 0.00000001f;

    private Dictionary<string, List<dfVirtualAxis>> _virtualAxisDictionary = new Dictionary<string, List<dfVirtualAxis>>();

    private Dictionary<string, List<dfVirtualButton>> _virtualButtonsDictionary = new Dictionary<string, List<dfVirtualButton>>();

    public delegate void OnTouchUp();
    public OnTouchUp _delegateTouchUp;

    public delegate void OnTouchDown();
    public OnTouchDown _delegateTouchDown;





    public int TouchCount
    {
        get
        {
            return Input.touchCount;
        }
    }

    public void Create()
    {
    }

    public GameObject GetEventSystem()
    {
        return null;
    }

    public Touch GetTouch(int touchIndex)
    {
        return Input.GetTouch(touchIndex);
    }

    public void RegisterVirtualAxis(dfVirtualAxis virtualAxis)
    {
        // If it's the first such virtual axis, create a new list for that axis name
        if (!_virtualAxisDictionary.ContainsKey(virtualAxis._name))
        {
            _virtualAxisDictionary[virtualAxis._name] = new List<dfVirtualAxis>();
        }

        _virtualAxisDictionary[virtualAxis._name].Add(virtualAxis);
    }

    public void UnregisterVirtualAxis(dfVirtualAxis virtualAxis)
    {
        // If it's the first such virtual axis, create a new list for that axis name
        if (_virtualAxisDictionary.ContainsKey(virtualAxis._name))
        {
            if (!_virtualAxisDictionary[virtualAxis._name].Remove(virtualAxis))
            {
                Debug.LogError("Requested axis " + virtualAxis._name + " exists, but there's no such virtual axis that you're trying to unregister");
            }
        }
        else
        {
            Debug.LogError("Trying to unregister an axis " + virtualAxis._name + " that was never registered");
        }
    }


    public float GetAxis(string axisName)
    {
        return GetAxis(axisName, false);
    }

    private float GetAxis(string axisName, bool isRaw)
    {
        // If we have the axis registered as virtual, we call the retreival logic
        if (AxisExists(axisName))
        {
            return GetVirtualAxisValue(_virtualAxisDictionary[axisName], axisName, isRaw);
        }

        // If we don't have the desired virtual axis registered, we just fallback to the default Unity Input behaviour
        return isRaw ? Input.GetAxisRaw(axisName) : Input.GetAxis(axisName);
    }

    public bool AxisExists(string axisName)
    {
        return _virtualAxisDictionary.ContainsKey(axisName);
    }

    private float GetVirtualAxisValue(List<dfVirtualAxis> virtualAxisList, string axisName, bool isRaw)
    {
        // The method is really straightforward here
        // First, we check the standard Input.GetAxis method
        // If it's not zero, we return the value
        // If it IS zero, we return first non-zero value of any of the passed virtual axis
        // Or zero if all of them are zero

        float axisValue = isRaw ? Input.GetAxisRaw(axisName) : Input.GetAxis(axisName);
        if (Mathf.Abs(axisValue) > AlmostZero)
        {
            return axisValue;
        }

        for (int i = 0; i < virtualAxisList.Count; i++)
        {
            var currentAxisValue = virtualAxisList[i]._value;
            if (Mathf.Abs(currentAxisValue) > AlmostZero)
            {
                return currentAxisValue;
            }
        }

        return 0f;
    }


    public void RegisterVirtualButton(dfVirtualButton virtualButton)
    {
        if (!_virtualButtonsDictionary.ContainsKey(virtualButton._name))
        {
            _virtualButtonsDictionary[virtualButton._name] = new List<dfVirtualButton>();
        }

        _virtualButtonsDictionary[virtualButton._name].Add(virtualButton);
    }

    public void UnregisterVirtualButton(dfVirtualButton virtualButton)
    {
        if (_virtualButtonsDictionary.ContainsKey(virtualButton._name))
        {
            if (!_virtualButtonsDictionary[virtualButton._name].Remove(virtualButton))
            {
                Debug.LogError("Requested button axis exists, but there's no such virtual button that you're trying to unregister");
            }
        }
        else
        {
            Debug.LogError("Trying to unregister a button that was never registered");
        }
    }



    public bool GetButton(string buttonName)
    {
        // We first check the stadard Button behaviour
        var standardInputButtonState = Input.GetButton(buttonName);
        // If the stadard Unity Input button is being pressed, we just retur true
        if (standardInputButtonState == true) return true;

        // If not, we check our virtual buttons
        if (ButtonExists(buttonName))
        {
            return GetAnyVirtualButton(_virtualButtonsDictionary[buttonName]); ;
        }

        // If there is no such button registered, we return false;
        return false;
    }


    public bool GetButtonDown(string buttonName)
    {
        // We first check the stadard Button behaviour
        var standardInputButtonState = Input.GetButtonDown(buttonName);
        // If the stadard Unity Input button is being pressed, we just retur true
        if (standardInputButtonState == true) return true;

        // If not, we check our virtual buttons
        if (ButtonExists(buttonName))
        {
            return GetAnyVirtualButtonDown(_virtualButtonsDictionary[buttonName]);
        }

        // If there is no such button registered, we return false;
        return false;
    }

    public bool GetButtonUp(string buttonName)
    {
        // We first check the stadard Button behaviour
        var standardInputButtonState = Input.GetButtonDown(buttonName);
        // If the stadard Unity Input button is being pressed, we just retur true
        if (standardInputButtonState == true) return true;

        // If not, we check our virtual buttons
        if (ButtonExists(buttonName))
        {
            return GetAnyVirtualButtonUp(_virtualButtonsDictionary[buttonName]);
        }

        // If there is no such button registered, we return false;
        return false;
    }

    public bool ButtonExists(string buttonName)
    {
        return _virtualButtonsDictionary.ContainsKey(buttonName);
    }

    private static bool GetAnyVirtualButtonDown(List<dfVirtualButton> virtualButtons)
    {
        for (int i = 0; i < virtualButtons.Count; i++)
        {
            if (virtualButtons[i].GetButtonDown)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Simple logic for checking whether is any of the virtual buttons has been just released
    /// </summary>
    /// <param name="virtualButtons">Virtual buttons to search through</param>
    /// <returns>Is any of the buttons has just been released?</returns>
    private bool GetAnyVirtualButtonUp(List<dfVirtualButton> virtualButtons)
    {
        for (int i = 0; i < virtualButtons.Count; i++)
        {
            if (virtualButtons[i].GetButtonUp) return true;
        }

        return false;
    }

    /// <summary>
    /// Simple logic for checking whether is any of the virtual buttons is currently pressed
    /// </summary>
    /// <param name="virtualButtons">Virtual buttons to search through</param>
    /// <returns>Is any of the buttons currently pressed?</returns>
    private bool GetAnyVirtualButton(List<dfVirtualButton> virtualButtons)
    {
        for (int i = 0; i < virtualButtons.Count; i++)
        {
            if (virtualButtons[i].GetButton) return true;
        }

        return false;
    }
 
}
