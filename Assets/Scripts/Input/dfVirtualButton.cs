using UnityEngine;
using System.Collections;

public class dfVirtualButton
{
        public string _name { get; set; }

        /// <summary>
        /// Is this button currently pressed?
        /// </summary>
        public bool _isPressed { get; private set; }

        /// <summary>
        /// The last frame this button was pressed
        /// </summary>
        private int _lastPressedFrame;

        /// <summary>
        /// The last frame this butto was released
        /// </summary>
        private int _lastReleasedFrame;

        public dfVirtualButton(string name)
        {
            _name = name;
            Release();
        }

        /// <summary>
        /// Press logic sets the current state of the button to "IsPressed" untill the Release() method is called
        /// </summary>
        public void Press()
        {
            if (_isPressed)
            {
                return;
            }
            _isPressed = true;
            _lastPressedFrame = Time.frameCount;
        }
        
        /// <summary>
        /// Release logic frees the button from its "IsPressed" state
        /// </summary>
        public void Release()
        {
            _isPressed = false;
            _lastReleasedFrame = Time.frameCount;
        }

        /// <summary>
        /// Is this button currently pressed?
        /// </summary>
        public bool GetButton
        {
            get { return _isPressed; }
        }

        /// <summary>
        /// Check whether this button has just been pressed 
        /// </summary>
        public bool GetButtonDown
        {
            get
            {
                return _lastPressedFrame - Time.frameCount == -1;
            }
        }

        /// <summary>
        /// Check whether this button has just been released 
        /// </summary>
        public bool GetButtonUp
        {
            get
            {
                return (_lastReleasedFrame == Time.frameCount - 1);
            }
        }
}
