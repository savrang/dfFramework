using UnityEngine;
using System.Collections;

public class dfButtonPadAxis : MonoBehaviour 
{
    public string _axisName;
    public float _axisMultiplier;

    public RectTransform RectTransform { get; private set; }
    public int _lastFingerId { get; set; }
    private dfVirtualAxis _virtualAxis;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _virtualAxis = _virtualAxis ?? new dfVirtualAxis(_axisName);
        _lastFingerId = -1;

        dfInputManager.Instance.RegisterVirtualAxis(_virtualAxis);
    }

    private void OnDisable()
    {
        dfInputManager.Instance.UnregisterVirtualAxis(_virtualAxis);
    }

    public void Press(Vector2 screenPoint, Camera eventCamera, int pointerId)
    {
        _virtualAxis._value = Mathf.Clamp(_axisMultiplier, -1f, 1f);
        _lastFingerId = pointerId;
    }

    public void TryRelease(int pointerId)
    {
        if (_lastFingerId == pointerId)
        {
            _virtualAxis._value = 0f;
            _lastFingerId = -1;
        }
    }
}
