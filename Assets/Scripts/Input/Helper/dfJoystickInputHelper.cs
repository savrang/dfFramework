

#if UNITY_EDITOR

using UnityEngine;
using System.Collections;

public class dfJoystickInputHelper : dfBaseInputHelper
{
    private dfJoystickInput _joystickInput;

    protected override void Awake()
    {
        base.Awake();

        _joystickInput = GetComponent<dfJoystickInput>();
        _joystickInput.CurrentEventCamera = _uiRootCamera;
    }

	
    public void Update()
    {
        // Now this is a bit tricky
        // As we are completely REPLACING the uGUI event system for the Editor, we need to handle both Touch and Mouse inputs

        // For every touch out there
        for (int i = 0; i < dfInputManager.Instance.TouchCount; i++)
        {
            var touch = dfInputManager.Instance.GetTouch(i);
            _pointerEventData.position = touch.position;

            // Check if it's inside our rectangle
            if (RectTransformUtility.RectangleContainsScreenPoint(_joystickInput.TouchZone, touch.position, _uiRootCamera))
            {
                // If it's inside, it's just started AND the joystick is not being tweaked yet
                if (touch.phase == TouchPhase.Began && _lastFingerID == -1)
                {
                    // We press the joystick
                    _joystickInput.OnPointerDown(_pointerEventData);
                    // Remember our pressed finger id
                    _lastFingerID = touch.fingerId;
                    return;
                }
            }

            // If it's just been lifted AND this is the finger that was tweaking this joystick
            if (touch.phase == TouchPhase.Ended && touch.fingerId == _lastFingerID)
            {
                // We release the joystick
                _joystickInput.OnPointerUp(_pointerEventData);
                // Reset finger ID so we can Press again
                _lastFingerID = -1;
                return;
            }

            if (touch.phase == TouchPhase.Moved && touch.fingerId == _lastFingerID)
            {
                _joystickInput.OnDrag(_pointerEventData);
                return;
            }
        }

        // Mouse input here
        // Same logic, but mouse is considered to be the finger with id of 255 so it's definitely won't interfere with actual fingers
        _pointerEventData.position = Input.mousePosition;
        if (RectTransformUtility.RectangleContainsScreenPoint(_joystickInput.TouchZone,
            _pointerEventData.position, _uiRootCamera))
        {
            if (Input.GetMouseButtonDown(0))
            {
                _joystickInput.OnPointerDown(_pointerEventData);
                _lastFingerID = 255;
                return;
            }
        }

        if (Input.GetMouseButtonUp(0) && _lastFingerID == 255)
        {
            _joystickInput.OnPointerUp(_pointerEventData);
            _lastFingerID = -1;
            return;
        }

        if (Input.GetMouseButton(0) && _lastFingerID == 255)
        {
            _joystickInput.OnDrag(_pointerEventData);
        }
    }
}

#endif