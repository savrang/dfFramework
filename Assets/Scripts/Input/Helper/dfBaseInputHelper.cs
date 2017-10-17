using UnityEngine;
using UnityEngine.EventSystems;

public class dfBaseInputHelper : MonoBehaviour 
{
    protected PointerEventData _pointerEventData;


    protected int _lastFingerID = -1;

    protected Camera _uiRootCamera;

    protected virtual void Awake()
    {
        _pointerEventData = new PointerEventData(FindObjectOfType<EventSystem>());
        _uiRootCamera = GetComponentInParent<Canvas>().worldCamera;
    }
}
