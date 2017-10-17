using UnityEngine;
using UnityEngine.EventSystems;

public class dfButtonInput : MonoBehaviour
#if !UNITY_EDITOR
        , IPointerUpHandler, IPointerDownHandler
#endif
{
    public string _buttonName = "fire";

    /// <summary>
    /// Utility object that is registered in the system
    /// </summary>
    private dfVirtualButton _virtualButton;

    // Again some Unity Remote supporting stuff
    // if we are in the editor, add an input helper component
#if UNITY_EDITOR
    private void Awake()
    {
        gameObject.AddComponent<dfButtonInputHelper>();
    }
#endif

    /// <summary>
    /// It's pretty simple here
    /// When we enable, we register our button in the input system
    /// </summary>
    private void OnEnable()
    {
        _virtualButton = _virtualButton ?? new dfVirtualButton(_buttonName);
        dfInputManager.Instance.RegisterVirtualButton(_virtualButton);
    }

    /// <summary>
    /// When we disable, we unregister our button
    /// </summary>
    private void OnDisable()
    {
        dfInputManager.Instance.UnregisterVirtualButton(_virtualButton);
    }

    /// <summary>
    /// uGUI Event system stuff
    /// It's also utilised by the editor input helper
    /// </summary>
    /// <param name="eventData">Data of the passed event</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        _virtualButton.Release();
    }

    /// <summary>
    /// uGUI Event system stuff
    /// It's also utilised by the editor input helper
    /// </summary>
    /// <param name="eventData">Data of the passed event</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        _virtualButton.Press();
    }
}
