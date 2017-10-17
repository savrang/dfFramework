
#if UNITY_EDITOR

using UnityEngine;
using System.Collections;

public class dfButtonPadInputHelper : dfBaseInputHelper 
{
    private dfButtonPad _linkedDpad;

    protected override void Awake()
    {
        base.Awake();

        _linkedDpad = GetComponent<dfButtonPad>();
        _linkedDpad.CurrentEventCamera = _uiRootCamera;
    }

    private void Update()
    {
        // For every touch out there
        for (int i = 0; i < dfInputManager.Instance.TouchCount; i++)
        {
            var touch = dfInputManager.Instance.GetTouch(i);
            _pointerEventData.position = touch.position;
            _pointerEventData.pointerId = touch.fingerId;

            if (touch.phase == TouchPhase.Began)
            {
                _linkedDpad.OnPointerDown(_pointerEventData);
                return;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                _linkedDpad.OnPointerUp(_pointerEventData);
                return;
            }

        }
        // Mouse input here
        // Same logic, but mouse is considered to be the finger with id of 255 so it's definitely won't interfere with actual fingers
        _pointerEventData.position = Input.mousePosition;
        _pointerEventData.pointerId = 255;

        if (Input.GetMouseButtonDown(0))
        {
            _linkedDpad.OnPointerDown(_pointerEventData);
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _linkedDpad.OnPointerUp(_pointerEventData);
            return;
        }
    }
   
}

#endif