using UnityEngine;
using UnityEngine.EventSystems;

public class dfButtonPad : MonoBehaviour
#if !UNITY_EDITOR
, IPointerDownHandler, IPointerUpHandler
#endif
{
    public dfButtonPadAxis[] _dpadAxis;

    /// <summary>
    /// Current event camera reference. Needed for the sake of Unity Remote input
    /// </summary>
    public Camera CurrentEventCamera { get; set; }

    private void Awake()
    {
#if UNITY_EDITOR
        gameObject.AddComponent<dfButtonPadInputHelper>();
#endif
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CurrentEventCamera = eventData.pressEventCamera ?? CurrentEventCamera;

        foreach (var dpadAxis in _dpadAxis)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(dpadAxis.RectTransform, eventData.position,
                CurrentEventCamera))
            {
                dpadAxis.Press(eventData.position, CurrentEventCamera, eventData.pointerId);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (var dpadAxis in _dpadAxis)
        {
            dpadAxis.TryRelease(eventData.pointerId);
        }
    }
}
